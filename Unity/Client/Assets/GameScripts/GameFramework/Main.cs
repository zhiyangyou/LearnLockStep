using System;
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

    private void Awake() {
        Debug.LogError($"DontDestroyOnLoad({gameObject.name})");
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    { 
        _instance = this;
        UIModule.Instance.Initialize();
        ZMAsset.InitFrameWork();
        WorldManager.CreateWorld<HallWorld>();
    }

    private void Update() {
       
    }
}