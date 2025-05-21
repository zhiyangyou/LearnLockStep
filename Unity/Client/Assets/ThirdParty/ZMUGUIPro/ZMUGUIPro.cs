using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZM.UGUIPro;

public class ZMUGUIPro : MonoBehaviour
{

    async void Start()
    {
        //ִ�пɵȴ��첽���ض��������ü���
        await LocalizationManager.Instance.InitLanguageConfig();
    }

    public async void OnChineseButtonClick()
    {
       await LocalizationManager.Instance.SwitchLanguage(LanguageType.Chinese);
    }

    public async void OnEnglishButtonClick()
    {
        await LocalizationManager.Instance.SwitchLanguage(LanguageType.English);
    }
}
