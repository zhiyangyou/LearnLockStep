using System;
using FixIntPhysics;
using FixMath;
using Sirenix.OdinInspector;
using UnityEngine;


[Serializable]
public class SkillConfig_Damage
{
    [LabelText("触发帧")] public int triggerFrame;

    [LabelText("结束帧")] public int endFrame;

    [LabelText("触发间隔")] public int triggerIntervalMs;

    [LabelText("碰撞体跟随特效移动")] public int isFollowEffect;

    [LabelText("伤害类型")] public DamageType damageType;

    [LabelText("伤害倍率,百分比")] public int damageRate;

    [LabelText("碰撞检测规则")] [OnValueChanged(nameof(OnDetectionModeChanged))]
    public DamageDetectionMode DetectionMode;

    [LabelText("Box大小"), ShowIf(nameof(_showBox3D)), OnValueChanged(nameof(OnValueChanged_BoxSize))]
    public Vector3 boxSize = Vector3.one;

    [LabelText("Box偏移"), ShowIf(nameof(_showBox3D)), OnValueChanged(nameof(OnValueChanged_ColliderOffset))]
    public Vector3 boxOffset = Vector3.zero;

    [LabelText("圆球偏移"), ShowIf(nameof(_showSphere3D)), OnValueChanged(nameof(OnValueChanged_ColliderOffset))]
    public Vector3 sphereOffset = new Vector3(0, 0.9f, 0f);

    [LabelText("圆球检测半径"), ShowIf(nameof(_showSphere3D)), OnValueChanged(nameof(OnValueChanged_Radius))]
    public float radius = 1f;

    [LabelText("圆球检测半径高度"), ShowIf(nameof(_showSphere3D))]
    public float radiusHeight = 0f;

    [LabelText("碰撞检测规则")] public ColliderPosType ColliderPosType = ColliderPosType.FollowDir;

    [LabelText("伤害触发目标")] public TargetType TargetType;

    [TitleGroup("附加buff", "伤害生效的一瞬间附加的多个伤害buff")]
    public int[] addBuffs;

    [TitleGroup("触发后续技能", "造成伤害后且技能释放完毕后触发的技能")]
    public int triggerSkillId;
#if UNITY_EDITOR


    private bool _showBox3D;
    private bool _showSphere3D;

    private int _curLogicFrame = 0; // 当前逻辑帧    
    private FixIntBoxCollider _fixIntBoxCollider;
    private FixIntSphereCollider _fixIntSphereCollider;

    private void OnDetectionModeChanged(DamageDetectionMode mode)
    {
        _showBox3D = mode == DamageDetectionMode.Box3D;
        _showSphere3D = mode == DamageDetectionMode.Sphere3D;
        CreateCollider();
    }

    private void OnValueChanged_Radius(float newRadius)
    {
        if (_fixIntSphereCollider != null)
            _fixIntSphereCollider.SetBoxData(newRadius, GetCollierOffsetPos(), ColliderPosType == ColliderPosType.FollowPos);
        else
            Debug.LogError($"{nameof(_fixIntSphereCollider)} is null");
    }


    private void OnValueChanged_ColliderOffset(Vector3 newOffset)
    {
        if (DetectionMode == DamageDetectionMode.Box3D)
        {
            if (_fixIntBoxCollider != null)
                _fixIntBoxCollider.SetBoxData(GetCollierOffsetPos(), boxSize, ColliderPosType == ColliderPosType.FollowPos);
            else
            {
                // Debug.LogError($"{nameof(_fixIntBoxCollider)} is null");
            }
        }
        else if (DetectionMode == DamageDetectionMode.Sphere3D)
        {
            if (_fixIntSphereCollider != null)
                _fixIntSphereCollider.SetBoxData(radius, GetCollierOffsetPos(), ColliderPosType == ColliderPosType.FollowPos);
            else
            {
                // Debug.LogError($"{nameof(_fixIntSphereCollider)} is null");
            }
        }
    }

