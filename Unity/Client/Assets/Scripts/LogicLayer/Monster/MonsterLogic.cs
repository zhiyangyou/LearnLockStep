using System.Collections;
using System.Collections.Generic;
using FixIntPhysics;
using FixMath;
using UnityEngine;

public class MonsterLogic : LogicActor {
    #region 属性字段

    public int MonsterID { get; private set; }

    #endregion

    #region public

    public MonsterLogic(int monsterID,
        RenderObject renderObject,
        FixIntBoxCollider fixIntBoxCollider,
        FixIntVector3 logicPos
    ) {
        MonsterID = monsterID;
        RenderObject = renderObject;
        FixIntBoxCollider = fixIntBoxCollider;
        LogicPos = logicPos;
        ObjectType = LogicObjectType.Monster;
    }


    public override void OnCreate() {
        base.OnCreate();
        InitAttribute();
    }

    public override void OnHit(GameObject goEffect, int survialTimeMS, LogicObject sourceObj, FixInt logicAxisX) {
        base.OnHit(goEffect, survialTimeMS, sourceObj, logicAxisX);
        LogicAxis_X = -logicAxisX;
    }

    public override void State_Floating(bool isUpFloating) {
        base.State_Floating(isUpFloating);
        string animClipName = isUpFloating ? AnimaNames.Anim_Float_up : AnimaNames.Anim_Float_down;
        PlayAnim(animClipName);
        ActionState = LogicObjectActionState.Float;
    }

    public override void State_TriggerGrounding() {
        base.State_TriggerGrounding();
        if (ObjectState != LogicObjectState.Death) {
            PlayAnim(AnimaNames.Anim_Getup);
            // 延迟动画的时间之后, 将状态改成Idle
            // TODO 这个固定延迟不是很准确, 应该更加Getup的动画时间决定
            LogicTimerManager.Instance.DelayCallOnce(0.7f, () => {
                PlayAnim(AnimaNames.Anim_Idle);
                ActionState = LogicObjectActionState.Idle;
            });
        }
        else {
            PlayAnim(AnimaNames.Anim_Dead);
            // ActionState = LogicObjectActionState.Idle; // TODO 还没验证
        }
    }

    #endregion

    #region private

    private void InitAttribute() {
        MonsterCfg cfg = ConfigCenter.Instance.GetMonsterCfgById(MonsterID);
        if (cfg == null) {
            Debug.LogError($"配置是空 MonsterID:{MonsterID}");
            return;
        }
        this.hp = cfg.hp;
        this.mp = cfg.mp;
        this.ap = cfg.ap;
        this.ad = cfg.ad;
        this.adDef = cfg.adDef;
        this.apDef = cfg.apDef;
        this.pct = cfg.pct;
        this.mct = cfg.mct;
        this.adPctRate = cfg.adPctRate;
        this.apMctRate = cfg.apMctRate;
        this.str = cfg.str;
        this.sta = cfg.sta;
        this.Int = cfg.Int;
        this.spi = cfg.spi;
        this.agl = cfg.agl;

        this.atkRange = cfg.atkRange;
        this.searchDisRange = cfg.searchDisRange;
    }

    #endregion
}