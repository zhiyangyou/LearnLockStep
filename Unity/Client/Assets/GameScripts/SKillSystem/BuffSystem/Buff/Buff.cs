using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZM.ZMAsset;

/// <summary>
/// 
/// </summary>
public enum BuffState {
    None,
    Delay, // 延迟中
    Start, // 开始
    Update, // 更新中
    End // 结束
}

public class Buff {
    #region 属性字段

    public BuffConfigSO BuffConfigSo { get; private set; }

    public BuffState buffState { get; private set; }

    /// <summary>
    /// 唯一id
    /// </summary>
    public int buffID { get; private set; }

    /// <summary>
    /// 释放者
    /// </summary>
    public LogicActor releaser { get; private set; }

    /// <summary>
    /// 附加目标
    /// </summary>
    public LogicActor attachTarget { get; private set; }

    /// <summary>
    /// 所属技能
    /// </summary>
    public Skill skill { get; private set; }

    /// <summary>
    /// buff参数
    /// </summary>
    public object[] paramObjs { get; private set; }

    private int _curLeftDelayMS = 0; // 当前剩余延迟时间

    private BuffComposite _buffLogic; // buff逻辑 (组合的对象)

    private int _curRealRuntime = 0; // 当前真实运行时间
    private int _curAccRuntime = 0; // 当前累积运行时间

    #endregion

    #region life-cycle

    public Buff(int buffID,
        LogicActor releaser,
        LogicActor attachTarget,
        Skill skill,
        object[] paramObjs
    ) {
        this.buffID = buffID;
        this.releaser = releaser;
        this.attachTarget = attachTarget;
        this.skill = skill;
        this.paramObjs = paramObjs;
    }

    public void OnCreate() {
        var soAssetPath = $"{AssetsPathConfig.Buff_Data_Path}/{buffID}.asset";
        BuffConfigSo = ZMAsset.LoadScriptableObject<BuffConfigSO>(soAssetPath);
        CompositeBuffImpl();
        buffState = BuffConfigSo.IsDelayBuff ? BuffState.Delay : BuffState.Start;
        _curLeftDelayMS = BuffConfigSo.delayMS;
        _curRealRuntime = 0;
        _curAccRuntime = 0;
    }


    public void OnLogicFrameUpdate() {
        switch (buffState) {
            case BuffState.None:
                break;
            case BuffState.Delay: {
                if (_curLeftDelayMS == BuffConfigSo.delayMS) {
                    // 处理buff延迟逻辑
                    _buffLogic.BuffDelay();
                }
                _curLeftDelayMS -= LogicFrameConfig.LogicFrameIntervalMS;
                if (_curLeftDelayMS <= 0) {
                    buffState = BuffState.Start;
                }
            }
                break;
            case BuffState.Start: {
                // 1. 调用buffStart接口
                _buffLogic.BuffStart();
                // 2. 调用buff触发逻辑
                _buffLogic.BuffTrigger();

                // 3. 判断buff是否需要进入 Update状态, 有限时长buff, 无限buff需要进入, 一次性buff不需要
                buffState = (BuffConfigSo.IsForeverBuff || BuffConfigSo.IsLimitTimeBuff)
                    ? BuffState.Update
                    : BuffState.End;
            }
                break;
            case BuffState.Update: {
                UpdateBuffLogic();
            }
                break;
            case BuffState.End: {
                // _buffLogic.BuffEnd(); // BuffEnd()放在OnDestory中去调用
                OnDestory();
            }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }


    public void OnDestory() {
        _buffLogic.BuffEnd();
        BuffSystem.Instance.RemoveBuff(this); // 这个写法不是很合理
    }

    #endregion

    #region private

    /// <summary>
    /// 组合具体的buff实现逻辑
    /// </summary>
    private void CompositeBuffImpl() {
        var buffType = BuffConfigSo.buffType;
        if (buffType == BuffType.Repel) {
            _buffLogic = new Buff_Repel(this);
        }
        else {
            Debug.LogError($"尚未实现的buff类型:{buffType}");
        }
    }

    private void UpdateBuffLogic() {
        int logicFrameIntervalMS = LogicFrameConfig.LogicFrameIntervalMS;
        // 1. 处理buff间隔逻辑
        if (BuffConfigSo.intervalMS > 0) {
            _curRealRuntime += logicFrameIntervalMS;
            if (_curRealRuntime >= BuffConfigSo.intervalMS) {
                _buffLogic.BuffTrigger();
                _curRealRuntime -= BuffConfigSo.intervalMS;
            }
        }
        else {
            // 如果没有配置间隔, 不是应该每一帧都去触发buff吗?
            _buffLogic.BuffTrigger();
        }
        // 累计运行时间是否大于间隔
        UpdateBuffDurationTime();
    }

    private void UpdateBuffDurationTime() {
        _curAccRuntime += LogicFrameConfig.LogicFrameIntervalMS;
        if (_curAccRuntime >= BuffConfigSo.DurationMS) {
            buffState = BuffState.End;
        }
    }

    #endregion
}