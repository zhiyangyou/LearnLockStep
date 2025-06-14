﻿using System;
using FixMath;
using ServerShareToClient;
using UnityEngine;


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

    private LogicObject _actionObj;
    private FixIntVector3 _startPos;
    private FixInt _durationMS;
    private MoveType _moveType;

    /// <summary>
    /// 移动的向量
    /// </summary>
    private FixIntVector3 _moveDistance;

    /// <summary>
    /// 当前累计已经运行的时间
    /// </summary>
    private FixInt _accRuntimeMS;

    private FixInt _curTimeProgress;

    #endregion


    #region public

    private MoveToAction() { }

    public MoveToAction(
        LogicObject actionObj,
        FixIntVector3 startPos,
        FixIntVector3 targetPos,
        FixInt durationMS,
        Action onMoveFinish,
        Action onMoveUpdate,
        MoveType moveType
    ) {
        _actionObj = actionObj;
        _startPos = startPos;
        _durationMS = durationMS;
        if (_durationMS == FixInt.Zero) {
            Debug.LogError("持续时间不允许配置0 , 强制改成0.1");
            _durationMS = 0.1;
        }
        _onMoveFinish = onMoveFinish;
        _onMoveUpdate = onMoveUpdate;
        _moveType = moveType;

        _moveDistance = targetPos - startPos;
        _accRuntimeMS = 0;
        _curTimeProgress = 0;
    }

    /// <summary>
    /// 逻辑帧更新
    /// </summary>
    public override void OnLogicFrameUpdate() {
        _accRuntimeMS += GameConstConfig.LogicFrameIntervalMS;
        _curTimeProgress = _accRuntimeMS / _durationMS; // TODO 不理解

        if (_curTimeProgress >= 1) {
            _curTimeProgress = 1;
            actionFinish = true;
            OnActionFinish();
            return;
        }
        _onMoveUpdate?.Invoke();
        // 计算角色应该所处的位置

        FixIntVector3 addDistance = FixIntVector3.zero; // 相对于_startPos的偏移值

        // TODO 这里不能使用startPos 会影响其他修改坐标位置的系统的逻辑
        switch (_moveType) {
            case MoveType.Target: {
                addDistance = _moveDistance * _curTimeProgress;
                _actionObj.LogicPos = (_startPos + addDistance);
                // Debug.LogError($"{Time.frameCount} move 赋值XYZ轴 位置数值");
            }
                break;
            case MoveType.X: {
                addDistance.x = (_moveDistance * _curTimeProgress).x;
                _actionObj.LogicPos = new FixIntVector3(
                    _actionObj.LogicPos.x + addDistance.x,
                    _actionObj.LogicPos.y,
                    _actionObj.LogicPos.z
                );
                // Debug.LogError($"{Time.frameCount} move 赋值X轴 位置数值");
            }
                break;
            case MoveType.Y: {
                addDistance.y = (_moveDistance * _curTimeProgress).y;
                _actionObj.LogicPos = new FixIntVector3(
                    _actionObj.LogicPos.x,
                    _actionObj.LogicPos.y + addDistance.y,
                    _actionObj.LogicPos.z
                );
                // Debug.LogError($"{Time.frameCount} move 赋值Y轴 位置数值");
            }
                break;
            case MoveType.Z: {
                addDistance.z = (_moveDistance * _curTimeProgress).z;
                _actionObj.LogicPos = new FixIntVector3(
                    _actionObj.LogicPos.x,
                    _actionObj.LogicPos.y,
                    _actionObj.LogicPos.z + addDistance.z
                );
                // Debug.LogError($"{Time.frameCount} move 赋值Z轴 位置数值");
            }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public override void OnActionFinish() {
        if (actionFinish) {
            _onMoveFinish?.Invoke();
        }
    }

    #endregion
}