using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LogicActor
{
    #region 属性和字段

    private SkillSystem _skillSystem;

    // 普通攻击技能ID数组
    private int[] _normalSkillArr = new[] { 1001, };

    #endregion

    #region public

    public void InitActorSkill()
    {
        _skillSystem = new SkillSystem(this);
        _skillSystem.InitSkills(_normalSkillArr);
    }

    public void ReleaseSkill(int skillID)
    {
        _skillSystem.ReleaseSkill(skillID, SkillCallback_OnAfter, SkillCallback_OnEnd);
    }


    /// <summary>
    /// 逻辑帧: 技能
    /// </summary>
    public void OnLogicFrameUpdate_Skill()
    {
        _skillSystem.OnLogicFrameUpdate();
    }

    #endregion

    #region private

    public void SkillCallback_OnEnd(Skill sk, bool isCombineSkill)
    {
    }

    public void SkillCallback_OnAfter(Skill sk)
    {
    }

    #endregion
}