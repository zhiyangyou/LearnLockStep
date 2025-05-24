using UnityEngine;



public class MapEntry : MonoBehaviour {
    /// <summary>
    /// 要去的地图
    /// </summary>
    [Header("要去的地图")] public MapType GotoMapType;

    /// <summary>
    /// 要去指定地图的哪一个门?
    /// </summary>
    [Header("要去指定地图的哪一个门?")] public DoorType GotoDoorType;


    [Header("门所在的初始化位置")] public Transform trDoorInitRolePos;
    
    public Vector3 DoorPos=> trDoorInitRolePos.position;
}