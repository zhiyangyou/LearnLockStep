using System;
using FixIntPhysics;
using UnityEngine;
using ZMGC.Hall;

public partial class Role_Hall {
    #region 属性字段

    private FixIntBoxCollider _fixIntBoxCollider = null;

    private Map _map;

    private Vector3 _boxOffset = Vector3.zero;

    #endregion

    #region public

    public void InitCollider() {
        _map = HallWorld.GetExitsLogicCtrl<MapLogicCtrl>().CurMap;
        if (_fixIntBoxCollider == null) {
            BoxColliderGizmo gizmo = GetComponent<BoxColliderGizmo>();
            if (gizmo) {
                gizmo.enabled = false;
            }
            _boxOffset = gizmo.mConter;
            _fixIntBoxCollider = new FixIntBoxCollider(gizmo.mSize, gizmo.mConter, needGizmos: false);
            _fixIntBoxCollider.SetBoxData(gizmo.mConter, gizmo.mSize);
            _fixIntBoxCollider.OnFixIntCollision_Enter += OnFixIntCollisionEnter;
            _fixIntBoxCollider.OnFixIntCollision_Stay += OnFixIntCollisionStay;
            _fixIntBoxCollider.OnFixIntCollision_Exit += OnFixIntCollisionExit;
        }
        _map.AddEntryBoxCheckCollider(_fixIntBoxCollider);
    }



    public void UpdateCollider() {
        if (_fixIntBoxCollider == null) {
            return;
        }
        var pos = transform.position + _boxOffset;
        _fixIntBoxCollider.UpdateColliderInfo(pos, _fixIntBoxCollider.Size.ToVector3());
    }

    public void ReleaseCollider() {
        _map.RemoveEntryBoxCollider(_fixIntBoxCollider);
        _fixIntBoxCollider.OnFixIntCollision_Enter = null;
    }

    #endregion

    #region private

    private void OnFixIntCollisionEnter(ColliderBehaviour other, MonoBehaviour otherMonoMayBeNull) {
        Debug.LogError($"Enter碰撞{otherMonoMayBeNull?.gameObject.name}");
    }

    private void OnFixIntCollisionExit(ColliderBehaviour other, MonoBehaviour otherMonoMayBeNull) {
        
        Debug.LogError($"Exit碰撞{otherMonoMayBeNull?.gameObject.name}");
    }


    private void OnFixIntCollisionStay(ColliderBehaviour other, MonoBehaviour otherMonoMayBeNull) {
        
        Debug.LogError($"Stay碰撞{otherMonoMayBeNull?.gameObject.name}");
    }

    #endregion

#if UNITY_EDITOR
    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_fixIntBoxCollider.LogicPosition.ToVector3(), _fixIntBoxCollider.Size.ToVector3());
    }
#endif
}