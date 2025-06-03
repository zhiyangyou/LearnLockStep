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
        switch (skillType) {
            case EBattleOperateSkillType.None:
                break;
            case EBattleOperateSkillType.ClickSkill:
                ReleaseSkill(frameOpData.skillId, null);
                break;
            case EBattleOperateSkillType.GuideSkill:
                break;
            case EBattleOperateSkillType.StockPileTriggerSkill:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }


    private void HanlderNetInput_Move(FrameOperateData frameOpData) {
        LogicFrameEvent_LocalMoveInput(frameOpData.input_dir.ToFixIntVector3());
        ((HeroRender)RenderObject).CurInputDir = frameOpData.input_dir.ToFixIntVector3().ToVector3();
    }
}