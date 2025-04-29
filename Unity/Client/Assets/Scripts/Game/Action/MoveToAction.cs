using System;
using FixMath;


public enum MoveType {
    /// <summary>
    /// 指定目标
    /// </summary>
    Target,

    /// <summary>
    /// 只移动X轴
    /// </summary>
    X,

    /// <summary>
    /// 只移动Y轴
    /// </summary>
    Y,

    /// <summary>
    /// 只移动Z轴
    /// </summary>
    Z
}

/// <summary>
/// 控制角色移动
/// </summary>
public class MoveToAction : ActionBehaviour {
    #region 属性字段

    private LogicActor _actionObj;
    private FixIntVector3 _startPos;
    private FixInt _durationMS;
    private MoveType _moveType;

    /// <summary>
    /// 移动的向量
    /// </summary>
    private FixIntVector3 _moveDistance;

    #endregion


    #region public

    private MoveToAction() { }

    public MoveToAction(
        LogicActor actionObj,
        FixIntVector3 startPos,
        FixIntVector3 targetPos,
        FixInt durationMS,
        Action onMoveFinish,
        Action onUpdate,
        MoveType moveType
    ) {
        _actionObj = actionObj;
        _startPos = startPos;
        _durationMS = durationMS;
        _callBackMoveFinish = onMoveFinish;
        _callBackUpdateAction = onUpdate;
        _moveType = moveType;

        _moveDistance = targetPos - startPos;
    }

    /// <summary>
    /// 逻辑帧更新
    /// </summary>
    public override void OnLogicFrameUpdate() { }

    #endregion
}