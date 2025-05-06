using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 技能系统
/// </summary>
public class SkillSystem {
    #region 属性和字段

    private LogicActor _skillCreater = null;

    private List<Skill> _listSkills = null;

    #endregion

    #region public

    public SkillSystem(LogicActor logicActor) {
        _listSkills = new();
        _skillCreater = logicActor;
    }

    public void InitSkills(int[] arrSkillID) {
        foreach (var skillID in arrSkillID) {
            Skill skill = new Skill(skillID, _skillCreater);
            if (skill.SkillCfgConfig.HasCombineSkill) {
                InitSkills(new int[1] { skill.SkillCfgConfig.CombinationSkillId });
            }

            if (skill.SkillCfgConfig.SkillType == SkillType.StockPile && skill.SkillCfgConfig.stockPIleStageDatas.Count > 0) {
                foreach (var data in skill.SkillCfgConfig.stockPIleStageDatas) {
                    if (data.skillId > 0) {
                        InitSkills(new int[1] { data.skillId });
                    }
                }
                // InitSkills(skill.SkillCfgConfig.stockPIleStageDatas.Where(data => data.skillId > 0).Select(data => data.skillId).ToArray());
            }
            _listSkills.Add(skill);
        }
        // Debug.Log($"技能初始化完成 个数: {arrSkillID.Length}");
    }

    public Skill GetSkill(int skillID) {
        foreach (var skill in _listSkills) {
            if (skill.SkillID == skillID) {
                return skill;
            }
        }
        return null;
    }

    public void TriggerStockPileSkill(int skillID) {
        Skill skill = GetSkill(skillID);
        if (skill != null) {
            skill.TriggerStockPileSkill();
        }
    }

    public Skill ReleaseSkill(int skillID, SkillCallback_OnAfter onAfter, SkillCallback_OnEnd onEnd) {
        foreach (Skill skill in _listSkills) {
            if (skillID == skill.SkillID) {
                if (skill.skillState != SkillState.None && skill.skillState != SkillState.End) {
                    Debug.LogError($"技能正在释放中{skillID} ");
                    return null;
                }
                // Debug.LogError($"release skill {skillID}");
                skill.ReleaseSkill(onAfter, (skRelease, isCombineSkill) => {
                    onEnd.Invoke(skRelease, isCombineSkill);
                    if (isCombineSkill) {
                        // TODO 根据技能组合的情况 处理组合逻辑
                    }
                });
                return skill;
            }
        }
        Debug.LogError($"释放技能失败 skillID:{skillID}");
        return null;
    }

    public void OnLogicFrameUpdate() {
        foreach (Skill sk in _listSkills) {
            sk.OnLogicFrameUpdate();
        }
    }

    #endregion
}