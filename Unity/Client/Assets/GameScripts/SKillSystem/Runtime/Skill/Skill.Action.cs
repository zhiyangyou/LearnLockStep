using System;
using FixMath;
using UnityEngine;

public partial class Skill {
    public void OnLogicFrameUpdate_Action() {
        var actionList = _skillConfig.actionList;
        if (actionList != null && actionList.Count > 0) {
            foreach (SkillConfig_Action actionConfig in actionList) {
                if (actionConfig.triggerFrame == _curLogicFrame) {
                    AddMoveAction(actionConfig, _skillCreater, default, null, null);
                }
            }
        }
    }

    public void AddMoveAction(SkillConfig_Action configAction, LogicObject actionActor, Vector3 effectOffset, Action onMoveFinish, Action onMoveUpdate) {
        FixIntVector3 movePos = new FixIntVector3(configAction.movePos);
        FixIntVector3 targetPos = actionActor.LogicPos + movePos * actionActor.LogicAxis_X;
        FixIntVector3 startPos = actionActor.LogicPos;
        // Debug.LogError($"add move action pos:{targetPos}");
        MoveType moveType = MoveType.Target;

        if (configAction.moveActionType == MoveActionType.TargerPos) {
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
        }
        else if (configAction.moveActionType == MoveActionType.GuidePos) {
            // 目标位置
            targetPos = _skillGuidePos;
            // 起始位置
            startPos = targetPos + _skillCreater.LogicPos + new FixIntVector3(effectOffset);
            moveType = MoveType.Target;
            // 调用MoveToAction
        }
        else {
            Debug.LogError($"暂时不支持其他类型的技能特效引导{configAction.moveActionType}");
        }
        MoveToAction moveToAction = new(
            actionActor,
            startPos,
            targetPos,
            configAction.durationMS,
            () => {
                onMoveFinish?.Invoke();
                switch (configAction.moveActionFinishOpation) {
                    case MoveActionFinishOpation.None:
                        break;
                    case MoveActionFinishOpation.Skill: {
                        foreach (int skillID in configAction.actionFinishIDList) {
                            _skillCreater.ReleaseSkill(skillID, null);
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
            onMoveUpdate,
            moveType
        );

        // 开始行动
        LogicActionController.Instance.RunAction(moveToAction);
    }
}