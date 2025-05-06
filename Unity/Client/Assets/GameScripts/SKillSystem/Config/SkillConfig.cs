using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[HideMonoScript]
[System.Serializable]
public class SkillConfig {
    private const string kStrTitleGroup_技能渲染相关 = "技能渲染相关";
    private const string kStrSubTitleGroup_技能渲染相关 = "所有英雄渲染数据会在开始释放技能是触发";

    private bool _showStockPileData = false; // 是否显示蓄力技能数据
    private bool _showSkillGuide = false; // 是否显示引导技能

    [LabelText("技能图标"),
     LabelWidth(0.1f),
     PreviewField(70, ObjectFieldAlignment.Left),
     SuffixLabel("技能图标")]
    public Sprite skillIcon;

    [LabelText("技能id")] public int skillID;

    [LabelText("名称")] public string skillName;

    [LabelText("消耗蓝量")] public int needMagicValue;

    [LabelText("前摇时间")] public int skillShakeBeforeTimeMs;

    [LabelText("持续时间(毫秒)")] public int skillAttackDurationMs;

    [LabelText("后摇时间")] public int skillShakeAfterTimeMs;

    [LabelText("CD")] public int skillCDTimeMS;

    [LabelText("技能类型"), OnValueChanged(nameof(OnSkillTypeChanged))]
    public SkillType SkillType;


    [LabelText("蓄力阶段配置数据(若第一阶段触发时间不是0,则空挡时间为动画表现时间)"), ShowIf(nameof(_showStockPileData))]
    public List<StockPileStageData> stockPIleStageDatas;

    [LabelText("技能引导特效"), ShowIf(nameof(_showSkillGuide))]
    public GameObject skillGuideObj;

    [LabelText("技能引导范围"), ShowIf(nameof(_showSkillGuide))]
    public float skillGuideRange;

    [LabelText("组合技能id(衔接下一个的技能Id)"), Tooltip("比如技能A:由B C 组成")]
    public int CombinationSkillId;

    // 技能渲染相关
    [LabelText("技能命中特效"), TitleGroup(kStrTitleGroup_技能渲染相关, kStrSubTitleGroup_技能渲染相关)]
    public GameObject skillHitEffect;

    [LabelText("技能特效存活时间"), TitleGroup(kStrTitleGroup_技能渲染相关, kStrSubTitleGroup_技能渲染相关)]
    public int hitEffectSurvialTimeMs;

    [LabelText("技能命中音效"), TitleGroup(kStrTitleGroup_技能渲染相关, kStrSubTitleGroup_技能渲染相关)]
    public AudioClip SkillHitAudio;

    [LabelText("是否显示技能立绘"), TitleGroup(kStrTitleGroup_技能渲染相关, kStrSubTitleGroup_技能渲染相关)]
    public bool showSkillPortrait;

    [LabelText("技能立绘"),
     TitleGroup(kStrTitleGroup_技能渲染相关, kStrSubTitleGroup_技能渲染相关),
     ShowIf(nameof(showSkillPortrait))
    ]
    public GameObject skillPortraitObj;

    [LabelText("技能描述"), TitleGroup(kStrTitleGroup_技能渲染相关, kStrSubTitleGroup_技能渲染相关)]
    public string skillDes;


    public bool HasCombineSkill => CombinationSkillId > 0;

    /// <summary>
    /// 技能类型改变回调
    /// </summary>
    public void OnSkillTypeChanged(SkillType skillType) {
        _showStockPileData = skillType == SkillType.StockPile;
        _showSkillGuide = skillType == SkillType.PosGuide;
    }
}

public enum SkillType {
    [LabelText("瞬发技能")] None,

    [LabelText("吟唱技能")] Chat,

    [LabelText("弹道技能")] Ballistic,

    [LabelText("蓄力技能")] StockPile,

    [LabelText("位置引导技能")] PosGuide,
}


/// <summary>
/// 蓄力阶段数据
/// </summary>
[Serializable]
public class StockPileStageData {
    [LabelText("蓄力阶段id")] public int stage;

    [LabelText("当前蓄力阶段触发的技能id")] public int skillId;

    [LabelText("当前阶段触发开始时间")] public int startTimeMs;

    [LabelText("当前阶段结束时间")] public int endTimeMs;
}