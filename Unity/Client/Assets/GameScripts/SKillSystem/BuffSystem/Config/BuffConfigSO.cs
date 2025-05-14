using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff配置", menuName = "Buff配置", order = 0)]
[Serializable]
public class BuffConfigSO : ScriptableObject {
    #region 序列化字段

    [LabelText("buff图标"), LabelWidth(0.1f), PreviewField(70, ObjectFieldAlignment.Left), SuffixLabel("Buff图标")]
    public Sprite Icon;

    [LabelText("buffID")] public int id;
    [LabelText("buff名字")] public string name;
    [LabelText("buff延迟触发时间(毫秒)")] public int delayMS;
    [LabelText("buff触发间隔(毫秒)")] public int intervalMS;

    [LabelText("buff持续(毫秒) 0代表1次, -1代表永久 >0代表持续时间")]
    public int DurationMS;

    [LabelText("buff类型")] public BuffType buffType; // buff类型: 晕眩, 沉默 , 击退等...
    [LabelText("buff附加目标类型")] public BuffAttachType attachType;
    [LabelText("buff附加位置")] public BuffPosType posType;
    [LabelText("buff伤害类型")] public BuffDamageType damageType;
    [LabelText("buff伤害倍率")] public int damageRate;
    [LabelText("buff数值配置")] public List<BuffParam> paramsList;

    [LabelText("抓取数据"), PropertyTooltip("把怪物抓取到目标位置")]
    public TargetGrabData targetGrabData;

    [LabelText("触发音效"), TitleGroup("buff技能表现", "所有的表现数据会在buff触发和释放时触发")]
    public AudioClip audioClip;

    [LabelText("触发特效配置"), TitleGroup("buff技能表现", "所有的表现数据会在buff触发和释放时触发")]
    public BuffEffectConfig effectConfig;

    [LabelText("Buff击中特效"), TitleGroup("buff技能表现", "所有的表现数据会在buff触发和释放时触发")]
    public GameObject goBuffHitEffect;

    [LabelText("Buff击中动画"), TitleGroup("buff技能表现", "所有的表现数据会在buff触发和释放时触发"), PropertyTooltip("buff触发的角色动画,比如眩晕, 硬直")]
    public ObjectAnimationState buffTriggerAnim = ObjectAnimationState.None;

    [LabelText("伤害目标配置")] public TargetConfig targetConfig;

    [LabelText("Buff描述"), HideLabel, MultiLineProperty(5)]
    public string Desc;

    #endregion

    #region 查询字段

    /// <summary>
    /// 
    /// </summary>
    public bool IsDelayBuff => delayMS > 0;

    /// <summary>
    /// 是不是永久性buff?
    /// </summary>
    public bool IsForeverBuff => DurationMS < 0;

    public bool HasEffect => effectConfig != null && effectConfig.GoEffect != null;

    /// <summary>
    /// 是不是限时buff
    /// </summary>
    public bool IsLimitTimeBuff => DurationMS > 0;

    /// <summary>
    /// 是不是一次性buff
    /// </summary>
    public bool IsOnceBuff => DurationMS == 0;

    #endregion
}

[Serializable]
[LabelText("Buff特效配置")]
public class BuffEffectConfig {
    [LabelText("Buff特效物体")] public GameObject GoEffect;
    [LabelText("Buff特效附加类型")] public BuffEffectPosType posType;
    [LabelText("Buff特效位置类型")] public BuffEffectAttachPosType effectAttachPosType;
}

[Serializable]
public class BuffParam {
    [LabelText("参数"), PropertyTooltip("例如: 击退距离 | 沉默时间 | 眩晕时间 | 伤害数值")]
    public float Value; //

    [LabelText("参数描述")] public string Desc; // 描述
}

[Serializable]
public class TargetGrabData {
    [LabelText("抓取到目标位置")] public Vector3 GrabMovePos;
    [LabelText("抓取到目标位置需要的移动时间")] public int GrabMoveTimeMS;
}

[Serializable, TabGroup("目标配置")]
public class TargetConfig {
    [LabelText("是否启用")] public bool isOpen = false; // 是否启用

    [LabelText("作用目标类型"), ShowIf(nameof(isOpen))]
    public TargetType targetType; // 作用目标

    [LabelText("伤害配置"), ShowIf(nameof(isOpen))]
    public SkillConfig_Damage damageConfig; // 伤害配置
}

/// <summary>
/// 表示: 当前buff触发时,需要播放的动画
/// </summary>
[LabelText("当前buff触发时,需要播放的动画")]
public enum ObjectAnimationState {
    [LabelText("无配置")] None,
    [LabelText("受击")] BeHit,
    [LabelText("硬直")] Stiff,
}

[LabelText("Buff特效位置类型")]
public enum BuffEffectAttachPosType {
    [LabelText("无配置")] None,
    [LabelText("中心")] Center,
    [LabelText("手的位置")] Hand,
    [LabelText("脚的位置")] Foot,
}

[LabelText("Buff特效附加类型")]
public enum BuffEffectPosType {
    [LabelText("无配置")] None,
    [LabelText("跟随目标")] TargetFollow,
    [LabelText("目标位置")] TargetPos,
}

[LabelText("buff伤害类型")]
public enum BuffDamageType {
    [LabelText("无配置")] None,
    [LabelText("物理伤害")] AD,
    [LabelText("魔法伤害")] AP,
}

[LabelText("buff位置类型")]
public enum BuffPosType {
    [LabelText("无配置")] None,
    [LabelText("跟随目标")] FollowTarget,
    [LabelText("摇杆输入位置")] UIInputPos,
    [LabelText("施法者位置")] ReleaserPos,
}

[LabelText("Buff附加类型")]
public enum BuffAttachType {
    [LabelText("无配置")] None,
    [LabelText("施法者")] Creator, // 附加到施法者身上
    [LabelText("施法目标")] Target, // 附加到目标上, 中毒
    [LabelText("施法者位置")] Creator_Pos, // 施法者所处位置, 岩浆地形
    [LabelText("目标位置")] Target_Pos, // 目标所处位置, 岩浆地形
    [LabelText("引导目标位置")] GuidePos, // 摇杆选中的位置, 岩浆地形
}


[LabelText("Buff类型")]
public enum BuffType {
    [LabelText("无配置")] None = 0,
    [LabelText("击退")] Repel, // 击退
    [LabelText("浮空")] Floating, // 浮空
    [LabelText("硬直")] Stiff, // 硬直
    [LabelText("群体血量修改")] HP_Modify, // 群体血量修改 
    [LabelText("抓取")] Grab, // 群体血量修改 
    [LabelText("无视重力")] IgnoreGravity, // 群体血量修改 
    [LabelText("单体移动速度修改")] MoveSpeed_Modify_Single, // 单体移动速度修改 
    [LabelText("允许移动")] AllowMove, // 单体移动速度修改 
    [LabelText("不允许修改朝向")] NotAllowModifyDir, // 不允许修改朝向 
}