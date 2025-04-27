using System.Collections.Generic;
using FixIntPhysics;
using FixMath;
using UnityEngine;

public partial class Skill
{
    #region 属性和字段

    /// <summary>
    /// 特效物体字段,k是SkillEffectConfig的hash值,value是SkillEffectLogic
    /// </summary>
    private Dictionary<int, ColliderBehaviour> _dicColliders = new();

    /// <summary>
    /// 当前伤害累加时间, LogicTime, 单位是毫米阿尼
    /// </summary>
    private int _curDamageAccTimeMS = 0;

    #endregion


    #region public

    /// <summary>
    ///  
    /// </summary>
    public void OnLogicFrameUpdate_Damage()
    {
        if (_skillData.damageCfgList != null
            && _skillData.damageCfgList.Count > 0)
        {
            foreach (SkillDamageConfig damageConfig in _skillData.damageCfgList)
            {
                // 创建碰撞体
                if (_curLogicFrame == damageConfig.triggerFrame)
                {
                    DestoryCollider(damageConfig);
                    var collider = CreateCollider(damageConfig);
                    _dicColliders.Add(damageConfig.GetHashCode(), collider);
                }

                // 碰撞体的伤害检测
                if (damageConfig.triggerIntervalMs == 0)
                {
                    // 直接触发伤害
                }
                else
                {
                    _curDamageAccTimeMS += damageConfig.triggerIntervalMs;
                    if (_curDamageAccTimeMS >= damageConfig.triggerIntervalMs)
                    {
                        _curDamageAccTimeMS = 0;
                        // 触发伤害
                    }
                }

                // 销毁碰撞体
                if (_curLogicFrame == damageConfig.endFrame)
                {
                    DestoryCollider(damageConfig);
                }
            }
        }
    }

    public ColliderBehaviour CreateCollider(SkillDamageConfig damageConfig)
    {
        ColliderBehaviour collider = null;

        if (damageConfig.DetectionMode == DamageDetectionMode.Box3D)
        {
            FixIntVector3 boxSize = new FixIntVector3(damageConfig.boxSize);
            FixIntVector3 offset = new FixIntVector3(damageConfig.boxOffset) * _skillCreater.LogicAxis_X;
            offset.y = FixIntMath.Abs(offset.y); // 限制Y轴偏移
            collider = new FixIntBoxCollider(boxSize, offset);
            collider.SetBoxData(offset, boxSize);
            collider.UpdateColliderInfo(_skillCreater.LogicPos, boxSize);
        }
        else if (damageConfig.DetectionMode == DamageDetectionMode.Sphere3D)
        {
            FixIntVector3 offset = new FixIntVector3(damageConfig.sphereOffset) * _skillCreater.LogicAxis_X;
            offset.y = FixIntMath.Abs(offset.y);

            collider = new FixIntSphereCollider(damageConfig.radius, offset);
            collider.SetBoxData(offset, offset);
            collider.UpdateColliderInfo(_skillCreater.LogicPos, FixIntVector3.zero, damageConfig.radius);
        }
        else
        {
            Debug.LogError($"暂时不支持{damageConfig.DetectionMode}类型的碰撞体类型");
        }
        return collider;
    }

    /// <summary>
    /// 销毁对应配置生成的碰撞体
    /// </summary>
    /// <param name="config"></param>
    public void DestoryCollider(SkillDamageConfig config)
    {
        var configHashCode = config.GetHashCode();
        if (_dicColliders.TryGetValue(configHashCode, out var collider))
        {
            _dicColliders.Remove(configHashCode);
            if (collider != null)
            {
                collider.OnRelease();
            }
        }
    }

    #endregion
}