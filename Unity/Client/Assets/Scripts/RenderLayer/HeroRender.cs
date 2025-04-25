using System;
using FixMath;
using UnityEngine;

public class HeroRender : RenderObject
{
    #region 属性字段

    private const string kStrAniName_Idle2 = "Anim_Idle02";
    private const string kStrAniName_Run = "Anim_Run";

    private HeroLogic _heroLogic;

    private Vector3 _curInputDir = Vector3.zero;

    // 角色动画
    private Animation _ani;

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
        _ani = GetComponent<Animation>();
        if (_ani == null) Debug.LogError("Hero Render 没有Animation组件");
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

        // 判断摇杆是否有输入值, 如果没有,播放待机动画, 如果有播放跑步动画
        PlayAni(_curInputDir is { x: 0f, z: 0f } ? kStrAniName_Idle2 : kStrAniName_Run);
    }

    private void OnDestroy()
    {
    }

    #endregion

    #region private

    private void PlayAni(string aniName)
    {
        if (_ani == null) return;
        if (string.IsNullOrEmpty(aniName)) return;
        _ani.CrossFade(aniName, 0.2f);
    }

    private void OnJoyStickMove(Vector3 pos)
    {
        FixIntVector3 logicDir = FixIntVector3.zero;
        if (pos != Vector3.zero)
        {
            logicDir.x = pos.x;
            logicDir.y = pos.y;
            logicDir.z = pos.z;
        }
        _curInputDir = pos;
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