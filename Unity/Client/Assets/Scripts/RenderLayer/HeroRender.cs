using System;
using FixMath;
using UnityEngine;
using ZMGC.Battle;

public class HeroRender : RenderObject {
    #region 属性字段

    public Transform trLeftHand;
    public Transform trRightHand;
    private const string kStrAniName_Idle2 = "Anim_Idle02";
    private const string kStrAniName_Run = "Anim_Run";
    private HeroLogic _heroLogic;
    private Vector3 _curInputDir = Vector3.zero;
    private GameObject _goGuideEffect; // 技能引导特效对象

    private bool _hasInitJoyStick = false;
    // 角色动画
    private Animation _ani;

    private HeroLogic heroLogic {
        get {
            if (_heroLogic == null) {
                _heroLogic = this.LogicObject as HeroLogic;
            }
            return _heroLogic;
        }
    }

    #endregion

    #region public

    public override void PlayAnim(AnimationClip animationClip) {
        base.PlayAnim(animationClip);
        if (_ani.GetClip(animationClip.name) == null) {
            _ani.AddClip(animationClip, animationClip.name);
        }
        _ani.clip = animationClip;
        PlayAni(animationClip.name);
    }

    /// <summary>
    /// 更新技能引导
    /// </summary>
    /// <param name="skillGuideType"></param>
    /// <param name="skillId"></param>
    /// <param name="isPress"></param>
    /// <param name="pos">摇杆位置</param>
    /// <param name="skillRange">范围</param>
    public void UpdateSkillGuide(SKillGuideType skillGuideType, int skillId, bool isPress, Vector3 pos, float skillRange) {
        // 初始化引导特效
        InitSkillGuide(skillId);

        // 更新引导位置
        if (skillGuideType == SKillGuideType.Position) {
            Vector3 skillGuidePos = transform.position + pos;
            skillGuidePos = new Vector3(skillGuidePos.x, 0f, Mathf.Clamp(skillGuidePos.z, -1f, 8.6f)); // 限制位置
            _goGuideEffect.transform.localPosition = skillGuidePos;
        }
        else {
            Debug.LogError($"暂时不支持其他类型的引导:{skillGuideType}");
        }
    }

    public void OnGuideRelease() {
        if (_goGuideEffect != null) {
            GameObject.DestroyImmediate(_goGuideEffect);
            _goGuideEffect = null;
        }
    }

    #endregion

    #region life-cycle

    public override void OnCreate() {
        base.OnCreate();
        _ani = GetComponent<Animation>();
        if (_ani == null) Debug.LogError("Hero Render 没有Animation组件");
        // UIModule.Instance.GetWindow<BattleWindow>().uiCompt.
       
    }

    

    public override void OnRelease() {
        UIModule.Instance.GetWindow<BattleWindow>().uiCompt.StickJoystickUGUI.OnMoveCallBack = null;
        base.OnRelease();
    }

    protected override void Update() {
        if (heroLogic == null) {
            return;
        }
        TryInitJoyStick();
        base.Update();

        // 判断有没有在技能释放, 有技能释放,播放技能动画的动画片段
        if (!heroLogic.HasReleasingSkill) {
            // 判断摇杆是否有输入值, 如果没有,播放待机动画, 如果有播放跑步动画
            PlayAni(_curInputDir is { x: 0f, z: 0f } ? kStrAniName_Idle2 : kStrAniName_Run);
        }
    }


    public override Transform GetTransParent(TransParentType transParentType) {
        switch (transParentType) {
            case TransParentType.None:
                return null;
                break;
            case TransParentType.LeftHand:
                return trLeftHand;
                break;
            case TransParentType.RightHand:
                return trRightHand;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(transParentType), transParentType, null);
        }
    }

    #endregion

    #region private

    private void PlayAni(string aniName) {
        if (_ani == null) return;
        if (string.IsNullOrEmpty(aniName)) return;
        _ani.CrossFade(aniName, 0.2f);
    }

    private void OnJoyStickMove(Vector3 pos) {
        FixIntVector3 logicDir = FixIntVector3.zero;
        if (pos != Vector3.zero) {
            logicDir.x = pos.x;
            logicDir.y = pos.y;
            logicDir.z = pos.z;
        }
        _curInputDir = pos;
        if (heroLogic != null) {
            heroLogic.LogicFrameEvent_Input(logicDir);
        }
        else {
            Debug.LogError("HeroLogic is null");
        }
    }

    private void TryInitJoyStick() {
        if (!_hasInitJoyStick) {
            UIModule.Instance.GetWindow<BattleWindow>().uiCompt.StickJoystickUGUI.OnMoveCallBack = OnJoyStickMove;
            _hasInitJoyStick = true;
        }
    }
    
    private void InitSkillGuide(int skillID) {
        if (_goGuideEffect == null) {
            Skill skill = _heroLogic.GetSkill(skillID);
            if (skill == null) {
                Debug.LogError($"error skill ID {skillID}");
                return;
            }
            _goGuideEffect = GameObject.Instantiate(skill.SkillCfgConfig.skillGuideObj);
            _goGuideEffect.transform.localScale = Vector3.one;
        }
    }

    #endregion
}