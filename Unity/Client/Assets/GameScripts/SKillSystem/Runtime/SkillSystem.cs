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

    private Skill _curReleasingSkill = null;

    private Skill curReleasingSkill {
        get { return _curReleasingSkill; }
        set {
            var curName = value == null ? "null" : $"{value.SkillCfgConfig.skillName}";
            // Debug.LogError($"当前正在释放的技能:{curName} frame:{Time.frameCount} ");
            _curReleasingSkill = value;
        }
    }

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
        // Debug.LogError($"TriggerStockPileSkill SkillStateMutex:{SkillStateMutex()}");
        if (SkillStateMutex(skillID)) {
            return;
        }
        Skill skill = GetSkill(skillID);
        if (skill != null) {
            skill.TriggerStockPileSkill();
        }
        else {
            Debug.LogError("skill is null ");
        }
    }

    public Skill ReleaseSkill(int skillID, SkillCallback_OnAfter onAfter, SkillCallback_OnEnd onEnd) {
        
        if (SkillStateMutex(skillID)) {
            return null;
        }
        // Debug.LogError($"release skill {skillID}");
        foreach (Skill skill in _listSkills) {
            if (skillID == skill.SkillID) {
                if (skill.skillState != SkillState.None && skill.skillState != SkillState.End) {
                    // Debug.LogError($"技能正在释放中{skillID} ");
                    return null;
                }
                skill.ReleaseSkill((afterSkill) => {
                    onAfter(afterSkill);
                    curReleasingSkill = null;
                }, (skRelease, isCombineSkill) => {
                    onEnd.Invoke(skRelease, isCombineSkill);
                    if (!isCombineSkill) {
                        // TODO 根据技能组合的情况 处理组合逻辑
                    }
                });
                curReleasingSkill = skill;
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

    #region private

    private bool SkillStateMutex(int willPlaySkillID) {
        if (curReleasingSkill != null
            && curReleasingSkill.skillState == SkillState.Before
            && curReleasingSkill.SkillID != willPlaySkillID
           ) {
            // Debug.LogError($"{curReleasingSkill.SkillCfgConfig.skillName} 技能前摇中");
            return true;
        }
        return false;
    }

    #endregion
}