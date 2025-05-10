using System.Collections;
using System.Collections.Generic;
using FixMath;
using UnityEngine;

public class HeroLogic : LogicActor {
    #region 属性和字段

    public int HeroId { get; private set; }

    #endregion

    #region public

    public HeroLogic(int heroId, RenderObject renderObject) {
        HeroId = heroId;
        RenderObject = renderObject;
        this.ObjectType = LogicObjectType.Hero;
    }

    #endregion

    #region private

    #endregion
}