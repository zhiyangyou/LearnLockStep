using System;
using FixMath;
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

    private FixInt _curTimeScale;

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
        _onMoveFinish = onMoveFinish;
        _onMoveUpdate = onMoveUpdate;
        _moveType = moveType;

        _moveDistance = targetPos - startPos;
        _accRuntimeMS = 0;
        _curTimeScale = 0;
    }

    /// <summary>
    /// 逻辑帧更新
    /// </summary>
    public override void OnLogicFrameUpdate() {
        _accRuntimeMS += LogicFrameConfig.LogicFrameIntervalMS;
        _curTimeScale = _accRuntimeMS / _durationMS; // TODO 不理解

        if (_curTimeScale >= 1) {
            _curTimeScale = 1;
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
                addDistance = _moveDistance * _curTimeScale;
                _actionObj.LogicPos = (_startPos + addDistance);
                Debug.LogError($"{Time.frameCount} move 赋值XYZ轴 位置数值");
            }
                break;
            case MoveType.X: {
                addDistance.x = (_moveDistance * _curTimeScale).x;
                _actionObj.LogicPos = new FixIntVector3(
                    _actionObj.LogicPos.x + addDistance.x,
                    _actionObj.LogicPos.y,
                    _actionObj.LogicPos.z
                );
                // Debug.LogError($"{Time.frameCount} move 赋值X轴 位置数值");
            }
                break;
            case MoveType.Y: {
                addDistance.y = (_moveDistance * _curTimeScale).y;
                _actionObj.LogicPos = new FixIntVector3(
                    _actionObj.LogicPos.x,
                    _actionObj.LogicPos.y + addDistance.y,
                    _actionObj.LogicPos.z
                );
                // Debug.LogError($"{Time.frameCount} move 赋值Y轴 位置数值");
            }
                break;
            case MoveType.Z: {
                addDistance.z = (_moveDistance * _curTimeScale).z;
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

        // 移动时的操作的逻辑
        //  这种算法不会考量技能播放时进行移动
        // _actionObj.LogicPos += addDistance; // 这种算法会考量在技能释放过程中,存在移动的可能性
    }

    public override void OnActionFinish() {
        if (actionFinish) {
            _onMoveFinish?.Invoke();
        }
    }

    #endregion
}