using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZM.UGUIPro;

public class Lesson4 : MonoBehaviour
{
    public ButtonPro button;
    void Start()
    {
        //1.ButtonProʹ�ý̳�

        //2.��̬����¼�
        //��ͨ�¼����
        //button.AddButtonClick(OnButtonClick);
        //button.onClick.AddListener(OnButtonClick);

        //�����¼����
        button.AddButtonLongListener(OnLongPressButtonClick,1);

        //˫���¼����
        //button.AddButtonDoubleClickListener(OnDoubleButtonClick, 0.3f);

        //3.��ť�¼�����ԭ�����Ч���� ButtonProBase 

        //4.���һ����չ����
        //���� ��һ��������Editorȥ���ƶ�Ӧ���ֶΡ� �ڶ���������������չ�࣬ȥʵ�ֶ�Ӧ���ܵ��߼���
        //ǰ�᣺������չ�����Ҫ֧�����л�
    }

    public void OnButtonClick()
    {
        Debug.Log("��ͨ����¼�����");
    }
    public void OnLongPressButtonClick()
    {
        Debug.Log("������ť�¼�����");
    }
    public void OnDoubleButtonClick()
    {
        Debug.Log("˫����ť�¼�����");
    }
}
