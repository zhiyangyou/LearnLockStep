using System;
using Sirenix.OdinInspector;

using UnityEngine;

[Serializable]
public class SkillEffectConfig
{
    [AssetList]
    [LabelText("技能特效")]
    [PreviewField(70, ObjectFieldAlignment.Left)]
    public GameObject skillEffect; // 技能特效

    [LabelText("触发帧")]
    public int triggerFrame; // 触发帧

    [LabelText("结束帧")]
    public int endFrame; // 结束帧

    [LabelText("特效偏移位置上")]
    public Vector3 effectOffsetPos; // 特效偏移位置上

    [LabelText("特效位置类型你")]
    public EffectPosType effectPosType; // 特效位置类型你

    [ToggleGroup(nameof(isSetTransParent),"是否设置特效父节点")]
    public bool isSetTransParent = false; // 是否设置特效父节点

    [ToggleGroup(nameof(isSetTransParent),"是否设置特效父节点")]
    [LabelText("父节点配置")]
    public TransParentType TransParentType; // 父节点配置
}

public enum TransParentType
{
    [LabelText("无配置")]
    None,

    [LabelText("左手")]
    LeftHand,

    [LabelText("右手")]
    RightHand,
}

public enum EffectPosType
{
    [LabelText("跟随角色位置和方向")]
    FollowPosDir, // 跟随角色位置和方向

    [LabelText("值跟随角色方向")]
    FollowDir, // 值跟随角色方向

    [LabelText("屏幕中心位置")]
    CenterPos, // 屏幕中心位置

    [LabelText("引导位置")]
    GuidePos, // 引导位置

    [LabelText("跟随特效移动位置")]
    FollowEffectMovePos, // 跟随特效移动位置
}