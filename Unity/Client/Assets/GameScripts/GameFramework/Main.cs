using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using ZM.ZMAsset;
using ZMGC.Hall;
using Fantasy;
using Fantasy.Network;


public class Main : MonoBehaviour {
    #region 属性字段

    private static Main _instance = null;
    public static Main Instance => _instance;

    #endregion

    #region life-cycle

    private async Task Awake() {
        InitScreenSettings();
        DontDestroyOnLoad(gameObject);
        InitLoadingStateCallback();
        await InitNetworkManager();
        InitUnityDebugger();
    }

    void Start() {
        _instance = this;
        ZMAsset.InitFrameWork();

        InitAssetBundle();
    }

    #endregion

    #region private

    private void InitAssetBundle() {
        HotUpdateManager.Instance.HotAndUnPackAssets(BundleModuleEnum.Game, OnUnPackAssetComplete);
    }

    private void OnUnPackAssetComplete() {
        UIModule.Instance.Initialize();
        WorldManager.CreateWorld<HallWorld>();
    }

    private void InitScreenSettings() {
        Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
    }

    private void InitUnityDebugger() {
        Debuger.InitLog(new LogConfig() { });
    }

    private async Task InitNetworkManager() {
        await NetworkManager.Instance.Initlization();
    }

    private void InitLoadingStateCallback() {
        HotUpdateManager.Instance.OnLoadingState += what => {
            if (what == LoadWhat.Config) {
                ConfigCenter.Instance.InitGameCfg();
            }
        };
    }

    #endregion
}