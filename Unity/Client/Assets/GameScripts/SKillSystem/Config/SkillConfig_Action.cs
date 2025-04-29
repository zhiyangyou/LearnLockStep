using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum MoveActionType {
    /// <summary>
    /// 往左移动多少距离, 往右移动多少距离
    /// </summary>
    [LabelText("指定目标位置")] TargerPos,

    /// <summary>
    /// 通过摇杆选中位置
    /// </summary>
    [LabelText("引导位置")] GuidePos,

    /// <summary>
    /// 技能特效沿着曲线走, 比如神枪手的手雷
    /// </summary>
    [LabelText("贝塞尔移动")] BezierPos,
}

/// <summary>
/// 行动完成后的操作
/// </summary>
public enum MoveActionFinishOpation {
    None,
    Skill,
    Buff
}

/// <summary>
/// 处理:释放技能的时候,角色会移动的功能
/// </summary>
[Serializable]
public class SkillConfig_Action {
    [LabelText("触发帧")] public int triggerFrame;

    [LabelText("移动方式")] [OnValueChanged(nameof(OnValueChanged_MoveActionType))]
    public MoveActionType moveActionType;

    /// <summary>
    /// BezierPos类型使用的: 比如手雷的最高点?
    /// </summary>
    [LabelText("最高位置点"), ShowIf(nameof(_isShowBezierPos))]
    public Vector3 heightPos;

    /// <summary>
    /// TargetPos类型使用
    /// </summary>
    [LabelText("移动位置")] [ShowIf(nameof(_isShowMovePos))]
    public Vector3 movePos;

    [LabelText("持续时间(毫秒)")] public int durationMS;

    /// <summary>
    /// 移动完成后, 执行的动作,附加 buff,或是释放后续技能
    /// </summary>
    [LabelText("移动操作完成")] [OnValueChanged(nameof(OnValueChanged_MoveActionFinishOpation))]
    public MoveActionFinishOpation moveActionFinishOpation;


    [ShowIf(nameof(_isShowFinishParam))] [LabelText("触发参数")]
    public List<int> actionFinishIDList;


    //是否显示移动位置
    private bool _isShowMovePos;

    //是否显示移动完成参数
    private bool _isShowFinishParam;

    // 是否显示贝塞尔数据
    private bool _isShowBezierPos;

    private void OnValueChanged_MoveActionType(MoveActionType moveActionType) {
        _isShowMovePos =
            moveActionType == MoveActionType.TargerPos
            || moveActionType == MoveActionType.BezierPos;
        _isShowBezierPos = moveActionType == MoveActionType.BezierPos;
    }

    private void OnValueChanged_MoveActionFinishOpation(MoveActionFinishOpation finishOpation) {
        _isShowFinishParam = finishOpation != MoveActionFinishOpation.None;
    }
}