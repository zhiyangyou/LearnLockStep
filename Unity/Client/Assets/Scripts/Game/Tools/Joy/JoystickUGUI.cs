﻿using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class JoystickUGUI : MonoBehaviour {
#if UNITY_EDITOR
    /// <summary>
    /// 在scene视图绘制线 方便查看可点击区域
    /// </summary>
    void OnDrawGizmos() {
        //Debug.Log("OnDrawGizmos start...");
        if (Application.isPlaying == false) {
            return;
        }
        // Gizmos.color = Color.red;

        //本地坐标
        // Vector3 max = new Vector3(transform.localPosition.x + showRangeMax.x + 119, transform.localPosition.y + 119 + showRangeMax.y, 0);
        // Vector3 min = new Vector3(transform.localPosition.x + showRangeMin.x - 119, transform.localPosition.y - 119 + showRangeMin.y, 0);

        //世界坐标
        // max = transform.parent.TransformPoint(max);
        // min = transform.parent.TransformPoint(min);


        // Gizmos.DrawLine(new Vector3(min.x, min.y, 0f), new Vector3(max.x, min.y, 0f));
        // Gizmos.DrawLine(new Vector3(max.x, min.y, 0f), new Vector3(max.x, max.y, 0f));
        // Gizmos.DrawLine(new Vector3(max.x, max.y, 0f), new Vector3(min.x, max.y, 0f));
        // Gizmos.DrawLine(new Vector3(min.x, max.y, 0f), new Vector3(min.x, min.y, 0f));
    }
#endif


    public enum JoystickState {
        /// <summary>
        /// 闲置
        /// </summary>
        ldle,

        /// <summary>
        /// 抬起
        /// </summary>
        TouchUp,

        /// <summary>
        /// 按下
        /// </summary>
        TouchDown,

        /// <summary>
        /// 准备
        /// </summary>
        Ready,

        /// <summary>
        /// 拖动
        /// </summary>
        Drag,
    }

    public Canvas canvas;
    [Header("摇杆触发点击区域")] public Transform trTriggerArea;
    [Header("摇杆总节点")] public Transform trRoot;
    [Header("摇杆背景")] public Transform trBg;
    [Header("中心")] public Transform trCenter;
    [Header("摇杆方向箭头")] public Transform trDirArrow;


    Vector3 joystickDirection;
    public Vector3 JoystickDirection => joystickDirection;

    /// <summary>
    /// 摇杆抬起位置
    /// </summary>
    // [Header("摇杆抬起位置(初始化位置)")]
    // public Vector3 joystickInitPosition = new Vector3(165f, 165f, 0f);
    /// <summary>
    /// 点击触发范围
    /// </summary>
    private Vector3 DefoutTriggerPos;


    /// <summary>
    /// 手柄状态
    /// </summary>
    public JoystickState joystickState = JoystickState.ldle;

    /// <summary>
    /// 按下状态位置
    /// </summary>
    Vector3 touchPosition;

    /// <summary>
    /// 切换到拖动状态最小距离差
    /// </summary>
    [Header("摇杆球最小拖动距离")] public float switchMoveMin = 20f;

    /// <summary>
    /// 杆拖动最大值
    /// </summary>
    [Header("摇杆球最大拖动距离")] public float stickMoveMax = 73f;

    /// <summary>
    /// 显示指向 鼠标距离中心最小距离
    /// </summary>
    [Header("鼠标距离中心最小距离(显示指向)")] public float showDirection = 20f;


    /// <summary>
    /// 箭头距离中心的初始偏移值
    /// </summary>
    private Vector3 _OffsetPosArrowWithCenter = Vector3.zero;

    public Action<Vector3> OnMoveCallBack = null;

    // Use this for initialization
    void Start() {
        DefoutTriggerPos = trTriggerArea.transform.localPosition;

        UIEventListener uiEventListener = transform.GetComponentInChildren<UIEventListener>();

        // Debug.Log("绑定委托");
        uiEventListener.OnDrag = onDrag;
        uiEventListener.OnPress = onPress;
        uiEventListener.OnUp = OnUP;

        _OffsetPosArrowWithCenter = trDirArrow.position - trCenter.position;
        InitState();
    }

    /// <summary>
    /// 鼠标按下触发
    /// </summary>
    /// <param name="go"></param>
    /// <param name="varPress"></param>
    void onPress(PointerEventData eventData) {
        //if (varPress == true)
        SwitchJoyStickState(JoystickState.TouchDown);
        //else
        //    SwitchJoyStickState(JoystickState.TouchUp);
    }

    void OnUP(PointerEventData eventData) {
        SwitchJoyStickState(JoystickState.TouchUp);
    }

    void onDrag(PointerEventData eventData) {
        Action();
    }

    /// <summary>
    /// 切换摇杆状态
    /// </summary>
    /// <param name="state"></param>
    public void SwitchJoyStickState(JoystickState state) {
        joystickState = state;

        Action();
    }

    void Action() {
        // Debug.Log("Action JoystickState:"+ joystickState);
        if (joystickState == JoystickState.ldle) {
            return;
        }

        switch (joystickState) {
            case JoystickState.TouchUp:
                InitState();
                SwitchJoyStickState(JoystickState.ldle);
                break;
            case JoystickState.TouchDown:
                TouchState();
                SwitchJoyStickState(JoystickState.Drag); // 一按下就进入拖拽模式
                DragState();
                break;
            case JoystickState.Ready:
                ReadyState();
                break;
            case JoystickState.Drag:
                DragState();
                break;
        }
    }

    /// <summary>
    /// 获取鼠标相对于对象的本地坐标
    /// </summary>
    /// <returns></returns>
    Vector2 GetMouseLocalPosition(Transform transform) {
        //获取鼠标屏幕坐标
        Vector2 mousePosition;
        //转换为Canvas针对物体的局部坐标
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.GetComponent<RectTransform>(), Input.mousePosition, canvas.worldCamera, out mousePosition);
        return mousePosition;
    }

    /// <summary>
    /// 抬起动作
    /// </summary>
    void InitState() {
        trCenter.localPosition = Vector3.zero;
        trDirArrow?.gameObject.SetActive(false);
        //设置虚拟摇杆 抬起 触发区域
        trTriggerArea.transform.localPosition = DefoutTriggerPos;
        OnMoveCallBack?.Invoke(Vector3.zero);
    }

    /// <summary>
    /// 按下动作
    /// </summary>
    void TouchState() {
        touchPosition = GetMouseLocalPosition(transform);
    }

    /// <summary>
    /// 准备状态
    /// </summary>
    void ReadyState() {
        Vector3 position = GetMouseLocalPosition(transform);

        float distance = Vector3.Distance(position, touchPosition);

        //点击屏幕拖动大于切换拖动状态最小距离 则切换到拖动状态
        if (distance > switchMoveMin) {
            SwitchJoyStickState(JoystickState.Drag);
        }
    }


    /// <summary>
    /// 拖动状态
    /// </summary>
    void DragState() {
        Vector3 mouseLocalPosition = GetMouseLocalPosition(trRoot);

        //鼠标与摇杆的距离
        float distance = Vector3.Distance(mouseLocalPosition, trBg.localPosition);


        //设置杆的位置
        Vector3 stickLocalPosition = mouseLocalPosition;

        //鼠标位置大于杆拖动的最大值
        if (distance > stickMoveMax) {
            float proportion = stickMoveMax / distance;

            stickLocalPosition = (mouseLocalPosition - trBg.localPosition) * proportion;
        }

        trCenter.localPosition = stickLocalPosition;

        trDirArrow.localPosition = trCenter.localPosition + _OffsetPosArrowWithCenter;
        //设置指向位置

        //摇杆与鼠标的距离 大于 指向显示最小距离  则显示指向 
        if (distance > showDirection) {
            trDirArrow?.gameObject.SetActive(true);

            //获取鼠标位置与摇杆的角度
            Double angle = Math.Atan2((mouseLocalPosition.y - trBg.localPosition.y), (mouseLocalPosition.x - trBg.localPosition.x)) * 180 / Math.PI;
            if (trDirArrow != null)
                trDirArrow.eulerAngles = new Vector3(0, 0, (float)angle);

            //设置摇杆指向
            joystickDirection = mouseLocalPosition - trBg.localPosition;
            joystickDirection.z = 0;
        }
        else {
            trDirArrow?.gameObject.SetActive(true);
        }
        //设置虚拟摇杆 拖动 触发区域
        trTriggerArea.transform.localPosition = DefoutTriggerPos;
        Vector3 dir = joystickDirection.normalized;
        //OnMoveCallBack?.Invoke(new Vector3(dir.x*1000, 0, dir.y * 1000));
        OnMoveCallBack?.Invoke(new Vector3(dir.x, 0, dir.y));
    }
}