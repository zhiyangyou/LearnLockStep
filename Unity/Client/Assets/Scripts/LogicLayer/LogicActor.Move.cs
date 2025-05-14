using System.Collections;
using System.Collections.Generic;
using FixMath;
using UnityEngine;

public partial class LogicActor {
    #region 属性字段

    private FixIntVector3 _inputMoveDir = FixIntVector3.zero;

    #endregion

    /// <summary>
    /// 逻辑帧: 位置
    /// </summary>
    public void OnLogicFrameUpdate_Move() {
        this.FixIntBoxCollider?.UpdateColliderInfo(LogicPos, FixIntBoxCollider.Size);
        if (ActionState is not (LogicObjectActionState.Idle or LogicObjectActionState.Move)
            && IsForceAllowMove == false) {
            return;
        }
        // 位置
        LogicPos += _inputMoveDir * LogicMoveSpeed * ((FixInt)LogicFrameConfig.LogicFrameInterval);

        // 朝向
        if (LogicDir != _inputMoveDir) {
            LogicDir = _inputMoveDir;
        }

        // 轴向
        if (LogicDir.x != FixInt.Zero
            && IsForceNotAllowModifyDir == false
           ) {
            LogicAxis_X = LogicDir.x > (FixInt)0
                ? FixInt.One
                : (-1);
        }
    }

    public void LogicFrameEvent_Input(FixIntVector3 inputDir) {
        _inputMoveDir = inputDir;
    }
}