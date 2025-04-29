using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class SkillConfig_Audio {
    [AssetList] [BoxGroup("音效文件"), PreviewField(70, ObjectFieldAlignment.Left), OnValueChanged(nameof(OnValueChanged_SkillAudio))]
    public AudioClip skillAudio;

    [LabelText("音效文件名称")] [BoxGroup("音效文件"), ReadOnly, GUIColor("green")]
    public string audioName;

    [LabelText("触发帧"), BoxGroup("参数配置"), GUIColor("green")]
    public int triggerFrame;

    [ToggleGroup("isLoop", "是否循环")] public bool isLoop;

    [ToggleGroup("isLoop", "结束帧")] public int endFrame;

    private void OnValueChanged_SkillAudio(AudioClip clip) {
        if (clip != null) {
            audioName = clip.name;
        }
    }
}