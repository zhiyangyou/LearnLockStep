using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZM.ZMAsset;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIModule.Instance.Initialize();
        ZMAsset.InitFrameWork();

        UIModule.Instance.PopUpWindow<CreateRuleWindow>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
