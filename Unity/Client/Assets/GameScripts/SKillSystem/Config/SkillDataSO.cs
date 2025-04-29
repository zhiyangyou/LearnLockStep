using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "SkillConfig", menuName = "SkillConfig", order = 0)]
public class SkillConfigSO : ScriptableObject {
    public const string SKillDataBasePath = "Assets/GameData/Game/SkillSystem/SkillData/";

    [FormerlySerializedAs("character")] public SkillConfig_Character configCharacter;
    [FormerlySerializedAs("SkillCfg")] public SkillConfig skill;
    public List<SkillConfig_Damage> damageCfgList;
    public List<SkillConfig_Effect> effectCfgList;
    public List<SkillConfig_Audio> audioList;
    [FormerlySerializedAs("actionConfigs")] public List<SkillConfig_Action> actionList;

    public static void SaveSkillData(
        SkillConfig_Character configCharacter,
        SkillConfig skillConfig,
        List<SkillConfig_Damage> damageConfigs,
        List<SkillConfig_Effect> effectConfigs,
        List<SkillConfig_Audio> audioConfigs,
        List<SkillConfig_Action> actionConfigs
    ) {
        var so = ScriptableObject.CreateInstance<SkillConfigSO>();
        so.configCharacter = configCharacter;
        so.skill = skillConfig;
        so.damageCfgList = damageConfigs;
        so.effectCfgList = effectConfigs;
        so.audioList = audioConfigs;
        so.actionList = actionConfigs;


        var assetPath = $"{SKillDataBasePath}{skillConfig.skillID.ToString()}.asset";
        AssetDatabase.DeleteAsset(assetPath);
        AssetDatabase.CreateAsset(so, assetPath);
    }

#if UNITY_EDITOR


    [Button("配置技能窗口", ButtonSizes.Large), GUIColor("green")]
    public void ShowSkillWindowButtonClick() {
        var window = SkillCompilerWindow.ShowWindow();
        window.LoadSkillData(this);
    }
#endif
}