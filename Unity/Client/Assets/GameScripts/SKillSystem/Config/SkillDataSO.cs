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
    [FormerlySerializedAs("skill")] [FormerlySerializedAs("SkillCfg")] public SkillConfig skillCfg;
    public List<SkillConfig_Buff> buffCfgList;
    public List<SkillConfig_Damage> damageCfgList;
    public List<SkillConfig_Effect> effectCfgList;
    public List<SkillConfig_Audio> audioList;
    public List<SkillConfig_Bullet> bulletList;
    [FormerlySerializedAs("actionConfigs")] public List<SkillConfig_Action> actionList;

    public static void SaveSkillData(
        SkillConfig_Character configCharacter,
        SkillConfig skillConfig,
        List<SkillConfig_Damage> damageConfigs,
        List<SkillConfig_Buff> buffConfigs,
        List<SkillConfig_Effect> effectConfigs,
        List<SkillConfig_Audio> audioConfigs,
        List<SkillConfig_Bullet> bulletConfigs,
        List<SkillConfig_Action> actionConfigs
    ) {
        var so = ScriptableObject.CreateInstance<SkillConfigSO>();
        so.configCharacter = configCharacter;
        so.skillCfg = skillConfig;
        so.damageCfgList = damageConfigs;
        so.buffCfgList = buffConfigs;
        so.effectCfgList = effectConfigs;
        so.audioList = audioConfigs;
        so.bulletList = bulletConfigs;
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