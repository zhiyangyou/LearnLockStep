using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZM.ZMAsset;
using ZMGC.Battle;
using ZMGC.Hall;
using ZM.ZMAsset;

public class Main : MonoBehaviour
{
    private static Main _instance = null;

    public static Main Instance => _instance;

    // Start is called before the first frame update
    void Start()
    {
        _instance = this;
        // init framework
        UIModule.Instance.Initialize();
        ZMAsset.InitFrameWork();
        WorldManager.CreateWorld<HallWorld>();

        // 
        DontDestroyOnLoad(gameObject);
    }


}