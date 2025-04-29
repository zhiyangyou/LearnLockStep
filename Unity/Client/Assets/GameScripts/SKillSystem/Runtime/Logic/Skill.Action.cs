public partial class Skill {
    public void OnLogicFrameUpdate_Action() {
        var actionList = _skillConfig.actionList;
        if (actionList != null && actionList.Count > 0) {
            foreach (SkillConfig_Action actionConfig in actionList) {
                if (actionConfig.triggerFrame == _curLogicFrame) {
                    // TODO 触发行动
                    AddMoveAction(actionConfig, _skillCreater);
                }
            }
        }
    }

    public void AddMoveAction(SkillConfig_Action actionConfig, LogicActor actionActor) { }
}