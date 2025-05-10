using System;
using UnityEngine;
using ZM.ZMAsset;


public delegate void SkillCallback_OnAfter(Skill skill);

public delegate void SkillCallback_OnEnd(Skill skill, bool isCombineSkill);


public enum SkillState {
    None,

    // 前摇
    Before,

    /// <summary>
    /// 后摇
    /// </summary>
    After,

    /// <summary>
    /// 结束
    /// </summary>
    End,
}

public partial class Skill {
    #region 属性字段

    // 技能id
    public int SkillID { get; private set; }

    // 释放技能者
    private LogicActor _skillCreater;

    // 配置数据
    private SkillConfigSO _skillConfig;

    public SkillState skillState { get; private set; } = SkillState.None;

    public SkillConfig SkillCfgConfig => _skillConfig.skillCfg;

    // 当前逻辑帧
    private int _curLogicFrame = 0;

    private int _curLogicFrameAccTimeMS = 0;

    public SkillCallback_OnAfter SkillCallbackOnAfter;
    public SkillCallback_OnEnd SkillCallbackOnEnd;

    /// <summary>
    /// 是否自动匹配蓄力阶段
    /// </summary>
    private bool _isAutoMatchStockStage = false;

    #endregion

    #region public

    public Skill(int skillId, LogicActor skillCreater) {
        SkillID = skillId;
        _skillCreater = skillCreater;
        var configPath = $"{AssetsPathConfig.Skill_Data_Path}/{skillId}.asset";
        _skillConfig = ZMAsset.LoadScriptableObject<SkillConfigSO>(configPath);
    }

    /// <summary>
    /// 释放技能
    /// 1. SkillStart前摇开始(同时播放动画 PlayAni)
    /// 2. 触发技能
    /// 3. SkillAfter后摇开始
    /// 4. SkillEnd 技能结束
    /// </summary>
    public void ReleaseSkill(SkillCallback_OnAfter onSkillCallbackOnAfter, SkillCallback_OnEnd onSkillCallbackOnEnd) {
        SkillCallbackOnAfter = onSkillCallbackOnAfter;
        SkillCallbackOnEnd = onSkillCallbackOnEnd;
        skillState = SkillState.Before;
        SkillStart();
        PlayAni();
    }

    /// <summary>
    /// 播放技能动画
    /// </summary>
    public void PlayAni() {
        // 播放角色动画
        _skillCreater.PlayAnim(_skillConfig.configCharacter.skillAnim);
    }

    /// <summary>
    /// 技能前摇
    /// </summary>
    public void SkillStart() {
        // 初始化技能数据
        _isAutoMatchStockStage = false;
        InitTimer();
    }

    /// <summary>
    /// 技能后摇
    /// </summary>
    public void SkillAfter() {
        skillState = SkillState.After;
        SkillCallbackOnAfter?.Invoke(this);
    }

    /// <summary>
    /// 技能释放结束
    /// </summary>
    public void SkillEnd() {
        skillState = SkillState.End;
        this.SkillCallbackOnEnd?.Invoke(this, _skillConfig.skillCfg.HasCombineSkill); // TODO 暂且都是false 2025年4月26日18:10:48 
        ReleaseAllEffect();
        // 组合技能
        if (_skillConfig.skillCfg.HasCombineSkill) {
            _skillCreater.ReleaseSkill(_skillConfig.skillCfg.CombinationSkillId, null);
        }
        InitTimer();
    }


    /// <summary>
    /// 逻辑帧更新
    /// </summary>
    public void OnLogicFrameUpdate() {
        if (skillState is SkillState.None or SkillState.End) {
            // Debug.LogError($"技能结束了 {skillState}");
            return;
        }

        _curLogicFrameAccTimeMS = _curLogicFrame * LogicFrameConfig.LogicFrameIntervalMS;

        // 尝试进入技能后摇
        if (skillState == SkillState.Before
            && _curLogicFrameAccTimeMS >= _skillConfig.skillCfg.skillShakeBeforeTimeMs
            && _skillConfig.skillCfg.SkillType != SkillType.StockPile // 蓄力技能没有后摇
           ) {
            SkillAfter();
        }

        // 更新不同配置的逻辑帧, 处理不同配置的逻辑

        //  更新特效逻辑帧
        OnLogicFrameUpdate_Effect();

        // 更新伤害
        OnLogicFrameUpdate_Damage();

        // 更新行动逻辑帧
        OnLogicFrameUpdate_Action();

        // 更新音效逻辑帧
        OnLogicFrameUpdate_Audio();

        // 更新子弹逻辑帧
        OnLogicFrameUpdate_Buttle();

        // 蓄力技能, 和蓄力时间相关, 所以和蓄力结束帧无关
        if (_skillConfig.skillCfg.SkillType == SkillType.StockPile) {
            var stockPileDataCount = _skillConfig.skillCfg.stockPIleStageDatas.Count;
            if (stockPileDataCount > 0) {
                // Debug.LogError($"_isAutoMatchStockStage:{_isAutoMatchStockStage}");
                // 1. 情况: 按下立马抬起
                if (_isAutoMatchStockStage) {
                    foreach (var stockData in _skillConfig.skillCfg.stockPIleStageDatas) {
                        if (_curLogicFrameAccTimeMS >= stockData.startTimeMs) {
                            StockPileFinish(stockData);
                        }
                    }
                }
                else {
                    // 2. 情况: 蓄力超时
                    var lastStage = _skillConfig.skillCfg.stockPIleStageDatas[stockPileDataCount - 1];
                    if (_curLogicFrameAccTimeMS >= lastStage.endTimeMs) {
                        // Debug.LogError($"_isAutoMatchStockStage --- true;;; ");
                        StockPileFinish(lastStage);
                    }
                }
            }
        }
        else {
            // 非蓄力技能

            if (_curLogicFrame == _skillConfig.configCharacter.logicFrame) {
                SkillEnd();
            }
        }

        // 计数自增
        _curLogicFrame++;
    }

    /// <summary>
    ///  主动触发蓄力技能
    /// </summary>
    public void TriggerStockPileSkill() {
        // 蓄力时间符合某个阶段, 直接触发
        foreach (StockPileStageData stageData in _skillConfig.skillCfg.stockPIleStageDatas) {
            if (_curLogicFrameAccTimeMS >= stageData.startTimeMs && _curLogicFrameAccTimeMS <= stageData.endTimeMs) {
                StockPileFinish(stageData);
                return;
            }
        }

        // 不符合某个阶段
        _isAutoMatchStockStage = true;
        // Debug.LogError("蓄力:不符合任意阶段");
    }

    #endregion

    #region private

    private void InitTimer() {
        _curLogicFrame = 0;
        _curLogicFrameAccTimeMS = 0;
        _curDamageAccTimeMS = 0;
    }

    private void StockPileFinish(StockPileStageData stageData) {
        SkillEnd();
        // Debug.LogError("蓄力结束");
        if (stageData.skillId <= 0) {
            Debug.LogError($"蓄力阶段id:{stageData.stage} 配置了错误的技能id:{stageData.skillId}");
        }
        else {
            // Debug.LogError($"蓄力结束: 阶段id{stageData.skillId} skillid:{stageData.skillId}");
            _skillCreater.ReleaseSkill(stageData.skillId, null);
        }
    }

    #endregion
}