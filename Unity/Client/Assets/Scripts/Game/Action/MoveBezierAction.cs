using System;
using FixMath;
using UnityEngine;

public class MoveBezierAction : ActionBehaviour {
    #region 属性字段

    private LogicObject _actionObj = null;
    private FixIntVector3 _startPos;
    private FixIntVector3 _highestPoint;
    private FixIntVector3 _endPos;
    private FixInt _durationMS;

    /// <summary>
    /// 当前累计已经运行的时间
    /// </summary>
    private FixInt _accRuntimeMS;

    private FixInt _curTimeProgress;

    #endregion

    #region public

    /// <summary>
    /// 
    /// </summary>
    /// <param name="actionObj"></param>
    /// <param name="startPos"></param>
    /// <param name="highestPoint">最高点(贝塞尔曲线中间控制点)</param>
    /// <param name="endPos"></param>
    /// <param name="durationMS"></param>
    /// <param name="onMoveFinish"></param>
    /// <param name="onMoveUpdate"></param>
    /// <param name="???"></param>
    public MoveBezierAction(
        LogicObject actionObj,
        FixIntVector3 startPos,
        FixIntVector3 highestPoint,
        FixIntVector3 endPos,
        FixInt durationMS,
        Action onMoveFinish,
        Action onMoveUpdate
    ) {
        _actionObj = actionObj;
        _startPos = startPos;
        _highestPoint = highestPoint;
        _endPos = endPos;
        _durationMS = durationMS;
        if (_durationMS == FixInt.Zero) {
            Debug.LogError("MoveBezierAction 持续时间不允许配置0 , 强制改成0.1");
            _durationMS = 0.1;
        }
        _onMoveFinish = onMoveFinish;
        _onMoveUpdate = onMoveUpdate;
        _accRuntimeMS = 0;
        _curTimeProgress = 0;
    }

    public override void OnLogicFrameUpdate() {
        _accRuntimeMS += LogicFrameConfig.LogicFrameIntervalMS;
        _curTimeProgress = _accRuntimeMS / _durationMS; // TODO 不理解

        if (_curTimeProgress >= 1) {
            _curTimeProgress = 1;
            actionFinish = true;
            OnActionFinish();
            return;
        }
        _onMoveUpdate?.Invoke();
        _actionObj.LogicPos = BezierUtils.BezierCurve(_startPos, _highestPoint, _endPos, _curTimeProgress);
    }

    public override void OnActionFinish() {
        if (actionFinish) {
            _onMoveFinish?.Invoke();
        }
    }

    #endregion

    #region private

    private MoveBezierAction() { }

    #endregion
}