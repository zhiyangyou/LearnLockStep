using System;
using System.Collections.Generic;
using FixIntPhysics;
using UnityEngine;

// [RequireComponent()]
public class FixIntBoxColliderEventMono : MonoBehaviour {
    #region 属性和字段

    private FixIntBoxCollider _fixIntBoxCollider;
    private List<FixIntBoxCollider> _listCheckTargets = new();
    private HashSet<FixIntBoxCollider> _setHasCollisonTargets = new();

    public bool isLogicUpdate = false;

    #endregion

    #region public

    public void AddCheckTarget(FixIntBoxCollider checkTarget) {
        if (checkTarget == null) {
            return;
        }
        _listCheckTargets.Add(checkTarget);
    }

    public void RemoveCheckTarget(FixIntBoxCollider checkTarget) {
        if (checkTarget == null) {
            return;
        }
        _listCheckTargets.Remove(checkTarget);
    }

    public void OnLogicFrameUpdate() {
        if (isLogicUpdate) {
            UpdateCollider();
        }
    }

    #endregion

    #region private

    private void Start() {
        BoxColliderGizmo gizmo = GetComponent<BoxColliderGizmo>();
        if (gizmo != null) {
            gizmo.enabled = false;
        }
        var pos = gizmo.mConter + transform.position;
        _fixIntBoxCollider = new FixIntBoxCollider(gizmo.mSize, pos);
        _fixIntBoxCollider.SetBoxData(pos, gizmo.mSize);
    }

    private void Update() {
        if (!isLogicUpdate) {
            UpdateCollider();
        }
    }

    private void UpdateCollider() {
        for (int i = _listCheckTargets.Count - 1; i >= 0; i--) {
            var target = _listCheckTargets[i];
            var isCollision = PhysicsManager.IsCollision(_fixIntBoxCollider, target);
            if (isCollision) {
                if (_setHasCollisonTargets.Contains(target)) {
                    // 持续碰撞
                    target.OnFixIntCollision_Stay?.Invoke(_fixIntBoxCollider, this);
                    _fixIntBoxCollider.OnFixIntCollision_Stay?.Invoke(target, null);
                }
                else {
                    // 首次碰撞
                    target.OnFixIntCollision_Enter?.Invoke(_fixIntBoxCollider, this);
                    _fixIntBoxCollider.OnFixIntCollision_Enter?.Invoke(target, null);
                    _setHasCollisonTargets.Add(target);
                }
            }
            else {
                if (_setHasCollisonTargets.Contains(target)) {
                    // 离开碰撞
                    _setHasCollisonTargets.Remove(target);
                    target.OnFixIntCollision_Exit?.Invoke(_fixIntBoxCollider, this);
                    _fixIntBoxCollider.OnFixIntCollision_Exit?.Invoke(target, null);
                }
            }
        }
    }

    #endregion
}