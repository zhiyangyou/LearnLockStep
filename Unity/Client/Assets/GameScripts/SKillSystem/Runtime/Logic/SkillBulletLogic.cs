public class SkillBulletLogic : LogicObject {
    #region 属性字段

    private Skill _skill;
    private LogicActor _fireLogicActor;
    private SkillConfig_Bullet _bulletConfig;

    #endregion

    #region public

    public SkillBulletLogic(Skill skill,
        LogicActor fireLogicActor,
        RenderObject selfRenderObj,
        SkillConfig_Bullet bulletConfig) {
        _skill = skill;
        _fireLogicActor = fireLogicActor;
        _bulletConfig = bulletConfig;
        RenderObject = selfRenderObj;
    }

    #endregion

    #region private

    #endregion
}