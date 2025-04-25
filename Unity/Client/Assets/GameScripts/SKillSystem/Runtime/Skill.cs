using System;
using ZM.ZMAsset;


public delegate void SkillAfterCallback(Skill skill);

public delegate void SkillEndCallback(Skill skill, bool isStockPile);

public class Skill
{
    #region 属性字段

    // 技能id
    private int _skillId;

    // 释放技能者
    private LogicActor _skillCreater;

    // 配置数据
    private SkillDataSO _skillData;

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
        SkillStart();
        PlayAni();
    }

    /// <summary>
    /// 播放技能动画
    /// </summary>
    public void PlayAni()
    {
        // 播放角色动画
    }

    /// <summary>
    /// 技能前摇
    /// </summary>
    public void SkillStart()
    {
        // 初始化技能数据
    }

    /// <summary>
    /// 技能后摇
    /// </summary>
    public void SkillAfter()
    {
    }

    /// <summary>
    /// 技能释放结束
    /// </summary>
    public void SkillEnd()
    {
    }

    #endregion

    #region private

    #endregion
}