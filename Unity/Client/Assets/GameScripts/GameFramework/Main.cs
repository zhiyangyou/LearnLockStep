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
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using Fantasy.Platform.Unity;
using FantasyScene = Fantasy.Scene;

public class Main : MonoBehaviour {
    #region 属性字段

    private static Main _instance = null;
    public static Main Instance => _instance;

    #endregion

    #region life-cycle

    private async Task Awake() {
        DontDestroyOnLoad(gameObject);
        await InitNetworkManager();
        InitUnityDebugger();
    }

    void Start() {
        _instance = this;
        UIModule.Instance.Initialize();
        ZMAsset.InitFrameWork();
        WorldManager.CreateWorld<HallWorld>();
    }

    #endregion

    #region private

    private void InitUnityDebugger() {
        Debuger.InitLog(new LogConfig() { });
        Debuger.LogGreen("InitUnityDebugger 初始化完成");
    }

    private async Task InitNetworkManager() {
        await NetworkManager.Instance.Initlization();
        NetworkManager.Instance.Connect("127.0.0.1:20000", NetworkProtocolType.TCP, async () => {
            Rcv_Test1 test1Rsp = (Rcv_Test1)await NetworkManager.Instance.SendCallMessage(new Send_Test1() {
                pass_word = "111",
                user_name = "222",
            });

            Debuger.LogGreen(test1Rsp.error_msg);
            Debuger.LogGreen(test1Rsp.ErrorCode);
            Debuger.LogGreen(test1Rsp.success);

            NetworkManager.Instance.Send(new C2G_Test2() {
                frameOpCode = 1001,
                msg_content = "C2G_Test2 content msg",
            });
        });
    }

    #endregion
}