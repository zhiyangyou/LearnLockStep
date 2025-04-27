using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能系统
/// </summary>
public class SkillSystem
{
    #region 属性和字段

    private LogicActor _skillCreater = null;

    private List<Skill> _listSkills = null;

    #endregion

    #region public

    public SkillSystem(LogicActor logicActor)
    {
        _listSkills = new();
        _skillCreater = logicActor;
    }

    public void InitSkills(int[] arrSkillID)
    {
        foreach (var skillID in arrSkillID)
        {
            Skill skill = new Skill(skillID, _skillCreater);
            _listSkills.Add(skill);
        }
        // Debug.Log($"技能初始化完成 个数: {arrSkillID.Length}");
    }

    public Skill ReleaseSkill(int skillID, SkillCallback_OnAfter onAfter, SkillCallback_OnEnd onEnd)
    {
        foreach (Skill sk in _listSkills)
        {
            if (skillID == sk.SkillID)
            {
                sk.ReleaseSkill(onAfter, (skRelease, isCombineSkill) =>
                {
                    onEnd.Invoke(skRelease, isCombineSkill);
                    if (isCombineSkill)
                    {
                        // TODO 根据技能组合的情况 处理组合逻辑
                    }
                });
                return sk;
            }
        }
        Debug.LogError($"释放技能失败 skillID:{skillID}");
        return null;
    }

    public void OnLogicFrameUpdate()
    {
        foreach (Skill sk in _listSkills)
        {
            sk.OnLogicFrameUpdate();
        }
    }

    #endregion
}