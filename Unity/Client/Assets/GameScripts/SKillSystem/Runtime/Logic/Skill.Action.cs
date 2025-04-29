using System;
using FixMath;
using UnityEngine;

public partial class Skill {
    public void OnLogicFrameUpdate_Action() {
        var actionList = _skillConfig.actionList;
        // if (SkillID == 1003) {
            // Debug.LogError($"skillID:{this.SkillID} actionList:{actionList?.Count}  logicFrame : {_curLogicFrame} ");
        // }
        if (actionList != null && actionList.Count > 0) {
            foreach (SkillConfig_Action actionConfig in actionList) {
                if (actionConfig.triggerFrame == _curLogicFrame) {
                    AddMoveAction(actionConfig, _skillCreater);
                }
            }
        }
    }

    public void AddMoveAction(SkillConfig_Action configAction, LogicActor actionActor) {
        FixIntVector3 movePos = new FixIntVector3(configAction.movePos);
        FixIntVector3 targetPos = actionActor.LogicPos + movePos * actionActor.LogicAxis_X;
        MoveType moveType = MoveType.Target;
        if (movePos.IsOnlyAxis_X()) {
            moveType = MoveType.X;
        }
        else if (movePos.IsOnlyAxis_Y()) {
            moveType = MoveType.Y;
        }
        else if (movePos.IsOnlyAxis_Z()) {
            moveType = MoveType.Z;
        }
        else {
            moveType = MoveType.Target;
        }
        MoveToAction moveToAction = new(
            actionActor,
            actionActor.LogicPos,
            targetPos,
            configAction.durationMS,
            () => {
                switch (configAction.moveActionFinishOpation) {
                    case MoveActionFinishOpation.None:
                        break;
                    case MoveActionFinishOpation.Skill: {
                        foreach (int skillID in configAction.actionFinishIDList) {
                            _skillCreater.ReleaseSkill(skillID);
                        }
                    }
                        break;
                    case MoveActionFinishOpation.Buff: {
                        // buff系统
                        Debug.LogError("TODO buff 系统的释放");
                    }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            },
            null,
            moveType
        );

        // 开始行动
        LogicActionController.Instance.RunAction(moveToAction);
    }
}