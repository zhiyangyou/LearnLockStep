using System.Collections.Generic;
using UnityEngine;

public partial class Skill
{
    #region 属性和字段

    /// <summary>
    /// 特效物体字段,k是物体的hash值,value是GameOBject
    /// </summary>
    private Dictionary<int, GameObject> _dicEffectGos = new();

    #endregion

    #region public

    public void OnLogicFrameUpdate_Effect()
    {
        var effectList = _skillData.effectCfgList;
        if (effectList != null && effectList.Count > 0)
        {
            foreach (SkillEffectConfig item in effectList)
            {
                var itemEffect = item;
                if (itemEffect == null)
                {
                    var id = _skillData?.SkillCfg.skillID;
                    Debug.LogError($"skillEffectConfig is null : skillID {id.ToString()}");
                    continue;
                }
                if (itemEffect.skillEffect != null && _curLogicFrame == itemEffect.triggerFrame)
                {
                    DestoryEffectGo(itemEffect); // 避免重复释放技能导致特效对象重复出现
                    // 技能特效生成触发
                    var goEffect = GameObject.Instantiate(itemEffect.skillEffect);
                    _dicEffectGos.Add(itemEffect.GetHashCode().GetHashCode(), goEffect);
                    goEffect.transform.localPosition = Vector3.zero;
                    goEffect.transform.localScale = Vector3.one;
                    goEffect.transform.localRotation = Quaternion.identity;
                }
                if (_curLogicFrame == itemEffect.endFrame)
                {
                    DestoryEffectGo(itemEffect);
                    // 技能特效结束,开始销毁
                }
            }
        }
    }


    public void DestoryEffectGo(SkillEffectConfig config)
    {
        GameObject effectGo = null;
        var configHashCode = config.GetHashCode();
        if (_dicEffectGos.TryGetValue(configHashCode, out var go))
        {
            _dicEffectGos.Remove(configHashCode);
            if (go != null)
            {
                GameObject.Destroy(go);
            }
        }
    }

    #endregion
}