using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff配置", menuName = "Buff配置", order = 0)]
[Serializable]
public class BuffConfigSO : ScriptableObject {
    public Sprite Icon;
    public int ID;
    public string Name;
    public int Delay;
    public int IntervalMS;
    public int DurationMS;
    public BuffType BuffType;
    public BuffAttachType AttachType;
    public BuffPosType PosType;
    public BuffDamageType DamageType;
    public int DamageRate;
    public List<BuffParam> ParamsList;
    public TargetGrabData TargetGrabData;
    public AudioClip AudioClip;
    public BuffEffectConfig EffectConfig;
    public GameObject goBuffHitEffect;
    public string ReplaceAnim;
    public string Desc;
}

public class BuffEffectConfig {
    public GameObject GoEffect;

    public EffectAttachType AttachType;
    public BuffEffectPosType BuffEffectPosType;
}

[Serializable]
public class BuffParam {
    public float Value;
    public string Desc;
}

[Serializable]
public class TargetGrabData {
    public Vector3 GrapMovePOs;
}

public enum BuffEffectPosType {
    None,
    Center,
    Hand,
}

public enum EffectAttachType {
    None,
    CreatorFollow,
    CreatorPos,
    TargetFollow,
    TargetPos,
}

public enum BuffDamageType { }

public enum BuffPosType {
    None,
    FollowTarget,
    HitTargetPos,
    UIInputPos,
}

public enum BuffAttachType {
    None,
    Creator,
    Target,
    Creator_Pos,
    Target_Pos,
    GuidePos,
}

public enum BuffType {
    None = 0,
}