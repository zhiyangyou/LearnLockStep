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

    private FantasyScene _fScene = null;

    #endregion

    #region life-cycle

    private async Task Awake() {
        DontDestroyOnLoad(gameObject);
        await InitServer();
        Rcv_Test1 test1Rsp = await _fScene.Session.Call(new Send_Test1() {
            pass_word = "111",
            user_name = "222",
        }) as Rcv_Test1;

        Debug.LogError(test1Rsp.error_msg);
        Debug.LogError(test1Rsp.ErrorCode);
        Debug.LogError(test1Rsp.success);

        _fScene.Session.Send(new C2G_Test2() {
            frameOpCode = 1001,
            msg_content = "C2G_Test2 content msg",
        });
    }

    void Start() {
        _instance = this;
        UIModule.Instance.Initialize();
        ZMAsset.InitFrameWork();
        WorldManager.CreateWorld<HallWorld>();
    }

    #endregion

    #region server

    private const string Server_Gate_Addresss = "127.0.0.1:20000";

    private async Task InitServer() {
        await Entry.Initialize(typeof(Main).Assembly);
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

public class G2C_Test_Handler : Message<G2C_Test2> {
    protected override async FTask Run(Session session, G2C_Test2 message) {
        Debug.LogError($"G2C_Test2 : {message.frameOpCode} {message.msg_content}");
        await FTask.CompletedTask;
    }
}