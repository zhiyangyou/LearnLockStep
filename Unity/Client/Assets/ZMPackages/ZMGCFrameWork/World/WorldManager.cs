using System;
using System.Collections.Generic;
using UnityEngine;

public enum WorldEnum
{
    Default,
    HallWorld,
    BattleWorld,
}

/// <summary>
/// 世界管理器
/// </summary>
public class WorldManager
{
    /// <summary>
    /// 构建状态
    /// </summary>
    public static bool Builder { get; private set; }

    /// <summary>
    /// 所有已构建出的世界列表
    /// </summary>
    private static List<World> mWorldList = new List<World>();

    /// <summary>
    /// 世界更新程序
    /// </summary>
    public static WorldUpdater WorldUpdater { get; private set; }

    /// <summary>
    /// 默认游戏世界
    /// </summary>
    public static World DefaultGameWorld { get; private set; }

    /// <summary>
    /// 当前游戏世界
    /// </summary>
    public static WorldEnum CurWorldEnum { get; private set; }


    /// <summary>
    /// 构建世界成功回调
    /// </summary>
    public static Action<WorldEnum> OnCreateWorldSuccessListener;


    /// <summary>
    /// 构建一个游戏世界
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void CreateWorld<T>(Action buildWorldComplete  = null) where T : World, new()
    {
        if (string.Equals(CurWorldEnum.ToString(), typeof(T).Name))
        {
            Debug.LogError($"重复构建游戏世界 curWorldEnum:{CurWorldEnum}，WroldName:{typeof(T).Name}");
            return;
        }
        T world = new T();
        //首个创建的世界为默认世界，按照目前的HallWorld常驻内存的设计，DefaultGameWorld一直HallWorld。
        if (DefaultGameWorld == null)
        {
            DefaultGameWorld = world;
        }
        //初始化当前游戏世界的程序集脚本
        TypeManager.InitlizateWorldAssemblies(world);
        CurWorldEnum = world.WorldEnum;
        buildWorldComplete?.Invoke();
        world.OnCreate();
        mWorldList.Add(world);
        OnCreateWorldSuccessListener?.Invoke(CurWorldEnum);

        if (!Builder)
            InitWorldUpdater();
        Builder = true;
    }


    /// <summary>
    /// 渲染帧更新,尽量少使用Update接口提升性能。但必要时，可以在对应World的Update中调用指定脚本的Update
    /// </summary>
    public static void Update()
    { 
        for (int i = 0; i < mWorldList.Count; i++)
        {
            mWorldList[i].OnUpdate();
        }
    }

    /// <summary>
    /// 初始化世界更新程序
    /// 
    /// </summary>
    public static void InitWorldUpdater()
    {
        GameObject worldObj = new GameObject("WorldUpdater");
        WorldUpdater = worldObj.AddComponent<WorldUpdater>();
        GameObject.DontDestroyOnLoad(worldObj);
    }


    /// <summary>
    /// 销毁指定游戏世界
    /// </summary>
    /// <typeparam name="T">要销毁的世界</typeparam>
    /// <param name="args">销毁后传出的参数，建议自定义class结构体，统一传出和管理</param>
    public static void DestroyWorld<T>(object args = null) where T : World
    {
        for (int i = 0; i < mWorldList.Count; i++)
        {
            World world = mWorldList[i];
            if (world.GetType().Name == typeof(T).Name)
            {
                world.DestroyWorld(typeof(T).Namespace, args);
                mWorldList.Remove(mWorldList[i]);
                CurWorldEnum = WorldEnum.Default;
                //触发销毁后处理，可在对应接口中返回其他世界
                world.OnDestroyPostProcess(args);
                break;
            }
        }
    }
}