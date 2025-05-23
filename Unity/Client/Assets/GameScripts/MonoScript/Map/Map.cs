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
    
    public void Init() {
        this.transform.position = mapInitPos;
    }
}