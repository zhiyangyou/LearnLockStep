using System;
using FixMath;
using UnityEngine;

public class HeroRender : RenderObject
{
    #region 属性字段

    private HeroLogic _heroLogic;

    private HeroLogic heroLogic
    {
        get
        {
            if (_heroLogic == null)
            {
                _heroLogic = this.LogicObject as HeroLogic;
            }
            return _heroLogic;
        }
    }

    #endregion

    #region life-cycle

    public override void OnCreate()
    {
        base.OnCreate();
        JoystickUGUI.OnMoveCallBack += OnJoyStickMove;
    }


    public override void OnRelease()
    {
        JoystickUGUI.OnMoveCallBack -= OnJoyStickMove;
        base.OnRelease();
    }

    protected override void Update()
    {
        base.Update();
    }

    private void OnDestroy()
    {
    }

    #endregion

    #region private

    private void OnJoyStickMove(Vector3 pos)
    {
        FixIntVector3 logicDir = FixIntVector3.zero;
        if (pos != Vector3.zero)
        {
            logicDir.x = pos.x;
            logicDir.y = pos.y;
            logicDir.z = pos.z;
        }
        if (heroLogic != null)
        {
            heroLogic.LogicFrameEvent_Input(logicDir);
        }
        else
        {
            Debug.LogError("HeroLogic is null");
        }
    }

    #endregion
}