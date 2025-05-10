using FixIntPhysics;
using FixMath;
using UnityEngine;

public class SkillBulletLogic : LogicObject {
    #region 属性字段

    private Skill _skill;
    private LogicActor _fireLogicActor;
    private SkillConfig_Bullet _bulletConfig;
    private ColliderBehaviour _collider;
    private LogicRandom _logicRandom = new LogicRandom(10);

    private int _curLogicFrame = 0;
    private int _curLogicFrameAccTime = 0;
    private bool _bulletIsHit = false;

    #endregion

    #region public

    public SkillBulletLogic(Skill skill,
        LogicActor fireLogicActor,
        RenderObject selfRenderObj,
        SkillConfig_Bullet bulletConfig) {
        // 字段初始化
        _skill = skill;
        _fireLogicActor = fireLogicActor;
        _bulletConfig = bulletConfig;
        RenderObject = selfRenderObj;


        // 位置更新

        LogicAxis_X = _fireLogicActor.LogicAxis_X;

        FixIntVector3 rangePos = FixIntVector3.one;
        if (_bulletConfig.isLoopCreate) {
            var minV3 = _bulletConfig.minRandomRangeVec3;
            var maxV3 = _bulletConfig.maxRandomRangeVec3;
            rangePos.x = _logicRandom.Range(minV3.x, maxV3.x);
            rangePos.y = _logicRandom.Range(minV3.y, maxV3.y);
            rangePos.z = _logicRandom.Range(minV3.z, maxV3.z);
        }
        FixIntVector3 genPos = LogicAxis_X * (new FixIntVector3(_bulletConfig.offset) + rangePos);
        genPos.y = FixIntMath.Abs(genPos.y);
        LogicPos = _fireLogicActor.LogicPos + genPos;
        LogicDir = _fireLogicActor.LogicDir;
        LogicAngle = new FixIntVector3(_bulletConfig.angle);

        // 伤害
        if (_bulletConfig.isAttachDamage) {
            var damageConfig = _bulletConfig.damageConfig;
            if (damageConfig != null) {
                if (damageConfig.DetectionMode == DamageDetectionMode.Box3D) {
                    _collider = new FixIntBoxCollider(FixIntVector3.one, FixIntVector3.zero);
                }
                else if (damageConfig.DetectionMode == DamageDetectionMode.Sphere3D) {
                    _collider = new FixIntSphereCollider(damageConfig.radius, FixIntVector3.zero);
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
        // 碰撞体更新

        var damageConfig = _bulletConfig.damageConfig;
        if (_collider != null && damageConfig != null) {
            if (damageConfig.ColliderPosType == ColliderPosType.FollowPos) {
                if (damageConfig.DetectionMode == DamageDetectionMode.Box3D) {
                    FixIntVector3 offset = LogicAxis_X * new FixIntVector3(damageConfig.boxOffset);
                    _collider.SetBoxData(offset, new FixIntVector3(damageConfig.boxSize));
                    _collider.UpdateColliderInfo(LogicPos, new FixIntVector3(damageConfig.boxSize));
                }
                else if (damageConfig.DetectionMode == DamageDetectionMode.Sphere3D) {
                    FixIntVector3 offset = LogicAxis_X * new FixIntVector3(damageConfig.sphereOffset);
                    _collider.SetBoxData(damageConfig.radius, offset);
                    _collider.UpdateColliderInfo(LogicPos, FixIntVector3.zero, damageConfig.radius);
                }
                else {
                    Debug.LogError($"不支持的伤害检测方式: {damageConfig.DetectionMode}");
                }
            }
        }


        // 击中逻辑

        if (_curLogicFrameAccTime >= _bulletConfig.survialTimeMsg) {
            Release();
        }
    }

    #endregion

    public void Release() { }

    #region private

    #endregion
}