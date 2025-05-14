// 处理立绘相关的逻辑

public partial class Skill {
    #region 属性字段

    #endregion

    #region private

    private void OnLogicFrameUpdate_Portrait() {
        if (_skillConfigSo.skillCfg.showSkillPortrait
            && _curLogicFrame == 0
            && _skillConfigSo.skillCfg.skillPortraitObj != null
           ) {
            _skillCreater.RenderObject.ShowSkillPortrait(_skillConfigSo.skillCfg.skillPortraitObj);
        }
    }

    #endregion
}