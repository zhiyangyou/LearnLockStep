#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class SkillCompilerWindow : OdinEditorWindow {
    [FormerlySerializedAs("CharacterConfig")] [TabGroup("Skill", "Character", SdfIconType.Person, TextColor = "green")]
    public SkillConfig_Character configCharacter = new();

    [TabGroup("SkillCompiler", "Skill", SdfIconType.Robot, TextColor = "lightmagenta")]
    public SkillConfig skill = new();

    [TabGroup("SkillCompiler", "Damage", SdfIconType.At, TextColor = "lightmagenta")]
    public List<SkillConfig_Damage> damageList = new();

    [TabGroup("SkillCompiler", "Effect", SdfIconType.OpticalAudio, TextColor = "blue")]
    public List<SkillConfig_Effect> effectList = new();

    [TabGroup("SkillCompiler", "Audio", SdfIconType.OpticalAudio, TextColor = "blue")]
    public List<SkillConfig_Audio> audioList = new();

    [TabGroup("SkillCompiler", "Action", SdfIconType.OpticalAudio, TextColor = "cyan")]
    public List<SkillConfig_Action> actionList = new();

    [MenuItem("Window/Skill/技能编译器 &`")]
    public static SkillCompilerWindow ShowWindow() {
        return GetWindowWithRect<SkillCompilerWindow>(new Rect(100, 100, 1000, 600));
    }

    public void SaveSkillData() {
        SkillConfigSO.SaveSkillData(
            configCharacter,
            skill,
            damageList,
            effectList,
            audioList,
            actionList
        );
    }

    public void LoadSkillData(SkillConfigSO so) {
        this.configCharacter = so.configCharacter;
        this.skill = so.skill;
        this.effectList = so.effectCfgList;
        this.damageList = so.damageCfgList;
        this.audioList = so.audioList;
        this.actionList = so.actionList;
    }

    public static SkillCompilerWindow GetWindow() {
        return GetWindow<SkillCompilerWindow>();
    }


    #region 状态字段

    /// <summary>
    /// 
    /// </summary>
    [NonSerialized] private bool _isStartPlaySkill = false;

    /// 逻辑帧累计运行的时间
    private float _accLogicRuntime = 0f;

    // 下一个逻辑帧的时间
    private float _nextLogicTime = 0f;

    // 动画缓动时间,当前帧的增量时间
    private float _deltaTime = 0f;

    //  上次更新的时间
    private double _lastUpdateTime = 0f;

    #endregion

    #region static

    /// <summary>
    /// 获取角色的坐标位置
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetCharacterPos() {
        var win = GetWindow<SkillCompilerWindow>();
        if (win == null) return Vector3.zero;
        return win.configCharacter.sKillCharacterPrefab.transform.position;
    }

    #endregion

    protected override void OnEnable() {
        base.OnEnable();
        foreach (var damageConfig in damageList) {
            damageConfig.OnInit();
        }
        EditorApplication.update += OnEditorUpdate;
    }

    protected override void OnDisable() {
        base.OnDisable();
        foreach (var damageConfig in damageList) {
            damageConfig.OnRelease();
        }
        EditorApplication.update -= OnEditorUpdate;
    }

    public void PlaySkillStart() {
        foreach (var effectConfig in effectList) {
            effectConfig.PlaySkillStart();
        }
        foreach (var damageConfig in damageList) {
            damageConfig.PlaySkillStart();
        }
        _accLogicRuntime = 0f;
        _nextLogicTime = 0f;
        _lastUpdateTime = 0f;

        _isStartPlaySkill = true;
    }

    public void SkillPause() {
        foreach (var effectConfig in effectList) {
            effectConfig.SkillPause();
        }
        foreach (var damageConfig in damageList) {
            damageConfig.PlaySkillEnd();
        }
    }

    public void PlaySkillEnd() {
        foreach (var effectConfig in effectList) {
            effectConfig.PlaySkillEnd();
        }
        foreach (var damageConfig in damageList) {
            damageConfig.PlaySkillEnd();
        }
        _isStartPlaySkill = false;
    }

    private void OnEditorUpdate() {
        try {
            configCharacter.OnUpdate(Focus);
            if (_isStartPlaySkill) {
                OnLogicUpdate();
            }
        }
        catch (Exception e) {
            Debug.LogException(e);
        }
    }

    /// <summary>
    /// 模拟帧同步的更新
    /// </summary>
    public void OnLogicUpdate() {
        if (_lastUpdateTime == 0f) {
            _lastUpdateTime = EditorApplication.timeSinceStartup;
        }
        _accLogicRuntime = (float)(EditorApplication.timeSinceStartup - _lastUpdateTime);

        while (_accLogicRuntime > _nextLogicTime) {
            OnLogicFrameUpdate();

            // 下一个逻辑帧的时间
            _nextLogicTime += LogicFrameConfig.LogicFrameInterval;
        }
    }

    public void OnLogicFrameUpdate() {
        foreach (var effectConfig in effectList) {
            effectConfig.OnLogicFrameUpdate();
        }
        foreach (var damageConfig in damageList) {
            damageConfig.OnLogicFrameUpdate();
        }
    }
}
#endif