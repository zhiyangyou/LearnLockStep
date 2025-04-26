using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LogicActor
{
    #region 属性和字段

    private SkillSystem _skillSystem;

    // 普通攻击技能ID数组
    private int[] _normalSkillArr = new[] { 1001, };

    private List<Skill> _listReleasingSkills = new();

    #endregion

    #region public

    /// <summary>
    /// 是否有正在释放的技能
    /// </summary>
    public bool HasReleasingSkill => _listReleasingSkills != null && _listReleasingSkills.Count > 0;

    /// <summary>
    ///  初始化技能
    /// </summary>
    public void InitActorSkill()
    {
        _skillSystem = new SkillSystem(this);
        _skillSystem.InitSkills(_normalSkillArr);
    }

    public void ReleaseSkill(int skillID)
    {
        var releasingSkill = _skillSystem.ReleaseSkill(skillID, SkillCallback_OnAfter, SkillCallback_OnEnd);
        if (releasingSkill != null) // 技能释放成功
        {
            _listReleasingSkills.Add(releasingSkill);
        }
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

    /// <summary>
    /// 回调:技能释放完成
    /// </summary>
    /// <param name="sk"></param>
    /// <param name="isCombineSkill"></param>
    public void SkillCallback_OnEnd(Skill sk, bool isCombineSkill)
    {
        Debug.LogError("SkillCallback_OnEnd call ... ");
        _listReleasingSkills.Remove(sk);
    }

    /// <summary>
    /// 回调:技能释放开始后摇
    /// </summary>
    /// <param name="sk"></param>
    public void SkillCallback_OnAfter(Skill sk)
    {
    }

    #endregion
}