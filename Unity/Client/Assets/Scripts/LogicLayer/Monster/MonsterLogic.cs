using System.Collections;
using System.Collections.Generic;
using FixIntPhysics;
using UnityEngine;

public class MonsterLogic : LogicActor
{
    public int MonsterID { get; private set; }

    public MonsterLogic(int monsterID, RenderObject renderObject, FixIntBoxCollider fixIntBoxCollider)
    {
        MonsterID = monsterID;
        RenderObject = renderObject;
        FixIntBoxCollider = fixIntBoxCollider;
        ObjectType = LogicObjectType.Monster;
    }
}