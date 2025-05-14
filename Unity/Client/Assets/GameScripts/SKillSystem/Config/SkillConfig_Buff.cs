using System;
using Sirenix.OdinInspector;

[Serializable]
public class SkillConfig_Buff {
    [LabelText("触发帧")] public int triggerFrame;


    [LabelText("附加buffID")] public int buffID;
}