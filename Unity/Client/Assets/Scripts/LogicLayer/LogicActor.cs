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
        OnLogicFrameUpdate_Gravity();
        OnLogicFrameUpdate_Skill();
        OnLogicFrameUpdate_Bullet();
    }

    public virtual void SkillDamage(FixInt hp, SkillConfig_Damage configDamage) {
        CaculateDamage(hp, DamageSource.Skill);
    }

    public virtual void BuffDamage(FixInt hp, SkillConfig_Damage configDamage) {
        // Debug.LogError($"buffDamage, hp:{hp}");
        CaculateDamage(hp, DamageSource.Buff);
    }

    public virtual void BulletDamage(FixInt hp, SkillConfig_Damage configDamage) {
        CaculateDamage(hp, DamageSource.Bullet);
        // Debug.LogError($"BulletDamage, hp:{hp}");
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
        
        // 3. 处理表现: 飘字 && 血条
        RenderObject.Damage(hp.RawInt, damageSource);
        
        // 1. 血量减少
        ReduceHp(hp);
        // 2. 判断死亡
        if (this.hp <= 0) {
            this.FixIntBoxCollider.Active = false;
            ObjectState = LogicObjectState.Death;
            RenderObject.OnDeath();
        }
       
    }


    /// <summary>
    /// 浮空
    /// </summary>
    /// <param name="isUpFloating">是否是上浮空</param>
    public virtual void State_Floating(bool isUpFloating) { }

    /// <summary>
    /// 对线触地
    /// </summary>
    public virtual void State_TriggerGrounding() { }

    public override void OnDestory() {
        base.OnDestory();
    }

    #endregion

    #region public

    public void PlayAnim(string clipName) {
        RenderObject.PlayAnim(clipName);
    }

    public void PlayAnim(AnimationClip animationClip) {
        RenderObject.PlayAnim(animationClip);
    }

    public virtual void OnHit(GameObject goEffect, int survialTimeMS, LogicObject sourceObj, FixInt logicAxisX) {
        RenderObject.OnHit(goEffect, survialTimeMS, sourceObj);
    }

    public void SetRenderData(RenderObject renderObject) {
        this.RenderObject = renderObject;
    }

    #endregion
}