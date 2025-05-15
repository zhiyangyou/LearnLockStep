using System.Collections.Generic;
using FixIntPhysics;
using FixMath;
using UnityEngine;
using ZMGC.Battle;

public class SkillBulletLogic : LogicObject {
    #region 属性字段

    private Skill _skill;
    private LogicActor _fireLogicActor;
    private SkillConfig_Bullet _bulletConfig;
    private ColliderBehaviour _bulletCollider;


    private int _curLogicFrame = 0;
    private int _curLogicFrameAccTime = 0;
    private bool _bulletIsHit = false;
    private List<LogicActor> _hitTargetList = new();

    public bool BulletIsUnValid { get; private set; } = false; // 子弹是否失效了

    #endregion

    #region public

    public SkillBulletLogic(Skill skill,
        LogicActor fireLogicActor,
        RenderObject selfRenderObj,
        SkillConfig_Bullet bulletConfig,
        FixIntVector3 rangePos
    ) {
        // 字段初始化
        _skill = skill;
        _fireLogicActor = fireLogicActor;
        _bulletConfig = bulletConfig;
        RenderObject = selfRenderObj;


        // 位置更新
        LogicAxis_X = _fireLogicActor.LogicAxis_X;
        LogicDir = new FixIntVector3(LogicAxis_X, 0, 0) + new FixIntVector3(bulletConfig.dirOffset);

        FixIntVector3 genPos = LogicAxis_X * (new FixIntVector3(_bulletConfig.offset) + rangePos);
        genPos.y = FixIntMath.Abs(genPos.y);
        LogicPos = _fireLogicActor.LogicPos + genPos;

        LogicAngle = new FixIntVector3(_bulletConfig.angle) * LogicAxis_X;

        // 伤害
        if (_bulletConfig.isAttachDamage) {
            var damageConfig = _bulletConfig.damageConfig;
            if (damageConfig != null) {
                if (damageConfig.DetectionMode == DamageDetectionMode.Box3D) {
                    _bulletCollider = new FixIntBoxCollider(FixIntVector3.one, FixIntVector3.zero);
                }
                else if (damageConfig.DetectionMode == DamageDetectionMode.Sphere3D) {
                    _bulletCollider = new FixIntSphereCollider(damageConfig.radius, FixIntVector3.zero);
                }
                else {
                    Debug.LogError("不支持的碰撞检测类型");
                }
            }
        }
    }

    public override void OnLogicFrameUpdate() {
        base.OnLogicFrameUpdate();
        _curLogicFrameAccTime = _curLogicFrame * LogicFrameConfig.LogicFrameIntervalMS;
        _curLogicFrame++;

        // 位置更新
        LogicPos += LogicDir * new FixInt(_bulletConfig.moveSpeed) * new FixInt(LogicFrameConfig.LogicFrameInterval);

        var damageConfig = _bulletConfig.damageConfig;

        // 触发伤害
        foreach (LogicActor hitTarget in _hitTargetList) {
            hitTarget.BulletDamage(DamageCalcuCenter.CalculateDamage(_bulletConfig.damageConfig, _fireLogicActor, hitTarget), damageConfig);
            hitTarget.OnHit(_bulletConfig.hitEffect, _bulletConfig.hitEffectSurvialTimeMS, this, LogicAxis_X);
            if (_bulletConfig.hitAudio != null) {
                AudioController.GetInstance().PlaySoundByAudioClip(_bulletConfig.hitAudio, false, AudioPriorityConfig.Bullet_AudioClip);
            }

            // 子弹附加buff
            AttachBuff(hitTarget);

            if (_bulletConfig.isHitDestory) {
                Release();
                break;
            }
        }
        _hitTargetList.Clear();

        // 碰撞体更新
        if (_bulletCollider != null && damageConfig != null) {
            if (damageConfig.ColliderPosType == ColliderPosType.FollowPos) {
                if (damageConfig.DetectionMode == DamageDetectionMode.Box3D) {
                    FixIntVector3 offset = LogicAxis_X * new FixIntVector3(damageConfig.boxOffset);
                    _bulletCollider.SetBoxData(offset, new FixIntVector3(damageConfig.boxSize));
                    _bulletCollider.UpdateColliderInfo(LogicPos, new FixIntVector3(damageConfig.boxSize));
                }
                else if (damageConfig.DetectionMode == DamageDetectionMode.Sphere3D) {
                    FixIntVector3 offset = LogicAxis_X * new FixIntVector3(damageConfig.sphereOffset);
                    _bulletCollider.SetBoxData(damageConfig.radius, offset);
                    _bulletCollider.UpdateColliderInfo(LogicPos, FixIntVector3.zero, damageConfig.radius);
                }
                else {
                    Debug.LogError($"不支持的伤害检测方式: {damageConfig.DetectionMode}");
                }
            }
        }
        // 击中检测
        if (_bulletCollider != null && damageConfig != null) {
            var enemyList = BattleWorld.GetExitsLogicCtrl<BattleLogicCtrl>().GetEnemyList(_fireLogicActor.ObjectType);

            foreach (var enemyActor in enemyList) {
                if (damageConfig.DetectionMode == DamageDetectionMode.Box3D) {
                    _bulletIsHit = PhysicsManager.IsCollision(_bulletCollider as FixIntBoxCollider, enemyActor.FixIntBoxCollider);
                }
                else if (damageConfig.DetectionMode == DamageDetectionMode.Sphere3D) {
                    _bulletIsHit = PhysicsManager.IsCollision(_bulletCollider as FixIntSphereCollider, enemyActor.FixIntBoxCollider);
                }
                if (_bulletIsHit) {
                    _hitTargetList.Add(enemyActor);
                }
            }
        }

        if (_curLogicFrameAccTime >= _bulletConfig.survialTimeMsg) {
            Release();
        }
    }

    public void Release() {
        RenderObject.OnRelease();
        _bulletCollider?.OnRelease();
        _bulletCollider = null;
        BulletIsUnValid = true;
    }

    #endregion


    #region private

    private void AttachBuff(LogicActor buffTarget) {
        var damageConfig = _bulletConfig.damageConfig;
        if (damageConfig is { HasAddBuffs: true }) {
            foreach (var buffID in damageConfig.addBuffs) {
                BuffSystem.Instance.AttachBuff(buffID, _fireLogicActor, buffTarget, _skill, null);
            }
        }
    }

    #endregion
}