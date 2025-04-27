using System.Collections.Generic;
using FixIntPhysics;
using FixMath;
using UnityEngine;
using ZMGC.Battle;

public partial class Skill {
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
    public void OnLogicFrameUpdate_Damage() {
        if (_skillData.damageCfgList != null
            && _skillData.damageCfgList.Count > 0) {
            foreach (SkillDamageConfig damageConfig in _skillData.damageCfgList) {
                var configHashCode = damageConfig.GetHashCode();
                // 创建碰撞体
                if (_curLogicFrame == damageConfig.triggerFrame) {
                    DestoryCollider(damageConfig);
                    var collider = CreateCollider(damageConfig);
                    _dicColliders.Add(configHashCode, collider);

                    if (damageConfig.triggerIntervalMs == 0) {
                        TriggerColliderDamage(collider, damageConfig);
                    }
                }

                // 碰撞体的伤害检测

                if (damageConfig.triggerIntervalMs != 0) {
                    _curDamageAccTimeMS += damageConfig.triggerIntervalMs;
                    if (_curDamageAccTimeMS >= damageConfig.triggerIntervalMs) {
                        _curDamageAccTimeMS = 0;
                        if (_dicColliders.TryGetValue(configHashCode, out var collider)) {
                            TriggerColliderDamage(collider, damageConfig);
                        }
                        // 直接触发伤害碰撞检测
                    }
                }

                // 销毁碰撞体
                if (_curLogicFrame == damageConfig.endFrame) {
                    DestoryCollider(damageConfig);
                }
            }
        }
    }

    public ColliderBehaviour CreateCollider(SkillDamageConfig damageConfig) {
        ColliderBehaviour collider = null;

        if (damageConfig.DetectionMode == DamageDetectionMode.Box3D) {
            FixIntVector3 boxSize = new FixIntVector3(damageConfig.boxSize);
            FixIntVector3 offset = new FixIntVector3(damageConfig.boxOffset) * _skillCreater.LogicAxis_X;
            offset.y = FixIntMath.Abs(offset.y); // 限制Y轴偏移
            collider = new FixIntBoxCollider(boxSize, offset);
            collider.SetBoxData(offset, boxSize);
            collider.UpdateColliderInfo(_skillCreater.LogicPos, boxSize);
        }
        else if (damageConfig.DetectionMode == DamageDetectionMode.Sphere3D) {
            FixIntVector3 offset = new FixIntVector3(damageConfig.sphereOffset) * _skillCreater.LogicAxis_X;
            offset.y = FixIntMath.Abs(offset.y);

            collider = new FixIntSphereCollider(damageConfig.radius, offset);
            collider.SetBoxData(offset, offset);
            collider.UpdateColliderInfo(_skillCreater.LogicPos, FixIntVector3.zero, damageConfig.radius);
        }
        else {
            Debug.LogError($"暂时不支持{damageConfig.DetectionMode}类型的碰撞体类型");
        }
        return collider;
    }

    /// <summary>
    /// 触发碰撞伤害检测
    /// </summary>
    public void TriggerColliderDamage(ColliderBehaviour collider, SkillDamageConfig damageConfig) {
        /*
         * 1. 获取目标
         * 2. 碰撞检测逻辑
         * 3. 获取到碰撞的目标之后, 对敌人造成伤害
         */

        // 1. 获取目标
        var enemyList = BattleWorld.GetExitsLogicCtrl<BattleLogicCtrl>().GetEnemyList(_skillCreater.ObjectType);


        // 2.碰撞检测逻辑
        List<LogicActor> damageTargetList = new List<LogicActor>();
        foreach (LogicActor target in enemyList) {
            if (collider.ColliderType == ColliderType.Box) {
                if (PhysicsManager.IsCollision(collider as FixIntBoxCollider, target.FixIntBoxCollider)) {
                    damageTargetList.Add(target);
                }
            }
            else if (collider.ColliderType == ColliderType.Shpere) {
                if (PhysicsManager.IsCollision(collider as FixIntSphereCollider, target.FixIntBoxCollider)) {
                    damageTargetList.Add(target);
                }
            }
            else {
                Debug.LogError($"不支持{collider.ColliderType}类型的碰撞盒子 objectType:{target.ObjectType}");
            }
        }

        // 3. 获取到碰撞的目标之后, 对敌人造成伤害
        enemyList.Clear();

        foreach (LogicActor targetDamage in damageTargetList) {
            // 造成伤害
            targetDamage.SkillDamage(9999, damageConfig);

            // TODO 添加伤害特效

            // TODO 添加伤害buff

            // TODO 播放击中音效
        }
    }

    /// <summary>
    /// 销毁对应配置生成的碰撞体
    /// </summary>
    /// <param name="config"></param>
    public void DestoryCollider(SkillDamageConfig config) {
        var configHashCode = config.GetHashCode();
        if (_dicColliders.TryGetValue(configHashCode, out var collider)) {
            _dicColliders.Remove(configHashCode);
            if (collider != null) {
                collider.OnRelease();
            }
        }
    }

    #endregion
}