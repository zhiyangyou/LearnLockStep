using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class SkillConfig_Bullet {
    [AssetList, LabelText("特效"), PreviewField(70f, ObjectFieldAlignment.Left)]
    public GameObject goBulletPrefab;

    [LabelText("锁定寻敌(常用于普通子弹, 调整子弹发射角度射向前方敌人)")]
    public bool intelligentAttack;

    [LabelText("触发帧")] public int triggerFrame;

    [LabelText("是否循环创建"), BoxGroup("循环创建参数")]
    public bool isLoopCreate;

    [LabelText("循环间隔,"), ShowIf(nameof(isLoopCreate)), BoxGroup("循环创建参数")]
    public int loopIntervalMs;

    [LabelText("最小随机位置波动范围,"), ShowIf(nameof(isLoopCreate)), BoxGroup("循环创建参数")]
    public Vector3 minRandomRangeVec3;

    [LabelText("最大随机位置波动范围,"), ShowIf(nameof(isLoopCreate)), BoxGroup("循环创建参数")]
    public Vector3 maxRandomRangeVec3;

    [LabelText("移动速度")] public float moveSpeed;
    [LabelText("存活时间")] public int survialTimeMsg;
    [LabelText("重力加速度")] public Vector2 gravitySpeed;
    [LabelText("发射位置偏移")] public Vector3 offset;
    [LabelText("发射角度")] public Vector3 angle;
    [LabelText("击中是否销毁")] public bool isHitDestory = true;

    [LabelText("击中特效"), PreviewField(70, ObjectFieldAlignment.Left)]
    public GameObject hitEffect;

    [LabelText("特效存活时间")]
    public int hitEffectSurvialTimeMS = 3000;
    [LabelText("击中音效")]
    public AudioClip hitAudio;
    [ToggleGroup(nameof(isAttachDamage),"是否附加伤害")]
    public bool isAttachDamage = false;
    [ToggleGroup(nameof(isAttachDamage),"是否附加伤害")]
    public SkillConfig_Damage damageConfig;
}