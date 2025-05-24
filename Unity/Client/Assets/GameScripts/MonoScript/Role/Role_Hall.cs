using System;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public partial class Role_Hall : MonoBehaviour {
    #region 属性和字段

    private Animation _anim;

    private Vector3 _inputDir = Vector3.zero;

    private Vector3 _renderDir;

    public float smoothSpeed = 2f;

    public float zMoveSpeedAdjust = 2.5f;

    #endregion

    #region private

    private void Awake() {
        _anim = GetComponent<Animation>();
        PlayAnim(AnimaNames.Anim_Idle02);
    }

    private void OnDestroy() {
        ReleaseCollider();
    }

    private void OnEnable() {
        JoystickUGUI.OnMoveCallBack += OnJoyStick;
    }

    private void OnDisable() {
        JoystickUGUI.OnMoveCallBack -= OnJoyStick;
    }

    private void OnJoyStick(Vector3 inputDir) {
        _inputDir = inputDir;
        _inputDir.z *= zMoveSpeedAdjust;
    }

    private void Update() {
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

    public void PlayAnim(string animName) {
        _anim.CrossFade(animName, 0.2f);
    }

    #endregion
}