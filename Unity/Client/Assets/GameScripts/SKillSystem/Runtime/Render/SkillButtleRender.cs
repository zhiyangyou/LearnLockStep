public class SkillButtleRender : RenderObject {
    #region 属性字段

    private SkillConfig_Bullet _bulletConfig;
    
    #endregion

    #region public

    public void SetRenderData(LogicObject logicObj, SkillConfig_Bullet bulletConfig) {
        
        SetLogicObject(logicObj);
        _bulletConfig = bulletConfig;
    }

    #endregion

    #region private

    #endregion
}