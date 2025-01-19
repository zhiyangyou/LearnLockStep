using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class SkillCompilerWindow : OdinEditorWindow
{
    [TabGroup("Skill", "Character", SdfIconType.Person, TextColor = "green")]
    public SkillCharacterConfig CharacterConfig = new();

    [MenuItem("Window/Skill/技能编译器 &`")]
    public static SkillCompilerWindow ShowWindow()
    {
        return GetWindowWithRect<SkillCompilerWindow>(new Rect(100, 100, 1000, 600));
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EditorApplication.update += OnEditorUpdate;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EditorApplication.update -= OnEditorUpdate;
    }

    private void OnEditorUpdate()
    {
        try
        {
            CharacterConfig.OnUpdate(Focus);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}