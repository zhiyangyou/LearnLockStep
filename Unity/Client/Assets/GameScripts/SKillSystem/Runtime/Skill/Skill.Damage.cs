﻿using System.Collections.Generic;
using FixIntPhysics;
using FixMath;
using ServerShareToClient;
using UnityEngine;
using ZMGC.Battle;

/// <summary>
/// 伤害来源
/// </summary>
public enum DamageSource {
    None,
    Skill, // 技能伤害
    Buff, // buff伤害
    Bullet, // 子弹伤害
}

public partial class Skill {
    #region 属性和字段

    /// <summary>
    /// 特效物体字段,k是SkillEffectConfig的hash值,value是SkillEffectLogic
    /// </summary>
    private Dictionary<int, ColliderBehaviour> _dicColliders = new();

    /// <summary>
    /// 当前伤害累加时间, LogicTime, 单位是毫米阿尼
    /// </summary>
    // private int _curDamageAccTimeMS = 0;
    private List<int> _listDamageAccTimeMS = new();

    #endregion


    #region public

    public ColliderBehaviour CreateOrUpdateCollider(SkillConfig_Damage configDamage, ColliderBehaviour colliderBehaviour, LogicObject followObj) {
        ColliderBehaviour collider = colliderBehaviour;

        LogicObject followTarget = followObj == null ? _skillCreater : followObj;
        if (configDamage.DetectionMode == DamageDetectionMode.Box3D) {
            FixIntVector3 boxSize = new FixIntVector3(configDamage.boxSize);
            FixIntVector3 offset = new FixIntVector3(configDamage.boxOffset) * followTarget.LogicAxis_X;
            offset.y = FixIntMath.Abs(offset.y); // 限制Y轴偏移
            if (collider == null) {
                collider = new FixIntBoxCollider(boxSize, offset);
                // Debug.LogError($"new collider {Time.frameCount} :{boxSize}");
            }
            collider.SetBoxData(offset, boxSize);
            collider.UpdateColliderInfo(followTarget.LogicPos, boxSize);
        }
        else if (configDamage.DetectionMode == DamageDetectionMode.Sphere3D) {
            FixIntVector3 offset = new FixIntVector3(configDamage.sphereOffset) * followTarget.LogicAxis_X;
            offset.y = FixIntMath.Abs(offset.y);

            if (collider == null) {
                collider = new FixIntSphereCollider(configDamage.radius, offset);
            }
            collider.SetBoxData(configDamage.radius, offset);
            collider.UpdateColliderInfo(followTarget.LogicPos, FixIntVector3.zero, configDamage.radius);
        }
        else {
            Debug.LogError($"暂时不支持{configDamage.DetectionMode}类型的碰撞体类型");
        }
        return collider;
    }


    /// <summary>
    /// 触发碰撞伤害检测
    /// </summary>
    public void TriggerColliderDamage(ColliderBehaviour collider, SkillConfig_Damage configDamage) {
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
        foreach (LogicActor damageTargetActor in damageTargetList) {
            // 造成伤害
            damageTargetActor.SkillDamage(DamageCalcuCenter.CalculateDamage(configDamage, _skillCreater, damageTargetActor), configDamage);

            // 添加伤害特效
            AddHitEffect(damageTargetActor, configDamage.TargetType == TargetType.Self ? _skillCreater : damageTargetActor);

            // 添加伤害buff
            if (configDamage.HasAddBuffs) {
                foreach (var buffID in configDamage.addBuffs) {
                    BuffSystem.Instance.AttachBuff(buffID, _skillCreater, damageTargetActor, this, null);
                }
            }

            // 触发后续技能
            if (configDamage.triggerSkillId != 0) {
                // 预先释放
                _combinationSkillID = configDamage.triggerSkillId;
            }

            // 播放击中音效
            PlayHitAudio();
        }
    }

    #endregion

    #region private

    private void ResetDamageAccTime() {
        _listDamageAccTimeMS.Clear();
    }

    private void InitDamageAccTime() {
        var damageConfigCount = _skillConfigSo.damageCfgList.Count;
        for (int i = 0; i < damageConfigCount; i++) {
            _listDamageAccTimeMS.Add(0);
        }
    }

    /// <summary>
    ///  
    /// </summary>
    private void OnLogicFrameUpdate_Damage() {
        if (_skillConfigSo.damageCfgList != null
            && _skillConfigSo.damageCfgList.Count > 0) {
            // foreach (SkillConfig_Damage damageConfig in _skillConfigSo.damageCfgList)
            for (int i = 0; i < _skillConfigSo.damageCfgList.Count; i++) {
                SkillConfig_Damage damageConfig = _skillConfigSo.damageCfgList[i];
                var configHashCode = damageConfig.GetHashCode();

                // 如果需要变更位置的碰撞盒类型,那么更新碰撞体位置
                if (damageConfig.ColliderPosType == ColliderPosType.FollowPos
                    && _dicColliders.TryGetValue(configHashCode, out var damageCollider)
                    && damageCollider != null) {
                    CreateOrUpdateCollider(damageConfig, damageCollider, _skillCreater);
                }

                // 创建碰撞体
                if (_curLogicFrame == damageConfig.triggerFrame) {
                    DestoryCollider(damageConfig);
                    var collider = CreateOrUpdateCollider(damageConfig, null, _skillCreater);
                    _dicColliders.Add(configHashCode, collider);

                    // 如果没有配置,每帧都触发
                    if (damageConfig.triggerIntervalMs == 0) {
                        TriggerColliderDamage(collider, damageConfig);
                    }
                }

                // 碰撞体的伤害检测
                if (damageConfig.triggerIntervalMs != 0) {
                    _listDamageAccTimeMS[i] += GameConstConfig.LogicFrameIntervalMS;
                    if (_listDamageAccTimeMS[i] >= damageConfig.triggerIntervalMs) {
                        _listDamageAccTimeMS[i] = 0;
                        if (_dicColliders.TryGetValue(configHashCode, out var collider)) {
                            TriggerColliderDamage(collider, damageConfig);
                        }
                    }
                }

                // 销毁碰撞体
                if (_curLogicFrame == damageConfig.endFrame) {
                    DestoryCollider(damageConfig);
                }
            }
        }
    }

    private void AddHitEffect(LogicActor target, LogicActor sourceActor) {
        if (_skillConfigSo.skillCfg.skillHitEffect != null) {
            target.OnHit(_skillConfigSo.skillCfg.skillHitEffect, _skillConfigSo.skillCfg.hitEffectSurvialTimeMs, sourceActor, _skillCreater.LogicAxis_X);
        }
    }

    /// <summary>
    /// 销毁对应配置生成的碰撞体
    /// </summary>
    /// <param name="config"></param>
    private void DestoryCollider(SkillConfig_Damage config) {
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