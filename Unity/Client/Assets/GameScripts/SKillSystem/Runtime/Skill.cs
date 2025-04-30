using System;
using ZM.ZMAsset;


public delegate void SkillCallback_OnAfter(Skill skill);

public delegate void SkillCallback_OnEnd(Skill skill, bool isStockPile);


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
        _curLogicFrame = 0;
        _curLogicFrameAccTimeMS = 0;
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
        this.SkillCallbackOnEnd?.Invoke(this, false); // TODO 暂且都是false 2025年4月26日18:10:48 
        // 组合技能
        if (_skillConfig.skillCfg.HasCombineSkill) {
            _skillCreater.ReleaseSkill(_skillConfig.skillCfg.CombinationSkillId);
        }
    }


    /// <summary>
    /// 逻辑帧更新
    /// </summary>
    public void OnLogicFrameUpdate() {
        if (skillState == SkillState.None) return;

        _curLogicFrameAccTimeMS = _curLogicFrame * LogicFrameConfig.LogicFrameIntervalMS;

        // 尝试进入技能后摇
        if (skillState == SkillState.Before
            && _curLogicFrameAccTimeMS >= _skillConfig.skillCfg.skillShakeBeforeTimeMs
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

        if (_curLogicFrame == _skillConfig.configCharacter.logicFrame) {
            SkillEnd();
        }

        // 计数自增
        _curLogicFrame++;
    }

    #endregion

    #region private

    #endregion
}