using System.Collections;
using System.Collections.Generic;
using FixIntPhysics;
using FixMath;
using UnityEngine;

public class MonsterLogic : LogicActor {
    public int MonsterID { get; private set; }

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

    public override void OnHit(GameObject goEffect, int survialTimeMS, LogicActor sourceActor) {
        base.OnHit(goEffect, survialTimeMS, sourceActor);
        LogicAxis_X = -sourceActor.LogicAxis_X;
    }

    public override void State_Floating(bool isUpFloating) {
        base.State_Floating(isUpFloating);
        string animClipName = isUpFloating ? "Anim_Float_up" : "Anim_Float_down";
        PlayAnim(animClipName);
        ActionState = LogicObjectActionState.Float;
    }

    public override void State_TriggerGrounding() {
        base.State_TriggerGrounding();
        if (ObjectState != LogicObjectState.Death) {
            PlayAnim("Anim_Getup");
        }
        else {
            PlayAnim("Anim_Dead");
            // ActionState = LogicObjectActionState.Idle; // TODO 还没验证
        }
    }
}