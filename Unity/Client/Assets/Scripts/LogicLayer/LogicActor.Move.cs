using System.Collections;
using System.Collections.Generic;
using Fantasy;
using FixMath;
using ServerShareToClient;
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
        LogicPos += _inputMoveDir * LogicMoveSpeed * ((FixInt)GameConstConfig.LogicFrameInterval);

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


    /// <summary>
    /// 本地驱动的移动
    /// </summary>
    /// <param name="inputDir"></param>
    public void LogicFrameEvent_LocalMoveInput(FixIntVector3 inputDir) {
        _inputMoveDir = inputDir;
    }

    /// <summary>
    /// 帧同步网络驱动的移动
    /// </summary>
    /// <param name="frameOpData"></param>
    public void LogicFrameEvent_NetInput(FrameOperateData frameOpData) {
        var opType = (EBattlePlayerOpType)frameOpData.operate_type;
        if (opType == (EBattlePlayerOpType.InputMove)) {
            LogicFrameEvent_LocalMoveInput(frameOpData.input_dir.ToFixIntVector3());
            ((HeroRender)RenderObject).CurInputDir = frameOpData.input_dir.ToFixIntVector3().ToVector3();
            // Debug.LogError($"LogicFrameEvent_LocalMoveInput:{frameOpData.input_dir.ToFixIntVector3()}");
        }
        else if (opType == EBattlePlayerOpType.ReleaseSkill) {
            Debug.LogError("TODO");
        }
    }
}