using UnityEngine;

public partial class Skill
{
    public void OnLogicFrameUpdate_Effect()
    {
        var effectList = _skillData.effectCfgList;
        if (effectList != null && effectList.Count > 0)
        {
            foreach (SkillEffectConfig itemEffect in effectList)
            {
                if (itemEffect == null)
                {
                    var id = _skillData?.SkillCfg.skillID;
                    Debug.LogError($"skillEffectConfig is null : skillID {id.ToString()}");
                    continue;
                }
                if (itemEffect.skillEffect != null
                    && _curLogicFrame == itemEffect.triggerFrame
                   )
                {
                    // 技能特效生成触发
                    var goEffect = GameObject.Instantiate(itemEffect.skillEffect);
                    goEffect.transform.localPosition = Vector3.zero;
                    goEffect.transform.localScale = Vector3.one;
                    goEffect.transform.localRotation = Quaternion.identity;
                    itemEffect.GoEffect = goEffect;
                }
                if (_curLogicFrame == itemEffect.endFrame)
                {
                    GameObject.Destroy(itemEffect.GoEffect);
                    // 技能特效结束,开始销毁
                }
            }
        }
    }
}