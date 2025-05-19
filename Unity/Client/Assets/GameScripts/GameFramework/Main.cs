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
using Fantasy.Network;
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

    #region server

    private const string Server_Gate_Addresss = "127.0.0.1:11002";

    private async Task InitServer() {
        await Entry.Initialize();
        _fScene = await Entry.CreateScene();
        _fScene.Connect(Server_Gate_Addresss,
            NetworkProtocolType.TCP,
            OnServerGate_ConnectComplete,
            OnServerGate_ConnectFailed,
            OnServer_Gate_Disconnect,
            false, 5000);
    }

    private void OnServer_Gate_Disconnect() {
        Debug.LogError($"{Server_Gate_Addresss} Disconnect");
    }

    private void OnServerGate_ConnectFailed() {
        Debug.LogError($"{Server_Gate_Addresss} ConnectFailed");
    }

    private void OnServerGate_ConnectComplete() {
        Debug.LogError($"{Server_Gate_Addresss} ConnectComplete");
    }

    #endregion
}