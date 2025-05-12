using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZMGC.Battle;

public partial class LogicActor {
    #region 属性和字段

    private SkillSystem _skillSystem;

    // 普通攻击技能ID数组
    private int[] _normalAttackSkillArr = null;
    private int[] _normalSkillArr = null;

    private List<Skill> _listReleasingSkills = new();

    private List<Buff> _listBuff = new();

    /// <summary>
    /// 普通攻击连击
    /// </summary>
    private int _curNormalComboIndex = 0;

    private int curNormalComboIndex {
        get { return _curNormalComboIndex; }
        set {
            if (value == 0) {
                // Debug.LogError("连发普攻归0");
            }
            _curNormalComboIndex = value;
        }
    }

    // /// <summary>
    // /// 普攻重置Timer
    // /// </summary>
    // private int _timerNormalAttack = 0;

    #endregion

    #region public

    /// <summary>
    /// 是否有正在释放的技能
    /// </summary>
    public bool HasReleasingSkill => _listReleasingSkills != null && _listReleasingSkills.Count > 0;

    /// <summary>
    ///  初始化技能
    /// </summary>
    public void InitActorSkill() {
        _normalAttackSkillArr = BattleWorld.GetExitsDataMgr<HeroDataMgr>().GetHeroNormalSkillIDs(HeroIDConfig.HeroID_神枪手);
        _normalSkillArr = BattleWorld.GetExitsDataMgr<HeroDataMgr>().GetHeroSkillIDs(HeroIDConfig.HeroID_神枪手);
        _skillSystem = new SkillSystem(this);
        _skillSystem.InitSkills(_normalAttackSkillArr);
        _skillSystem.InitSkills(_normalSkillArr);
    }

    public void ReleaseNormalAttack() {
        ReleaseSkill(_normalAttackSkillArr[curNormalComboIndex], null);
    }


    public bool IsNormalSkill(int skillID) {
        return _normalAttackSkillArr.Contains(skillID);
    }

    public void ReleaseSkill(int skillID, Action<bool> onReleaseSkillResult) {
        var releasingSkill = _skillSystem.ReleaseSkill(skillID, SkillCallback_OnAfter, (skill, combineSkill) => {
            SkillCallback_OnEnd(skill, combineSkill);
            if (skill.SkillCfgConfig.SkillType == SkillType.StockPile) {
                onReleaseSkillResult?.Invoke(true);
            }
        });
        if (releasingSkill != null) // 技能释放成功
        {
            _listReleasingSkills.Add(releasingSkill);
            if (!IsNormalSkill(skillID)) {
                curNormalComboIndex = 0; // 重置普攻combo: 其他技能释放 Start阶段
            }
            ActionState = LogicObjectActionState.ReleasingSkill;
            if (releasingSkill.SkillCfgConfig.SkillType != SkillType.StockPile) {
                onReleaseSkillResult?.Invoke(true);
            }
        }
        else {
            onReleaseSkillResult?.Invoke(false);
        }
    }


    public void TriggerStockPileSkill(int skillId) {
        _skillSystem.TriggerStockPileSkill(skillId);
    }

    public Skill GetSkill(int skillID) {
        return _skillSystem.GetSkill(skillID);
    }

    #endregion

    #region buff相关

    /// <summary>
    /// 添加buff
    /// </summary>
    /// <param name="buff"></param>
    public void AddBuff(Buff buff) {
        _listBuff.Add(buff);
    }

    /// <summary>
    /// 移除Buff
    /// </summary>
    /// <param name="buff"></param>
    public void RemoveBuff(Buff buff) {
        _listBuff.Remove(buff);
        if (ObjectState == LogicObjectState.Death) {
            return;
        }
        if (_listBuff.Count == 0 && RenderObject.GetCurAnimName() != AnimaNames.Anim_Getup) {
            PlayAnim(AnimaNames.Anim_Idle);
            ActionState = LogicObjectActionState.Idle;
        }
    }

    #endregion

    #region private

    /// <summary>
    /// 逻辑帧: 技能
    /// </summary>
    private void OnLogicFrameUpdate_Skill() {
        _skillSystem.OnLogicFrameUpdate();
    }

    /// <summary>
    /// 回调:技能释放完成
    /// </summary>
    /// <param name="sk"></param>
    /// <param name="isCombineSkill"></param>
    private void SkillCallback_OnEnd(Skill sk, bool isCombineSkill) {
        if (_listReleasingSkills != null && _listReleasingSkills.Count == 0) {
            curNormalComboIndex = 0; // 重置普攻combo: 技能释放 End阶段
        }
        _listReleasingSkills.Remove(sk);
        ActionState = LogicObjectActionState.Idle;
    }

    /// <summary>
    /// 回调:技能释放开始后摇
    /// </summary>
    /// <param name="sk"></param>
    private void SkillCallback_OnAfter(Skill sk) {
        if (!IsNormalSkill(sk.SkillID)) {
            curNormalComboIndex = 0; // 重置普攻combo: 其他技能释放 After阶段
        }
        else {
            curNormalComboIndex++;
            // Debug.LogError($"curNormalComboIndex++ {curNormalComboIndex}");
            // 归零
            if (curNormalComboIndex >= _normalAttackSkillArr.Length) {
                curNormalComboIndex = 0; // 重置普攻combo: 普攻最后一段完成
            }
        }
    }

    #endregion
}