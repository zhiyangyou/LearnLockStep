using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZM.ZMAsset;

public class Main : MonoBehaviour
{
    private static Main _instance = null;
    public static Main Instance => _instance;

    private Action _onSceneLoadComplete = null;
    private Action<float> _onSceneLoadProgress = null;

    // Start is called before the first frame update
    void Start()
    {
        _instance = this;
        // init framework
        UIModule.Instance.Initialize();
        ZMAsset.InitFrameWork();

        // pop first winow
        UIModule.Instance.PopUpWindow<CreateRuleWindow>();

        // 
        DontDestroyOnLoad(gameObject);
    }

    public void LoadSceneAsync(Action onLoadComplete, Action<float> onLoadProgress)
    {
        _onSceneLoadProgress = onLoadProgress;
        _onSceneLoadComplete = onLoadComplete;
        StartCoroutine(AsyncLoadScene());
    }

    IEnumerator AsyncLoadScene()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Battle");
        asyncOperation.allowSceneActivation = false;

        float curProgress = 0f;
        float maxProgress = 100f;
        while (curProgress < 90)
        {
            curProgress = asyncOperation.progress * 100f;
            _onSceneLoadProgress?.Invoke(curProgress);
            yield return null;
        }

        while (curProgress < maxProgress)
        {
            curProgress++;
            _onSceneLoadProgress?.Invoke(curProgress);
            yield return null;
        }
        asyncOperation.allowSceneActivation = true;
        yield return null;
        _onSceneLoadComplete?.Invoke();
    }
}