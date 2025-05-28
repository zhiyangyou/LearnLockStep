using System;
using ServerShareToClient;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

[HideMonoScript]
[System.Serializable]
public class SkillConfig_Character {
    #region const

    private const string kStr操作按钮组 = "操作按钮组";

    #endregion

    #region 程序字段和属性

    private GameObject _goTempCharacter = null;

    /// <summary>
    /// 控制动画
    /// </summary>
    private bool _isPlayAnimation = false;


    /// <summary>
    /// 上次运行的时间
    /// </summary>
    private double _lastRuntime = 0;

    private Animation _curPlayAnimation = null;

    #endregion

    #region odin序列化属性和字段

    [AssetList] [LabelText("角色模型")] [PreviewField(90, ObjectFieldAlignment.Center)]
    public GameObject sKillCharacterPrefab;


    [TitleGroup("技能渲染", "所有英雄渲染数据会在技能开始释放时触发")] [LabelText("技能动画")]
    public AnimationClip skillAnim;

    [BoxGroup("kStr动画数据")] [ProgressBar(0f, 100f, r: 0, g: 255, b: 0, Height = 30)] [HideLabel] [OnValueChanged(nameof(OnValueChanged_AnimProgress), InvokeOnUndoRedo = true)]
    public short animProgress = 0;

    [BoxGroup("kStr动画数据")] [LabelText("是否循环动画")]
    public bool isLoopAnim;

    [BoxGroup("kStr动画数据")] [LabelText("动画循环次数")] [ShowIf(nameof(isLoopAnim))]
    public int animLoopCount;

    [BoxGroup("kStr动画数据")] [LabelText("逻辑帧数"), HideIf(nameof(isSetCustomLogicFrame))]
    public int logicFrame = 0;

    [BoxGroup("kStr动画数据")] [LabelText("是否设置自定义逻辑帧数")]
    public bool isSetCustomLogicFrame = false;

    [BoxGroup("kStr动画数据")] [LabelText("自定义逻辑帧数"), ShowIf(nameof(isSetCustomLogicFrame))]
    public int customLogicFrame = 0;

    [BoxGroup("kStr动画数据")] [LabelText("动画长度")]
    public float animLength = 0f;

    [BoxGroup("kStr动画数据")] [LabelText("技能推荐时长(毫秒ms)")]
    public float skillDurationMS = 0f;

    #endregion

    #region 按钮

    [GUIColor(0, 0.7f, 0.2f)]
    [ButtonGroup(kStr操作按钮组)]
    [Button("播放", ButtonSizes.Large)]
    public void Play() {
        if (sKillCharacterPrefab != null) {
            _curPlayAnimation = TryInitTempCharacter();

            // 动画文件长度
            animLength = isLoopAnim ? skillAnim.length * animLoopCount : skillAnim.length;

            // 对应的逻辑帧的数量
            logicFrame = (int)(isLoopAnim
                ? skillAnim.length / GameConst.OneLogicFrameTime * animLoopCount
                : skillAnim.length / GameConst.OneLogicFrameTime);

            // 技能时长
            skillDurationMS = (int)(animLength * 1000f);

            // 开始播放角色动画
            _lastRuntime = 0;
            _isPlayAnimation = true;
            _curPlayAnimation.Play();
#if UNITY_EDITOR
            var win = SkillCompilerWindow.GetWindow();
            win?.PlaySkillStart();
#endif
        }
    }

    [GUIColor(0, 0.3f, 0.8f)]
    [ButtonGroup(kStr操作按钮组)]
    [Button("暂停", ButtonSizes.Large)]
    public void Pause() {
#if UNITY_EDITOR
        _isPlayAnimation = false;
        var win = SkillCompilerWindow.GetWindow();
        win?.SkillPause();
#endif
    }


    [GUIColor(0.7f, 0.2f, 0)]
    [ButtonGroup(kStr操作按钮组)]
    [Button("保存", ButtonSizes.Large)]
    public void SafeAssets() {
#if UNITY_EDITOR
        SkillCompilerWindow.GetWindow().SaveSkillData();
#endif
    }

    [GUIColor(0.7f, 0.7f, 0.7f)]
    [ButtonGroup(kStr操作按钮组)]
    [Button("打开战斗场景", ButtonSizes.Large)]
    public void OpenBattleScene() {
#if UNITY_EDITOR
        EditorSceneManager.OpenScene("Assets/Scenes/Battle.unity");
#endif
    }

    #endregion

    #region private

#if UNITY_EDITOR

    public void OnEditorUpdate(Action onUpdateSkillAnim) {
        if (_isPlayAnimation) {
            if (_lastRuntime == 0) {
                _lastRuntime = EditorApplication.timeSinceStartup;
            }
            // 当前运行时间
            double curRuntime = EditorApplication.timeSinceStartup - _lastRuntime;

            // 动画播放进度
            float curAniNormalizationValue = (float)curRuntime / animLength;
            animProgress = (short)(Mathf.Clamp(curAniNormalizationValue * 100, 0, 100));

            // 计算逻辑帧
            logicFrame = (int)(curRuntime / GameConstConfig.LogicFrameInterval);

            // 采样动画
            _curPlayAnimation.clip.SampleAnimation(_goTempCharacter, (float)curRuntime);

            if (animProgress >= 100) {
                // 播放完成
                PlaySkillEnd();
            }
            onUpdateSkillAnim?.Invoke();
        }
    }

#endif
    /// <summary>
    /// 先从场景中查找技能对象，找不到就克隆
    /// </summary>
    private Animation TryInitTempCharacter() {
        // 先从场景中查找技能对线，找不到就克隆
        string name = sKillCharacterPrefab.name;
        _goTempCharacter = GameObject.Find(name);
        if (_goTempCharacter == null) {
            _goTempCharacter = GameObject.Instantiate(sKillCharacterPrefab);
            _goTempCharacter.name = name;
        }

        // 模型身上有无该动画，没有就添加
        var animation = _goTempCharacter.GetComponent<Animation>();
        if (!animation.GetClip(skillAnim.name))
            animation.AddClip(skillAnim, skillAnim.name);
        animation.clip = skillAnim;

        return animation;
    }

    private void PlaySkillEnd() {
#if UNITY_EDITOR
        _isPlayAnimation = false;
        SkillCompilerWindow.GetWindow()?.PlaySkillEnd();
#endif
    }

    private void OnValueChanged_AnimProgress(float value) {
        var animation = TryInitTempCharacter();

        // 根据当前动画进度进行采样
        float progress = (value / 100) * skillAnim.length;
        logicFrame = (int)(progress / GameConstConfig.LogicFrameInterval);
        animation.clip.SampleAnimation(_goTempCharacter, progress);
    }

    #endregion
}