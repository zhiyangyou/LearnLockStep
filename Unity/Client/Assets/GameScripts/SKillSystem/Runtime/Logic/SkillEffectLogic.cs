using FixMath;
using UnityEngine;

public class SkillEffectLogic : LogicObject {
    #region 属性和字段

    private SkillConfig_Effect _skillConfigEffect = null;
    private LogicActor _skillCreater = null;

    #endregion


    #region public

    public SkillEffectLogic(LogicObjectType logicObjectType, SkillConfig_Effect skillConfigEffect, RenderObject renderObject, LogicActor skillCreater) {
        this.ObjectType = logicObjectType;
        this.RenderObject = renderObject;
        this._skillConfigEffect = skillConfigEffect;
        this._skillCreater = skillCreater;
        this.LogicAxis_X = skillCreater.LogicAxis_X;
        if (skillConfigEffect.effectPosType is EffectPosType.FollowDir or EffectPosType.FollowPosDir) {
            var offsetPos = (new FixIntVector3(skillConfigEffect.effectOffsetPos)) * LogicAxis_X;
            offsetPos.y = FixIntMath.Abs(offsetPos.y); // 轴向不能影响Y
            LogicPos = skillCreater.LogicPos + offsetPos;
        }
        else if (skillConfigEffect.effectPosType == EffectPosType.Zero) {
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