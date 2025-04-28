using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class LogicActor {
    #region 属性和字段

    private SkillSystem _skillSystem;

    // 普通攻击技能ID数组
    private int[] _normalSkillArr = new[] { 1001, 1002, 1003 };

    private List<Skill> _listReleasingSkills = new();

    /// <summary>
    /// 普通攻击连击
    /// </summary>
    private int _curNormalComboIndex = 0;

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
        _skillSystem = new SkillSystem(this);
        _skillSystem.InitSkills(_normalSkillArr);
    }

    public void ReleaseNormalAttack() {
        ReleaseSkill(_normalSkillArr[_curNormalComboIndex]);
    }


    public bool IsNormalSkill(int skillID) {
        return _normalSkillArr.Contains(skillID);
    }

    public void ReleaseSkill(int skillID) {
        var releasingSkill = _skillSystem.ReleaseSkill(skillID, SkillCallback_OnAfter, SkillCallback_OnEnd);
        if (releasingSkill != null) // 技能释放成功
        {
            _listReleasingSkills.Add(releasingSkill);
            if (!IsNormalSkill(skillID)) {
                _curNormalComboIndex = 0;
            }
        }
    }


    /// <summary>
    /// 逻辑帧: 技能
    /// </summary>
    public void OnLogicFrameUpdate_Skill() {
        _skillSystem.OnLogicFrameUpdate();
    }

    #endregion

    #region private

    /// <summary>
    /// 回调:技能释放完成
    /// </summary>
    /// <param name="sk"></param>
    /// <param name="isCombineSkill"></param>
    public void SkillCallback_OnEnd(Skill sk, bool isCombineSkill) {
        _listReleasingSkills.Remove(sk);
        if (IsNormalSkill(sk.SkillID)) {
            // _curNormalComboIndex = 0; // 连击按钮过长,中断
            // Debug.LogError("连击按钮过长,中断");
        }
    }

    /// <summary>
    /// 回调:技能释放开始后摇
    /// </summary>
    /// <param name="sk"></param>
    public void SkillCallback_OnAfter(Skill sk) {
        if (!IsNormalSkill(sk.SkillID)) {
            _curNormalComboIndex = 0;
        }
        else {
            _curNormalComboIndex++;
            // 归零
            if (_curNormalComboIndex >= _normalSkillArr.Length) {
                _curNormalComboIndex = 0;
            }
        }
    }

    #endregion
}