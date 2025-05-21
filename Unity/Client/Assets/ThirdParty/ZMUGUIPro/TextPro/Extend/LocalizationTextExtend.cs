/*----------------------------------------------------------------
* Title: ZM.UGUIPro
*
* Description: TextPro ImagePro ButtonPro TextMesh Pro
* 
* Support Function: ��������ߡ����ض������ı���ͼƬ����ť˫��ģʽ������ģʽ���ı�������ɫ���䡢˫ɫ���䡢��ɫ����
* 
* Usage: �Ҽ�-TextPro-ImagePro-ButtonPro-TextMeshPro
* 
* Author: ���� www.taikr.com/user/63798c7981862239d5b3da44d820a7171f0ce14d
*
* Date: 2023.4.13
*
* Modify: 
--------------------------------------------------------------------*/
using UnityEditor;
using UnityEngine;

namespace ZM.UGUIPro
{
    [System.Serializable]
    public class LocalizationTextExtend
    {

        [SerializeField]
        private bool m_UseLocalization;
        public bool UseLocalization
        {
            get
            {
                return m_UseLocalization;
            }
            set
            {
                m_UseLocalization = value;
            }
        }
        [SerializeField]
        private bool m_ChangeFont = true;
        public bool ChangeFont { set { m_ChangeFont = value; } get { return m_ChangeFont; } }
        [SerializeField]
        private string m_Key;
        public string Key { get { return m_Key; } set { m_Key = value; } }

        private TextPro mTextPro;

        private TextMeshPro mTextMeshPro;
        public void Initializa(TextProBase textPro)
        {
            mTextPro = (TextPro)textPro;
            LocalizationManager.Instance.AddLanguageChangeListener(UpdateText);
        }
        public void Initializa(TextMeshPro textMeshPro)
        {
            mTextMeshPro = textMeshPro;
            LocalizationManager.Instance.AddLanguageChangeListener(UpdateText);
        }

        public void InitFontListener(TextProBase textPro)
        {
            mTextPro = (TextPro)textPro;
            LocalizationManager.Instance.AddFontChangeListener(UpdateFont);
        }

        public void Release()
        {
            LocalizationManager.Instance.RemoveLanguageChangeListener(UpdateText);
        }
        public void RemoveFontListener()
        {
            LocalizationManager.Instance.RemoveFontChangeListener(UpdateFont);
        }

        public void UpdateText()
        {
            if (m_UseLocalization == false)
                return;
            if (mTextPro != null)
                mTextPro.text = LocalizationManager.Instance.GetLocalizationText(m_Key);
            else if (mTextMeshPro!=null)
                mTextMeshPro.text = LocalizationManager.Instance.GetLocalizationText(m_Key);
        }
        public void UpdateFont()
        {
            if (m_ChangeFont == false)
                return;
            if (mTextPro != null)
            {
                LocalizationManager.Instance.ChangeFont(mTextPro);
            }
        }
    }
}
