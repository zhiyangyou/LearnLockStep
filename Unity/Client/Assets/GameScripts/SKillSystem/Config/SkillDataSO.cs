using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "SkillConfig", menuName = "SkillConfig", order = 0)]
public class SkillDataSO : ScriptableObject
{
    public const string SKillDataBasePath = "Assets/GameData/Game/SkillSystem/SkillData/";
    public SkillCharacterConfig character;
    public SkillConfig SkillCfg;
    public List<SkillDamageConfig> damageCfgList;
    public List<SkillEffectConfig> effectCfgList;

    public static void SaveSkillData(
        SkillCharacterConfig characterConfig,
        SkillConfig skillConfig,
        List<SkillDamageConfig> damageConfigs,
        List<SkillEffectConfig> effectConfigs
    )
    {
        var so = ScriptableObject.CreateInstance<SkillDataSO>();
        so.character = characterConfig;
        so.SkillCfg = skillConfig;
        so.damageCfgList = damageConfigs;
        so.effectCfgList = effectConfigs;

        var assetPath = $"{SKillDataBasePath}{skillConfig.skillID.ToString()}.asset";
        AssetDatabase.DeleteAsset(assetPath);
        AssetDatabase.CreateAsset(so, assetPath);
    }

#if UNITY_EDITOR


    [Button("配置技能窗口", ButtonSizes.Large), GUIColor("green")]
    public void ShowSkillWindowButtonClick()
    {
        var window = SkillCompilerWindow.ShowWindow();
        window.LoadSkillData(this);
    }
#endif
}