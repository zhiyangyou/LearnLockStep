using System;
using FixMath;
using UnityEngine;

public partial class Skill {
    public void OnLogicFrameUpdate_Action() {
        var actionList = _skillConfigSo.actionList;
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
        +
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
            targetPos = skillGuidePos;
            // 起始位置
            startPos = targetPos + _skillCreater.LogicAxis_X * new FixIntVector3(effectOffset);
            startPos.y = FixIntMath.Abs(startPos.y);
            // startPos = targetPos + new FixIntVector3(effectOffset);
            // moveType = MoveType.Target;
            // 调用MoveToAction
        }
        else if (configAction.moveActionType == MoveActionType.BezierPos) {
            //
            startPos = _skillCreater.LogicPos + _skillCreater.LogicAxis_X * new FixIntVector3(effectOffset);
            startPos.y = FixIntMath.Abs(startPos.y);

            //
            FixIntVector3 highestPos = new FixIntVector3(configAction.heightPos) * _skillCreater.LogicAxis_X;
            highestPos.y = FixIntMath.Abs(highestPos.y);
            highestPos += _skillCreater.LogicPos;

            // 
            FixIntVector3 endPos = new FixIntVector3(configAction.movePos) * _skillCreater.LogicAxis_X;
            endPos.y = FixIntMath.Abs(endPos.y);
            targetPos += _skillCreater.LogicPos;
            
            MoveBezierAction bezierAction = new MoveBezierAction(,) {
                
            }
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
                        foreach (var buffID in configAction.actionFinishIDList) {
                            BuffSystem.Instance.AttachBuff(buffID, _skillCreater, _skillCreater, this, null);
                        }
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