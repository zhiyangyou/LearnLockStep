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

    public void LoadSceneAsync()
    {
        UIModule.Instance.PopUpWindow<LoadingWindow>();

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
            UIEventControl.DispensEvent(UIEventEnum.SceneLoadingProgress, curProgress / 100f);
            yield return null;
        }

        while (curProgress < maxProgress)
        {
            curProgress++;
            UIEventControl.DispensEvent(UIEventEnum.SceneLoadingProgress, curProgress / 100f);
            yield return null;
        }
        asyncOperation.allowSceneActivation = true;
        yield return null;
        UIEventControl.DispensEvent(UIEventEnum.SceneLoadComplete);

        WorldManager.CreateWorld<BattleWorld>();
    }
}