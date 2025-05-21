using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZM.UGUIPro;

public class Lesson1 : MonoBehaviour
{

    async void Start()
    {
        //1.������Excel���������ɷ�ʽ
        //��������:�˵���-ZMFrame-���ɶ���������

        //2.������Excel��ȡ�����������·������ ExcelToConfig 

        //3.�����Թ���ʹ����ʾ
        //��ʼ��������ϵͳ���Զ����ر��ض�Ӧ���������ļ�
        await LocalizationManager.Instance.InitLanguageConfig();

        //4.�л�����
        //await LocalizationManager.Instance.SwitchLanguage(LanguageType.English);

        //5.������ͼƬ���ط�ʽ  ImageProBase LocalizationImageExtend

        //6.��������������

        //7.
        //Text ������ TextMeshPro
        //Text����̬���� �ᶯ̬ȥ�����豸��֧�ֵ����塣���� 
        //TextmeshPro ��̬���� ���� ȥ�������� ������� ��
        //�ܽ᣺3�����ϣ���ʦ����ʹ��TextPro  
        //     2�֣�ʹ����һ�׶��У��������ϸ�İ������ƣ�����������Ƽ�ʹ��TextPro

    }
    private async void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            await LocalizationManager.Instance.SwitchLanguage(LanguageType.English);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            await LocalizationManager.Instance.SwitchLanguage(LanguageType.Chinese);
        }
    }
}
