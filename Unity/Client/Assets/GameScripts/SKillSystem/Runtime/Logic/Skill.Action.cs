using System;
using FixMath;
using UnityEngine;

public partial class Skill {
    public void OnLogicFrameUpdate_Action() {
        var actionList = _skillConfig.actionList;
        if (actionList != null && actionList.Count > 0) {
            foreach (SkillConfig_Action actionConfig in actionList) {
                if (actionConfig.triggerFrame == _curLogicFrame) {
                    AddMoveAction(actionConfig, _skillCreater, null);
                }
            }
        }
    }

    public void AddMoveAction(SkillConfig_Action configAction, LogicObject actionActor, Action onMoveFinish) {
        FixIntVector3 movePos = new FixIntVector3(configAction.movePos);
        FixIntVector3 targetPos = actionActor.LogicPos + movePos * actionActor.LogicAxis_X;
        // Debug.LogError($"add move action pos:{targetPos}");
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
                onMoveFinish?.Invoke();
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