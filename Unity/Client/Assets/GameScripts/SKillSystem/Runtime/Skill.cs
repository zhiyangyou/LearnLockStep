using System;
using ZM.ZMAsset;


public delegate void SkillAfterCallback(Skill skill);

public delegate void SkillEndCallback(Skill skill, bool isStockPile);


public enum SkillState
{
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

public partial class Skill
{
    #region 属性字段

    // 技能id
    private int _skillId;

    // 释放技能者
    private LogicActor _skillCreater;

    // 配置数据
    private SkillDataSO _skillData;

    private SkillState _skillState = SkillState.None;

    // 当前逻辑帧
    private int _curLogicFrame = 0;

    private int _curLogicFrameAccTimeMS = 0;

    public SkillAfterCallback _skillAfterCallback;
    public SkillEndCallback _skillEndCallback;

    #endregion

    #region public

    public Skill(int skillId, LogicActor skillCreater)
    {
        _skillId = skillId;
        _skillCreater = skillCreater;
        var configPath = $"{AssetsPathConfig.Skill_Data_Path}/{skillId}.asset";
        ZMAsset.LoadScriptableObject<SkillDataSO>(configPath);
    }

    /// <summary>
    /// 释放技能
    /// 1. SkillStart前摇开始(同时播放动画 PlayAni)
    /// 2. 触发技能
    /// 3. SkillAfter后摇开始
    /// 4. SkillEnd 技能结束
    /// </summary>
    public void ReleaseSkill(SkillAfterCallback onSkillAfter, SkillEndCallback onSkillEnd)
    {
        _skillAfterCallback = onSkillAfter;
        _skillEndCallback = onSkillEnd;
        _skillState = SkillState.Before;
        SkillStart();
        PlayAni();
    }

    /// <summary>
    /// 播放技能动画
    /// </summary>
    public void PlayAni()
    {
        // 播放角色动画
        _skillCreater.PlayAnim(_skillData.character.skillAnim);
    }

    /// <summary>
    /// 技能前摇
    /// </summary>
    public void SkillStart()
    {
        // 初始化技能数据
        _curLogicFrame = 0;
        _curLogicFrameAccTimeMS = 0;
    }

    /// <summary>
    /// 技能后摇
    /// </summary>
    public void SkillAfter()
    {
        _skillState = SkillState.After;
    }

    /// <summary>
    /// 技能释放结束
    /// </summary>
    public void SkillEnd()
    {
        _skillState = SkillState.End;
    }

    #endregion

    /// <summary>
    /// 逻辑帧更新
    /// </summary>
    public void OnLogicFrameUpdate()
    {
        _curLogicFrameAccTimeMS = _curLogicFrame * LogicFrameConfig.LogicFrameIntervalMS;

        // 尝试进入技能后摇
        if (_skillState == SkillState.Before
            && _curLogicFrameAccTimeMS >= _skillData.SkillCfg.skillShakeBeforeTimeMs
           )
        {
            SkillAfter();
        }

        // 更新不同配置的逻辑帧, 处理不同配置的逻辑

        //  更新特效逻辑帧
        OnLogicFrameUpdate_Effect();
        
        // 更新伤害

        // 更新行动逻辑帧

        // 更新音效逻辑帧

        // 更新子弹逻辑帧

        // 计数自增
        _curLogicFrame++;
    }

    #region private

    #endregion
}