    private void OnValueChanged_BoxSize(Vector3 newSize)
    {
        if (_fixIntBoxCollider != null)
            _fixIntBoxCollider.SetBoxData(GetCollierOffsetPos(), newSize, ColliderPosType == ColliderPosType.FollowPos);
        else
        {
            // Debug.LogError($"{nameof(_fixIntBoxCollider)} is null");
        }
    }

    /// <summary>
    /// 获取碰撞体偏移值
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCollierOffsetPos()
    {
        var characterPos = SkillCompilerWindow.GetCharacterPos();
        if (DetectionMode == DamageDetectionMode.Box3D)
        {
            return characterPos + boxOffset;
        }
        else if (DetectionMode == DamageDetectionMode.Sphere3D)
        {
            return characterPos + sphereOffset;
        }
        return Vector3.zero;
    }

    public void CreateCollider()
    {
        DestoryCollider();
        if (DetectionMode == DamageDetectionMode.Box3D)
        {
            var offsetPos = GetCollierOffsetPos();
            _fixIntBoxCollider = new FixIntBoxCollider(boxSize, offsetPos);
            _fixIntBoxCollider.SetBoxData(offsetPos, boxSize, ColliderPosType == ColliderPosType.FollowPos);
        }
        else if (DetectionMode == DamageDetectionMode.Sphere3D)
        {
            var offsetPos = GetCollierOffsetPos();
            _fixIntSphereCollider = new FixIntSphereCollider(new FixInt(this.radius), offsetPos);
            _fixIntSphereCollider.SetBoxData(radius, offsetPos, ColliderPosType == ColliderPosType.FollowPos);
        }
    }

    public void DestoryCollider()
    {
        if (_fixIntBoxCollider != null)
        {
            _fixIntBoxCollider.OnRelease();
            _fixIntBoxCollider = null;
        }
        if (_fixIntSphereCollider != null)
        {
            _fixIntSphereCollider.OnRelease();
            _fixIntSphereCollider = null;
        }
    }

    public void OnInit()
    {
        CreateCollider();
    }

    public void OnRelease()
    {
        DestoryCollider();
    }

    public void PlaySkillStart()
    {
        _curLogicFrame = 0;
        DestoryCollider();
    }

    public void PlaySkillEnd()
    {
        DestoryCollider();
    }

    public void OnLogicFrameUpdate()
    {
        if (_curLogicFrame == triggerFrame)
        {
            CreateCollider();
        }
        else if (_curLogicFrame == endFrame)
        {
            DestoryCollider();
        }
        _curLogicFrame++;
    }
#endif
}


/// <summary>
/// 对象目标
/// </summary>
public enum TargetType
{
    [LabelText("无配置")] None,

    [LabelText("队伍")] Teamate,

    [LabelText("敌人")] Enemy,

    [LabelText("只身")] Self,

    [LabelText("所有对象")] AllObject
}

public enum ColliderPosType
{
    [LabelText("跟随角色朝向")] FollowDir,

    [LabelText("跟随角色位置")] FollowPos,

    [LabelText("中心坐标")] CenterPos,

    [LabelText("目标位置")] TargetPos,
}

/// <summary>
/// 伤害类型
/// </summary>
public enum DamageType
{
    [LabelText("无伤害")] None,

    [LabelText("物理伤害")] AD,

    [LabelText("魔法伤害")] AP
}

/// <summary>
/// 伤害碰撞检测类型
/// </summary>
public enum DamageDetectionMode
{
    [LabelText("无配置")] None,

    [LabelText("3D盒子碰撞检测")] Box3D,

    [LabelText("3D圆球碰撞检测")] Sphere3D,

    [LabelText("3D圆柱体碰撞检测")] Cylinder3D,

    [LabelText("半径范围内(代码搜索)")] RadiusDistance,

    [LabelText("所有目标")] AllTarget
}