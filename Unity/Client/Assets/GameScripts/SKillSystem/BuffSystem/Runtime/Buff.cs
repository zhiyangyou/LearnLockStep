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
    private BuffRender _buffRender; // buffRender 

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
                Buff_Start(); // 1. 调用buffStart接口
                Buff_Trigger(); //2. 调用buff触发逻辑
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
        _buffRender?.OnRelease();
        _buffLogic.BuffEnd();
        BuffSystem.Instance.RemoveBuff(this); // 这个写法不是很合理
        attachTarget.RemoveBuff(this);
    }

    #endregion

    #region private

    private void Buff_Start() {
        CreateBuffEffect();
        if (BuffConfigSo.HasEffect) {
            _buffRender.InitBuffRender(releaser, attachTarget, BuffConfigSo, skill.skillGuidePos);
        }
        _buffLogic.BuffStart();
        attachTarget.AddBuff(this);

        // 播放音效
        if (BuffConfigSo.audioClip != null) {
            AudioController.GetInstance().PlaySoundByAudioClip(BuffConfigSo.audioClip, false, AudioPriorityConfig.Buff_AudioClip);
        }
    }

    private void Buff_Trigger() {
        // buff逻辑执行 
        _buffLogic.BuffTrigger();

        // 目标的受击动画
        switch (BuffConfigSo.buffTriggerAnim) {
            case ObjectAnimationState.None:
                // 不做任何处理
                break;
            case ObjectAnimationState.BeHit:
                attachTarget.PlayAnim(AnimaNames.Anim_Beiji_01);
                break;
            case ObjectAnimationState.Stiff:
                attachTarget.PlayAnim(AnimaNames.Anim_Beiji_02);
                break;
            default:
                Debug.LogError($"尚未实现的buffTriggerAnim类型:{BuffConfigSo.buffTriggerAnim}");
                break;
        }
    }

    /// <summary>
    /// 组合具体的buff实现逻辑
    /// </summary>
    private void CompositeBuffImpl() {
        var buffType = BuffConfigSo.buffType;
        if (buffType == BuffType.Repel) {
            _buffLogic = new Buff_Repel(this);
        }
        else if (buffType == BuffType.Floating) {
            _buffLogic = new Buff_Floating(this);
        }
        else if (buffType == BuffType.Stiff) {
            _buffLogic = new Buff_Stiff(this);
        }
        else if (buffType == BuffType.HP_Modify) {
            _buffLogic = new Buff_ModifyAttribute(this);
        }
        else if (buffType == BuffType.Grab) {
            _buffLogic = new Buff_Grab(this);
        }
        else if (buffType == BuffType.IgnoreGravity) {
            _buffLogic = new Buff_IgnoreGravity(this);
        }
        else if (buffType == BuffType.MoveSpeed_Modify_Single) {
            _buffLogic = new Buff_ModifyAttribute_Single(this);
        }
        else if (buffType == BuffType.AllowMove 
                 || buffType == BuffType.NotAllowModifyDir) {
            _buffLogic = new Buff_StatusModify_Single(this);
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
                Buff_Trigger();
                _curRealRuntime -= BuffConfigSo.intervalMS;
            }
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

    private BuffRender CreateBuffEffect() {
        // 读取配置, 生成特效
        if (BuffConfigSo.HasEffect) {
            var goBuffEffect = GameObject.Instantiate(BuffConfigSo.effectConfig.GoEffect);
            _buffRender = goBuffEffect.GetComponent<BuffRender>();
            if (_buffRender == null) {
                _buffRender = goBuffEffect.AddComponent<BuffRender>();
            }
            return _buffRender;
        }
        return null;
    }

    #endregion
}