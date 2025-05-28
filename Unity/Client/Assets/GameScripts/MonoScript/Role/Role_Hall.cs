using System;
using UnityEngine;
using ZM.ZMAsset;


public enum RoleSource {
    Self,
    OtherPlayer,
}

[RequireComponent(typeof(Animation))]
public partial class Role_Hall : MonoBehaviour {
    #region 属性和字段

    private Animation _anim;

    private Vector3 _inputDir = Vector3.zero;

    private Vector3 _renderDir;

    private bool _moveActive = false;

    public float smoothSpeed = 8f;

    public float zMoveSpeedAdjust = 2.5f;

    public RoleSource roleSource = RoleSource.OtherPlayer;

    private int _roleID { get; set; }

    #endregion

    #region private

    private void Awake() {
        _anim = GetComponent<Animation>();
        PlayAnim(AnimaNames.Anim_Idle02);
    }


    private void OnEnable() {
        // if (roleSource == RoleSource.Self)
        {
            UIModule.Instance.GetWindow<HallWindow>().uiCompt.RoleJoystickUGUI.OnMoveCallBack = OnJoyStick;
        }
    }

    private void OnDisable() {
        // if (roleSource == RoleSource.Self) 
        {
            var hallWindow = UIModule.Instance.GetWindow<HallWindow>();
            if (hallWindow != null) {
                hallWindow.uiCompt.RoleJoystickUGUI.OnMoveCallBack = null;
            }
        }
    }

    private void OnJoyStick(Vector3 inputDir) {
        if (roleSource == RoleSource.Self) {
            _inputDir = inputDir;
            _inputDir.z *= zMoveSpeedAdjust;
        }
    }

    private void Update() {
        // Debug.LogError($"{_roleID } move : {_moveActive} _inputDir:{_inputDir}");
        if (!_moveActive) {
            return;
        }
        UpdatePos();
        UpdateDir();
        UpdateState();
        UpdateCollider();
    }

    private void UpdatePos() {
        // Vector3 targetPos = transform.position + _inputDir;
        transform.position = Vector3.Lerp(transform.position, _syncTargetPos, Time.deltaTime * smoothSpeed);
    }

    private void UpdateDir() {
        if (_syncDir.x != 0) {
            _renderDir.y = _syncDir.x > 0f ? 0f : 180f;
            transform.localEulerAngles = _renderDir;
        }
    }


    private void UpdateState() {
        var judgeDir = roleSource == RoleSource.OtherPlayer ? _syncDir : _inputDir;

        if (judgeDir == Vector3.zero) {
            PlayAnim(AnimaNames.Anim_Idle02);
        }
        else {
            PlayAnim(AnimaNames.Anim_Walk);
        }
    }

    #endregion

    #region public

    public void OnRlease() {
        ZMAsset.Release(gameObject);
        ReleaseCollider();
    }

    public void ActiveMove(bool moveActive) {
        if (!moveActive) { }
        _moveActive = moveActive;
        if (moveActive) {
            PlayAnim(AnimaNames.Anim_Idle02);
        }
    }

    public void PlayAnim(string animName) {
        if (_anim == null) {
            return;
        }
        _anim?.CrossFade(animName, 0.2f);
    }

    #endregion
}