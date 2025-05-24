using UnityEngine;

public enum MapType {
    None,
    Home, // 赛利亚房间
    HeDunMaer, // 赫顿玛尔
    Dungeons, // 地下城
}

public enum DoorType {
    None,
    Home, // 赛利亚房间
    HeDunMaer, // 赫顿玛尔
    Dungeons, // 地下城
}

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