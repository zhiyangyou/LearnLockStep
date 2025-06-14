﻿using FixIntPhysics;
using FixMath;
using ServerShareToClient;
using UnityEngine;

public class SkillEffectLogic : LogicObject {
    #region 属性和字段

    private SkillConfig_Effect _skillConfigEffect = null;
    private LogicActor _skillCreater = null;
    private ColliderBehaviour _collider = null;
    private int _accRuntime = 0;

    #endregion


    #region public

    public SkillEffectLogic(
        LogicObjectType logicObjectType,
        SkillConfig_Effect skillConfigEffect,
        RenderObject renderObject,
        LogicActor skillCreater,
        Skill skill
    ) {
        this.ObjectType = logicObjectType;
        this.RenderObject = renderObject;
        this._skillConfigEffect = skillConfigEffect;
        this._skillCreater = skillCreater;
        this.LogicAxis_X = skillCreater.LogicAxis_X;
        if (skillConfigEffect.effectPosType is EffectPosType.FollowDir or EffectPosType.FollowPosDir) {
            var offsetPos = (new FixIntVector3(skillConfigEffect.effectOffsetPos)) * LogicAxis_X;
            offsetPos.y = FixIntMath.Abs(offsetPos.y); // 轴向不能影响Y
            LogicPos = skillCreater.LogicPos + offsetPos;
        }
        else if (skillConfigEffect.effectPosType == EffectPosType.Zero) {
            LogicPos = FixIntVector3.zero;
        }
        else if (skillConfigEffect.effectPosType == EffectPosType.GuidePos) {
            var initPos = skill.skillGuidePos + skillCreater.LogicAxis_X * new FixIntVector3(skillConfigEffect.effectOffsetPos);
            initPos.y = FixIntMath.Abs(initPos.y);
            // Debug.LogError($"initPos:{initPos}");
            LogicPos = initPos;
        }
        else {
            Debug.LogError($"不支持的 effectPosType 类型{skillConfigEffect.effectPosType}");
        }
    }

    public override void OnDestory() {
        base.OnDestory();
        RenderObject.OnRelease();
    }

    public void OnLogicFrameUpdate_Effect(Skill skill, int curFrame) {
        if (_skillConfigEffect.effectPosType is EffectPosType.FollowPosDir) {
            var offsetPos = (new FixIntVector3(_skillConfigEffect.effectOffsetPos)) * LogicAxis_X;
            offsetPos.y = FixIntMath.Abs(offsetPos.y); // 轴向不能影响Y
            LogicPos = _skillCreater.LogicPos + offsetPos;
        }

        // 特效行动配置
        if (_skillConfigEffect.IsAttachAction && _skillConfigEffect.SkillConfigAction.triggerFrame == curFrame) {
            skill.AddMoveAction(_skillConfigEffect.SkillConfigAction, this, _skillConfigEffect.effectOffsetPos, () => {
                _collider?.OnRelease();
                skill.DestoryEffect(_skillConfigEffect);
                _collider = null;
            }, () => { OnEffectMoveUpdate(skill); });
        }

        if (_skillConfigEffect.IsAttachDamage) {
            var damageConfig = _skillConfigEffect.SkillConfigDamage;
            if (damageConfig.triggerFrame == curFrame) {
                _collider = skill.CreateOrUpdateCollider(damageConfig, null, this);
            }
        }

        // 特效伤害配置
    }

    #endregion

    #region private

    private void OnEffectMoveUpdate(Skill skill) {
        var damageConfig = _skillConfigEffect.SkillConfigDamage;
        // 特效移动逻辑帧回调
        if (damageConfig.isFollowEffect && _collider != null) {
            skill.CreateOrUpdateCollider(damageConfig, _collider, this);
        }

        if (_skillConfigEffect.IsAttachDamage) {
            // 持续伤害
            if (_collider != null && damageConfig.triggerIntervalMs == 0) {
                skill.TriggerColliderDamage(_collider, damageConfig);
            }

            // 间隔伤害
            if (_collider != null && damageConfig.triggerIntervalMs != 0) {
                _accRuntime += GameConstConfig.LogicFrameIntervalMS;
                if (_accRuntime >= damageConfig.triggerIntervalMs) {
                    _accRuntime -= damageConfig.triggerIntervalMs;
                    skill.TriggerColliderDamage(_collider, damageConfig);
                }
            }
        }
    }

    private SkillEffectLogic() { }

    #endregion
}