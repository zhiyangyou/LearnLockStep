using System;
using Fantasy;
using ServerShareToClient;
using UnityEngine;

public partial class LogicActor {
    /// <summary>
    /// 帧同步网络驱动的移动
    /// </summary>
    /// <param name="frameOpData"></param>
    public void LogicFrameEvent_NetInput(FrameOperateData frameOpData) {
        var opType = (EBattlePlayerOpType)frameOpData.operate_type;
        if (opType == (EBattlePlayerOpType.InputMove)) {
            HanlderNetInput_Move(frameOpData);
        }
        else if (opType == EBattlePlayerOpType.ReleaseSkill) {
            HanlderNetInput_Skill(frameOpData);
        }
    }

    private void HanlderNetInput_Skill(FrameOperateData frameOpData) {
        // 处理多种类型技能的释放
        var skillType = (EBattleOperateSkillType)frameOpData.skillType;
        var releaseSkillContextID = frameOpData.frame_op_context_object_id;
        var contextObj = _battleLogicCtrl.LoadContextObj(releaseSkillContextID);
        OnReleaseSkillResult onReleaseSkillResult = contextObj as OnReleaseSkillResult;
        switch (skillType) {
            case EBattleOperateSkillType.ClickSkill:
                ReleaseSkill(frameOpData.skillId, onReleaseSkillResult);
                break;
            case EBattleOperateSkillType.GuideSkill:
                ReleaseSkill(frameOpData.skillId, onReleaseSkillResult, frameOpData.skillPos.ToFixIntVector3());
                break;
            case EBattleOperateSkillType.StockPileTriggerSkill_Begin:
                TriggerStockPileSkill(frameOpData.skillId);
                break;
            case EBattleOperateSkillType.StockPileTriggerSkill_End:
                ReleaseSkill(frameOpData.skillId, onReleaseSkillResult);
                break;
            default:
                Debug.LogError($"尚未被实现的技能类型:{skillType}");
                break;
        }
    }


    private void HanlderNetInput_Move(FrameOperateData frameOpData) {
        LogicFrameEvent_LocalMoveInput(frameOpData.input_dir.ToFixIntVector3());
        ((HeroRender)RenderObject).CurInputDir = frameOpData.input_dir.ToFixIntVector3().ToVector3();
    }
}