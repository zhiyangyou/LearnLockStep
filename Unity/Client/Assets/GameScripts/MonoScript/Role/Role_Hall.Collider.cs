using System;
using System.Threading.Tasks;
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

    public void Init() {
        if (_fixIntBoxCollider == null) {
            BoxColliderGizmo gizmo = GetComponent<BoxColliderGizmo>();
            if (gizmo) {
                gizmo.enabled = false;
            }
            _boxOffset = gizmo.mConter;
            _fixIntBoxCollider = new FixIntBoxCollider(gizmo.mSize, gizmo.mConter, needGizmos: false);
            _fixIntBoxCollider.SetBoxData(gizmo.mConter, gizmo.mSize);
            _fixIntBoxCollider.OnFixIntCollision_Enter += OnFixIntCollisionEnter;
        }
        _map = HallWorld.GetExitsLogicCtrl<MapLogicCtrl>().CurMap;
        _map.AddEntryBoxCheckCollider(_fixIntBoxCollider);
        _hallRoleLogicCtrl = HallWorld.GetExitsLogicCtrl<HallRoleLogicCtrl>();
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

    private async void OnFixIntCollisionEnter(ColliderBehaviour other, MonoBehaviour otherMonoMayBeNull) {
        if (otherMonoMayBeNull == null) {
            Debug.LogError("目标碰撞体上没有Mono组件");
            return;
        }
        if (!otherMonoMayBeNull.gameObject.TryGetComponent<MapEntry>(out var mapEntry)) {
            Debug.LogError("目标碰撞体上没有MapEntry组件");
            return;
        }
        // Debug.LogError($" 地图传送: from: {mapEntry.GotoMapType} to: {mapEntry.GotoDoorType}");
        var mapCtrl = HallWorld.GetExitsLogicCtrl<MapLogicCtrl>();
        var originMapType = mapCtrl.CurMap.MapType;
        var gotoMapType = mapEntry.GotoMapType;
        await mapCtrl.LoadMapAsync(gotoMapType);
        Vector3? roleInitPos = mapCtrl.GetMapEntryPos(originMapType);
        if (roleInitPos == null) {
            Debug.LogError($"找不到  from:{originMapType} to:{gotoMapType} 对应门的位置");
        }
        HallWorld.GetExitsLogicCtrl<HallRoleLogicCtrl>().InitRoleEnv(roleInitPos == null ? Vector3.zero : roleInitPos.Value);
    }

    #endregion

#if UNITY_EDITOR
    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_fixIntBoxCollider.LogicPosition.ToVector3(), _fixIntBoxCollider.Size.ToVector3());
    }
#endif
}