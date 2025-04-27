using System.Collections;
using System.Collections.Generic;
using FixMath;
using UnityEngine;

public partial class LogicActor : LogicObject {
    #region life-cycle

    public override void OnCreate() {
        base.OnCreate();
        InitActorSkill();
    }

    public override void OnLogicFrameUpdate() {
        base.OnLogicFrameUpdate();
        OnLogicFrameUpdate_Move();
        OnLogicFrameUpdate_Skill();
        OnLogicFrameUpdate_Gravity();
    }

    public virtual void SkillDamage(FixInt hp, SkillDamageConfig damageConfig) {
        Debug.LogError($"skill damaga hp :{hp} config{damageConfig.triggerSkillId}");
    }

    public override void OnDestory() {
        base.OnDestory();
    }

    #endregion

    #region public

    public void PlayAnim(AnimationClip animationClip) {
        RenderObject.PlayAnim(animationClip);
    }

    public void SetRenderData(RenderObject renderObject) {
        this.RenderObject = renderObject;
    }

    #endregion
}