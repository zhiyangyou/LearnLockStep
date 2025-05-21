using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonProDemo : MonoBehaviour
{
    public static ButtonProDemo Instance;//���Դ��룬������Ҫ��ôд
    public GameObject doubleObj;
    public GameObject longPressObj;
    public AudioSource audioSound;
    private void Awake()
    {
        Instance = this;
    }
    public void OnLongPressButtonClick()
    {
        Debug.Log("���������¼�");
        longPressObj.SetActive(true);
    }


    public void OnDoubleButtonClick()
    {
        Debug.Log("����˫���¼�");
        doubleObj.SetActive(true);
    }

    public void ResetObjState()
    {
        doubleObj.SetActive(false);
        longPressObj.SetActive(false);
    }

    public void PlaySound()
    {
        AudioClip audioClip = Resources.Load<AudioClip>("ButtonClick");
        audioSound.PlayOneShot(audioClip);
    }
}
