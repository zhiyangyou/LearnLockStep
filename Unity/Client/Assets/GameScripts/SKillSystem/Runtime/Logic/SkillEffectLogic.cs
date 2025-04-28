using FixMath;
using UnityEngine;

public class SkillEffectLogic : LogicObject {
    #region 属性和字段

    private SkillEffectConfig _skillEffectConfig = null;
    private LogicActor _skillCreater = null;

    #endregion


    #region public

    public SkillEffectLogic(LogicObjectType logicObjectType, SkillEffectConfig skillEffectConfig, RenderObject renderObject, LogicActor skillCreater) {
        this.ObjectType = logicObjectType;
        this.RenderObject = renderObject;
        this._skillEffectConfig = skillEffectConfig;
        this._skillCreater = skillCreater;
        this.LogicAxis_X = skillCreater.LogicAxis_X;
        if (skillEffectConfig.effectPosType is EffectPosType.FollowDir or EffectPosType.FollowPosDir) {
            var offsetPos = (new FixIntVector3(skillEffectConfig.effectOffsetPos)) * LogicAxis_X;
            offsetPos.y = FixIntMath.Abs(offsetPos.y); // 轴向不能影响Y
            LogicPos = skillCreater.LogicPos + offsetPos;
        }
        else if (skillEffectConfig.effectPosType == EffectPosType.Zero) {
            LogicPos = FixIntVector3.zero;
        }
    }

    public override void OnDestory() {
        base.OnDestory();
        RenderObject.OnRelease();
    }

    #endregion

    #region private

    private SkillEffectLogic() { }

    #endregion
}