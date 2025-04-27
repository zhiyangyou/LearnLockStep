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
                var itemEffect = item;
                if (itemEffect == null) {
                    var id = _skillData?.SkillCfg.skillID;
                    Debug.LogError($"skillEffectConfig is null : skillID {id.ToString()}");
                    continue;
                }
                if (itemEffect.skillEffect != null && _curLogicFrame == itemEffect.triggerFrame) {
                    DestoryEffectGo(itemEffect); // 避免重复释放技能导致特效对象重复出现

                    // 创建技能特效渲染层
                    var goEffect = GameObject.Instantiate(itemEffect.skillEffect);
                    // goEffect.transform.localPosition = Vector3.zero;
                    // goEffect.transform.localScale = Vector3.one;
                    // goEffect.transform.localRotation = Quaternion.identity;
                    var effectRender = goEffect.GetComponent<SkillEffectRender>();
                    if (effectRender == null) effectRender = goEffect.AddComponent<SkillEffectRender>();
                    
                    // 技能特效逻辑层
                    SkillEffectLogic skillEffectLogic = new SkillEffectLogic(LogicObjectType.Effect, itemEffect, effectRender, _skillCreater);
                    effectRender.SetLogicObject(skillEffectLogic);
                    effectRender.UpdatePosAndDir(); // 需要立即更新一次位置 

                    // 生命周期维护
                    _dicEffectLogics.Add(itemEffect.GetHashCode().GetHashCode(), skillEffectLogic);
                }

                // 结束之后,自动销毁
                if (_curLogicFrame == itemEffect.endFrame) {
                    DestoryEffectGo(itemEffect);
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