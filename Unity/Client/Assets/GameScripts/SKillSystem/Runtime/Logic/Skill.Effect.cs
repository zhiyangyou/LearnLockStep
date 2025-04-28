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
        var effectList = _skillData.effectCfgList;
        if (effectList != null && effectList.Count > 0) {
            foreach (SkillEffectConfig item in effectList) {
                var effectConfig = item;
                if (effectConfig == null) {
                    var id = _skillData?.SkillCfg.skillID;
                    Debug.LogError($"skillEffectConfig is null : skillID {id.ToString()}");
                    continue;
                }
                if (effectConfig.skillEffect != null && _curLogicFrame == effectConfig.triggerFrame) {
                    DestoryEffectGo(effectConfig); // 避免重复释放技能导致特效对象重复出现

                    Transform trParent = null;
                    if (effectConfig.isSetTransParent) {
                        trParent = _skillCreater.RenderObject.GetTransParent(effectConfig.TransParentType);
                    }

                    // 创建技能特效渲染层
                    var goEffect = GameObject.Instantiate(effectConfig.skillEffect, trParent);
                    // goEffect.transform.localPosition = Vector3.zero;
                    // goEffect.transform.localScale = Vector3.one;
                    // goEffect.transform.localRotation = Quaternion.identity;
                    var effectRender = goEffect.GetComponent<SkillEffectRender>();
                    if (effectRender == null) effectRender = goEffect.AddComponent<SkillEffectRender>();

                    // 技能特效逻辑层
                    SkillEffectLogic skillEffectLogic = new SkillEffectLogic(LogicObjectType.Effect, effectConfig, effectRender, _skillCreater);
                    effectRender.SetLogicObject(skillEffectLogic, effectConfig.effectPosType != EffectPosType.Zero);

                    // 生命周期维护
                    _dicEffectLogics.Add(effectConfig.GetHashCode().GetHashCode(), skillEffectLogic);
                }

                // 结束之后,自动销毁
                if (_curLogicFrame == effectConfig.endFrame) {
                    DestoryEffectGo(effectConfig);
                }
            }
        }
    }


    public void DestoryEffectGo(SkillEffectConfig config) {
        var configHashCode = config.GetHashCode();
        if (_dicEffectLogics.TryGetValue(configHashCode, out var skillEffectLogic)) {
            _dicEffectLogics.Remove(configHashCode);
            if (skillEffectLogic != null) {
                skillEffectLogic.OnDestory();
            }
        }
    }

    #endregion
}