using System;
using UnityEngine;

[RequireComponent(typeof(HeroRender))]
public class HeroStateDebugger : MonoBehaviour {
    private HeroRender _heroRender = null;

    [Header("是否有技能释放中")] public bool HasReleasingSkill = false;
    [Header("Action状态")] public LogicObjectActionState ActionState = LogicObjectActionState.Float;

#if UNITY_EDITOR

    private void Awake() {
        _heroRender = GetComponent<HeroRender>();
    }

    private void Update() {
        HeroLogic heroLogic = _heroRender.LogicObject as HeroLogic;
        if (heroLogic != null) {
            this.HasReleasingSkill = heroLogic.HasReleasingSkill;
            this.ActionState = heroLogic.ActionState;
        }
    }
#endif
}