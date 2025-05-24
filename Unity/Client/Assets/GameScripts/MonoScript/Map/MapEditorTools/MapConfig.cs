using Fantasy;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
 
    
public class MapConfig 
{
    public string mapName;

    public MapType mapType;

    /// <summary>
    /// 最小移动位置(通过读取地图配置进行赋值，如果测试代码可以通过手动或运行时赋值)
    /// </summary>
    public CSVector3 minPosition;
    /// <summary>
    /// 最大移动位置(通过读取地图配置进行赋值，如果测试代码可以通过手动或运行时赋值)
    /// </summary>
    public CSVector3 maxPosition;
    /// <summary>
    /// 地图入口配置
    /// </summary>
    public List<MapDoorDataCfg> mapDoorDataList;

    /// <summary>
    /// 获取角色位置
    /// </summary>
    /// <param name="doorType"></param>
    /// <returns></returns>
    public CSVector3 GetRoleInitPos(DoorType doorType)
    {
        foreach (var item in mapDoorDataList)
        {
            if (item.doorType== doorType)
            {
                return item.roleInitPos;
            }
        }
        return null;
    }
} 
public class MapDoorDataCfg
{   
    public DoorType doorType;//地图入口类型
    public CSVector3 roleInitPos;//入口角色初始化位置
}