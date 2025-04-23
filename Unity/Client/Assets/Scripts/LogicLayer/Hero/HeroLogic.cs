using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroLogic : LogicActor
{
    public int HeroId { get; private set; }


    public HeroLogic(int heroId, RenderObject renderObject)
    {
        HeroId = heroId;
        RenderObject = renderObject;
        this.ObjectType = LogicObjectType.Hero;
    }
}