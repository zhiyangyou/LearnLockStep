using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Serialization;

[HideMonoScript]
[System.Serializable]
public class SkillCharacterConfig
{
    #region 程序字段和属性

    private GameObject _goTempCharacter = null;

    #endregion

    #region odin序列化属性和字段

    [AssetList]
    [LabelText("角色模型")]
    [PreviewField(90, ObjectFieldAlignment.Center)]
    public GameObject sKillCharacterPrefab;


    [TitleGroup("技能渲染", "所有英雄渲染数据会在技能开始释放时触发")]
    [LabelText("技能动画")]
    public AnimationClip skillAnim;

    [BoxGroup("动画数据")]
    [ProgressBar(0f, 100f, r: 0, g: 255, b: 0, Height = 30)]
    [HideLabel]
    public short animProgress = 0;

    [BoxGroup("动画数据")]
    [LabelText("是否循环动画")]
    public bool isLoopAnim;

    [BoxGroup("动画数据")]
    [LabelText("动画循环次数")]
    [ShowIf(nameof(isLoopAnim))]
    public int animLoopCount;

    [BoxGroup("动画数据")]
    [LabelText("逻辑帧数")]
    public int logicFrame = 0;

    [BoxGroup("动画数据")]
    [LabelText("动画长度")]
    public float animLength = 0f;

    [BoxGroup("动画数据")]
    [LabelText("技能推荐时长(毫秒ms)")]
    public float skillDurationMS = 0f;

    #endregion

    #region 按钮

    [GUIColor(0, 0.7f, 0.2f)]
    [ButtonGroup("操作按钮组")]
    [Button("播放", ButtonSizes.Large)]
    public void Play()
    {
        if (sKillCharacterPrefab != null)
        {
            // 先从场景中查找技能对线，找不到就克隆
            string name = sKillCharacterPrefab.name;
            _goTempCharacter = GameObject.Find(name);
            if (_goTempCharacter == null)
            {
                _goTempCharacter = GameObject.Instantiate(sKillCharacterPrefab);
                _goTempCharacter.name = name;
            }

            // 模型身上有无该动画，没有就添加
            var animation = _goTempCharacter.GetComponent<Animation>();
            if (!animation.GetClip(skillAnim.name))
                animation.AddClip(skillAnim, skillAnim.name);
            animation.clip = skillAnim;

            // 动画文件长度
            animLength = isLoopAnim ? skillAnim.length * animLoopCount : skillAnim.length;

            // 对应的逻辑帧的数量
            logicFrame = (int)(isLoopAnim
                ? skillAnim.length / GameConst.OneLogicFrameTime * animLoopCount
                : skillAnim.length / GameConst.OneLogicFrameTime);

            // 技能时长
            skillDurationMS = (int)(animLength * 1000f);
        }
    }

    [GUIColor(0, 0.3f, 0.8f)]
    [ButtonGroup("操作按钮组")]
    [Button("暂停", ButtonSizes.Large)]
    public void Pause()
    {
    }


    [GUIColor(0.7f, 0.2f, 0)]
    [ButtonGroup("操作按钮组")]
    [Button("保存", ButtonSizes.Large)]
    public void SafeAssets()
    {
    }

    [GUIColor(0.7f, 0.7f, 0.7f)]
    [ButtonGroup("操作按钮组")]
    [Button("打开战斗场景", ButtonSizes.Large)]
    public void OpenBattleScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Battle.unity");
    }

    #endregion
}