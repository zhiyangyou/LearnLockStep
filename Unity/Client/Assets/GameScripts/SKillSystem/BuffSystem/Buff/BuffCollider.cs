using System;
using System.Collections.Generic;
using FixIntPhysics;
using FixMath;
using UnityEngine;
using ZMGC.Battle;

public class BuffCollider {
    // 初始化数据
    // 生成碰撞体
    // 更新碰撞体, 碰撞检测
    // 释放


    #region 属性字段

    private ColliderBehaviour _buffCollider = null;


    private Buff _buff = null;
    private BuffConfigSO _buffConfig = null;
    private SkillConfig_Damage _configDamage = null;
    private LogicActor _buffReleaser = null;
    private LogicActor _buffAttachTarget = null;

    private Skill _skill;

    #endregion


    #region public

    public BuffCollider(Buff buff) {
        _buff = buff;
        _skill = buff.skill;
        _buffConfig = buff.BuffConfigSo;
        _configDamage = buff.BuffConfigSo.targetConfig.damageConfig;
        _buffReleaser = buff.releaser;
        _buffAttachTarget = buff.attachTarget;
    }


    public ColliderBehaviour CreateOrUpdateCollider(LogicObject followObj) {
        if (_configDamage.DetectionMode == DamageDetectionMode.Box3D) {
            FixIntVector3 boxSize = new FixIntVector3(_configDamage.boxSize);
            FixIntVector3 offset = new FixIntVector3(_configDamage.boxOffset);
            offset.y = FixIntMath.Abs(offset.y); // 限制Y轴偏移
            if (_buffCollider == null) {
                _buffCollider = new FixIntBoxCollider(boxSize, offset);
                // Debug.LogError($"new collider {Time.frameCount} :{boxSize}");
            }
            _buffCollider.SetBoxData(offset, boxSize);
            _buffCollider.UpdateColliderInfo(GetBuffPos(), boxSize);
        }
        else if (_configDamage.DetectionMode == DamageDetectionMode.Sphere3D) {
            FixIntVector3 offset = new FixIntVector3(_configDamage.sphereOffset);
            offset.y = FixIntMath.Abs(offset.y);

            if (_buffCollider == null) {
                _buffCollider = new FixIntSphereCollider(_configDamage.radius, offset);
            }
            _buffCollider.SetBoxData(_configDamage.radius, offset);
            _buffCollider.UpdateColliderInfo(GetBuffPos(), FixIntVector3.zero, _configDamage.radius);
        }
        else {
            Debug.LogError($"暂时不支持{_configDamage.DetectionMode}类型的碰撞体类型");
        }
        return _buffCollider;
    }


    /// <summary>
    /// 触发碰撞伤害检测
    /// </summary>
    public List<LogicActor> CaculateColliderTargetObjs(ColliderBehaviour collider) {
        // 1. 获取目标
        var enemyList = BattleWorld.GetExitsLogicCtrl<BattleLogicCtrl>().GetEnemyList(_buffReleaser.ObjectType);

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

        return damageTargetList;
    }

    public void OnRelease() {
        _buffCollider?.OnRelease();
        _buffCollider = null;
    }

    #endregion

    #region private

    public FixIntVector3 GetBuffPos() {
        var attachType = _buffConfig.attachType;
        switch (attachType) {
            case BuffAttachType.Creator:
            case BuffAttachType.Creator_Pos:
                return _buffReleaser.LogicPos;
                break;
            case BuffAttachType.Target:
            case BuffAttachType.Target_Pos:
                return _buffAttachTarget.LogicPos;
                break;
            case BuffAttachType.GuidePos:
                return _skill.skillGuidePos;
                break;
            case BuffAttachType.None:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #endregion
}