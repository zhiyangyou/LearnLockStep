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
        void OnActionFinish() {
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
                    skillGuidePos = actionActor.LogicPos; 
                    foreach (var buffID in configAction.actionFinishIDList) {
                        BuffSystem.Instance.AttachBuff(buffID, _skillCreater, _skillCreater, this, null);
                    }
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void AddLineAction() {
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
                targetPos = skillGuidePos;
                startPos = targetPos + _skillCreater.LogicAxis_X * new FixIntVector3(effectOffset);
                startPos.y = FixIntMath.Abs(startPos.y);
            }
            else {
                Debug.LogError($"暂时不支持其他类型的技能特效引导{configAction.moveActionType}");
            }
            LogicActionController.Instance.RunAction(new MoveToAction(
                actionActor,
                startPos,
                targetPos,
                configAction.durationMS,
                OnActionFinish,
                onMoveUpdate,
                moveType
            ));
        }

        void AddBezierAction() {
            if (configAction.moveActionType == MoveActionType.BezierPos) {
                var startPos = _skillCreater.LogicPos + _skillCreater.LogicAxis_X * new FixIntVector3(effectOffset);
                startPos.y = FixIntMath.Abs(startPos.y);

                var highestPos = new FixIntVector3(configAction.heightPos) * _skillCreater.LogicAxis_X;
                highestPos.y = FixIntMath.Abs(highestPos.y);
                highestPos += _skillCreater.LogicPos;

                var endPos = new FixIntVector3(configAction.movePos) * _skillCreater.LogicAxis_X;
                endPos.y = FixIntMath.Abs(endPos.y);
                endPos += _skillCreater.LogicPos;
                MoveBezierAction bezierAction = new MoveBezierAction(actionActor, startPos, highestPos, endPos, configAction.durationMS, OnActionFinish, onMoveUpdate);
                LogicActionController.Instance.RunAction(bezierAction);
            }
            else {
                Debug.LogError($"AddBezierAction 暂时不支持其他类型的技能特效引导{configAction.moveActionType}");
            }
        }

        switch (configAction.moveActionType) {
            case MoveActionType.TargerPos:
            case MoveActionType.GuidePos:
                AddLineAction();
                break;
            case MoveActionType.BezierPos:
                AddBezierAction();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}