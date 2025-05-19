using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZM.ZMAsset;
using ZMGC.Battle;
using ZMGC.Hall;
using ZM.ZMAsset;
using Fantasy;
using Fantasy.Platform.Unity;
using FantasyScene = Fantasy.Scene;

public class Main : MonoBehaviour {
    #region 属性字段

    private static Main _instance = null;
    public static Main Instance => _instance;

    private FantasyScene _fScene = null;

    #endregion

    #region life-cycle

    private async Task Awake() {
        DontDestroyOnLoad(gameObject);
        await InitServer();
    }

    void Start() {
        _instance = this;
        UIModule.Instance.Initialize();
        ZMAsset.InitFrameWork();
        WorldManager.CreateWorld<HallWorld>();
    }

    #endregion

    #region private

    private async Task InitServer() {
        await Entry.Initialize();
        _fScene = await Entry.CreateScene();
    }

    #endregion
}