using System.Collections.Generic;
using UnityEngine;

public partial class Skill {
    #region 属性和字段

    /// <summary>
    /// 特效物体字段,k是SkillEffectConfig的hash值,value是SkillEffectLogic
    /// </summary>
    private Dictionary<int, SkillEffectLogic> _dicEffectLogics = new();

    #endregion

    #region public

    public void OnLogicFrameUpdate_Effect() {
        var effectList = _skillConfigSo.effectCfgList;
        if (effectList != null && effectList.Count > 0) {
            foreach (SkillConfig_Effect item in effectList) {
                var effectConfig = item;
                if (effectConfig == null) {
                    var id = _skillConfigSo?.skillCfg.skillID;
                    Debug.LogError($"skillEffectConfig is null : skillID {id.ToString()}");
                    continue;
                }
                var effectConfigHashCode = effectConfig.GetHashCode();
                if (effectConfig.skillEffect != null && _curLogicFrame == effectConfig.triggerFrame) {
                    DestoryEffect(effectConfig); // 避免重复释放技能导致特效对象重复出现

                    Transform trParent = null;
                    if (effectConfig.isSetTransParent) {
                        trParent = _skillCreater.RenderObject.GetTransParent(effectConfig.TransParentType);
                    }

                    // 创建技能特效渲染层
                    var goEffect = GameObject.Instantiate(effectConfig.skillEffect, trParent);
                    var effectRender = goEffect.GetComponent<SkillEffectRender>();
                    if (effectRender == null) effectRender = goEffect.AddComponent<SkillEffectRender>();
                    
                     
                    // 技能特效逻辑层
                    SkillEffectLogic skillEffectLogic = new SkillEffectLogic(
                        LogicObjectType.Effect, effectConfig, effectRender, _skillCreater, this);
                    effectRender.SetLogicObject(skillEffectLogic, effectConfig.effectPosType != EffectPosType.Zero);

                    // 生命周期维护
                    _dicEffectLogics.Add(effectConfigHashCode, skillEffectLogic);
                }

                // 结束之后,自动销毁
                if (_curLogicFrame == effectConfig.endFrame && !effectConfig.IsAttachAction) {
                    DestoryEffect(effectConfig);
                    continue;
                }

                // 更新特效逻辑帧
                SkillEffectLogic effectLogic = null;
                if (_dicEffectLogics.TryGetValue(effectConfigHashCode, out effectLogic) && effectLogic != null) {
                    effectLogic.OnLogicFrameUpdate_Effect(this, _curLogicFrame);
                }
            }
        }
    }


    public void DestoryEffect(SkillConfig_Effect config) {
        var configHashCode = config.GetHashCode();
        if (_dicEffectLogics.TryGetValue(configHashCode, out var skillEffectLogic)) {
            _dicEffectLogics.Remove(configHashCode);
            if (skillEffectLogic != null) {
                skillEffectLogic.OnDestory();
            }
        }
    }

    public void ReleaseAllEffect() {
        foreach (var configEffect in _skillConfigSo.effectCfgList) {
            if (!configEffect.IsAttachAction) {
                DestoryEffect(configEffect);
            }
        }
    }

    #endregion
}