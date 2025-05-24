using System.Collections.Generic;
using FixIntPhysics;
using Sirenix.OdinInspector;
using UnityEngine;

public class Map : MonoBehaviour {
    /// <summary>
    /// 地图初始位置
    /// </summary>
    public Vector3 mapInitPos;

    /// <summary>
    /// 人物可移动边界: 最小移动位置
    /// </summary>
    public Vector3 roleMoveMinPos;

    /// <summary>
    /// 人物可移动边界: 最大移动位置
    /// </summary>
    public Vector3 roleMoveMaxPos;

    public Transform trRoleInitPos;

    public MapType MapType;

    [SerializeField] [Header("所有入口碰撞体Box(自动赋值)")][DisableInEditorMode]
    private List<FixIntBoxColliderEventMono> _listAllEntryEventBox;

    [SerializeField] [Header("所有入口MapEntry(自动赋值)")] [DisableInEditorMode]
    public List<MapEntry> _ListAllMapEntry;

    public void AddEntryBoxCheckCollider(FixIntBoxCollider boxCollider) {
        if (boxCollider == null) {
            return;
        }
        foreach (FixIntBoxColliderEventMono boxColliderEvent in _listAllEntryEventBox) {
            boxColliderEvent.AddCheckTarget(boxCollider);
        }
    }

    public void RemoveEntryBoxCollider(FixIntBoxCollider boxCollider) {
        if (boxCollider == null) {
            return;
        }
        foreach (FixIntBoxColliderEventMono boxColliderEvent in _listAllEntryEventBox) {
            boxColliderEvent.RemoveCheckTarget(boxCollider);
        }
    }

    public void Init() {
        this.transform.position = mapInitPos;
        _listAllEntryEventBox.AddRange(GetComponentsInChildren<FixIntBoxColliderEventMono>());
        _ListAllMapEntry.AddRange(GetComponentsInChildren<MapEntry>());
    }
}