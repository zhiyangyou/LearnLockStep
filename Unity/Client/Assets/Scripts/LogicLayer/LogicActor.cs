using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LogicActor : LogicObject
{
    #region life-cycle

    public override void OnCreate()
    {
        base.OnCreate();
    }

    public override void OnLogicFrameUpdate()
    {
        base.OnLogicFrameUpdate();
        OnLogicFrameUpdate_Move();
        OnLogicFrameUpdate_Skill();
        OnLogicFrameUpdate_Gravity();
    }

    public override void OnDestory()
    {
        base.OnDestory();
    }

    #endregion

    #region public

    public void SetRenderData(RenderObject renderObject)
    {
        this.RenderObject = renderObject;
    }

    #endregion
}