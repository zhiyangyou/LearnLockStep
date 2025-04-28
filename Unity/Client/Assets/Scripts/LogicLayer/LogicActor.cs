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
        CaculateDamage(hp, DamageSource.Skill);
    }

    /// <summary>
    /// 计算伤害
    /// </summary>
    /// <param name="hp"></param>
    /// <param name="damageSource"></param>
    public void CaculateDamage(FixInt hp, DamageSource damageSource) {
        if (ObjectState == LogicObjectState.Death) {
            return;
        }
        // 1. 血量减少
        // 2. 判断死亡
        // 3. 飘字渲染
        RenderObject.Damage(hp.RawInt, damageSource);
    }

    public override void OnDestory() {
        base.OnDestory();
    }

    #endregion

    #region public

    public void PlayAnim(AnimationClip animationClip) {
        RenderObject.PlayAnim(animationClip);
    }

    public virtual void OnHit(GameObject goEffect, int survialTimeMS, LogicActor sourceActor) {
        RenderObject.OnHit(goEffect, survialTimeMS,  sourceActor);
    }

    public void SetRenderData(RenderObject renderObject) {
        this.RenderObject = renderObject;
    }

    #endregion
}