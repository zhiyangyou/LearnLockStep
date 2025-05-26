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

    public int roleID { get; private set; }

    #endregion

    #region private

    private void Awake() {
        _anim = GetComponent<Animation>();
        PlayAnim(AnimaNames.Anim_Idle02);
    }

    

    private void OnEnable() {
        if (roleSource == RoleSource.OtherPlayer) {
            JoystickUGUI.OnMoveCallBack += OnJoyStick;
        }
    }

    private void OnDisable() {
        if (roleSource == RoleSource.Self) {
            JoystickUGUI.OnMoveCallBack -= OnJoyStick;
        }
    }

    private void OnJoyStick(Vector3 inputDir) {
        if (roleSource == RoleSource.Self) {
            _inputDir = inputDir;
            _inputDir.z *= zMoveSpeedAdjust;
        }
    }

    private void Update() {
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
        if (_inputDir == Vector3.zero) {
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
        _anim?.CrossFade(animName, 0.2f);
    }

    #endregion
}