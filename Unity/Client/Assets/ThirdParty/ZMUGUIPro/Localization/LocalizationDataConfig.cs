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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Threading.Tasks;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ZM.UGUIPro
{
    [System.Serializable]
    public class LocalizationData
    {
        //[JsonConverter(typeof(StringEnumConverter))]
        //public LanguageType languageType;
        public string Key;
        public string value;
    }
    public class LocalizationDataConfig
    {
 
        /// <summary>
        /// �����������ļ�·��
        /// </summary>
        public const string OUTPUTCONFIGPATH = "ZMUGUIPro/Localization/ExcelData/";
        /// <summary>
        /// �Ƿ��첽������
        /// </summary>
        private bool IsConfigLoading = false;
        /// <summary>
        /// ���ض�Ӧ��������
        /// </summary>
        /// <param name="languageType"></param>
        /// <returns></returns>
        public async Task<List<LocalizationData>> LoadConfig(LanguageType languageType)
        {
            if (IsConfigLoading)
                return null;
            IsConfigLoading = true;

            string[] languageNames = Enum.GetNames(typeof(LanguageType));
            string name = languageNames[(int)languageType];
            string configPath = "Assets/" + OUTPUTCONFIGPATH + name + "/" + name + ".txt";
#if UNITY_EDITOR
            TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(configPath);
#else
            //TextAsset textAsset = AssetsManager.Instance.LoadTextAsset(configPath);
             TextAsset textAsset = Resources.Load<TextAsset>(configPath);
#endif


            if (textAsset != null && textAsset.text != null)
            {
                string json = textAsset.text;
                List<LocalizationData> localizationDatalist = null;
                await Task.Run(() =>
                {
                   localizationDatalist = JsonConvert.DeserializeObject<List<LocalizationData>>(json);
                });
                IsConfigLoading = false;
                return localizationDatalist;
            }
            IsConfigLoading = false;
            return null;
        }

        /// <summary>
        /// ���ض�Ӧ�������ã�ͨ��Editorģʽ
        /// </summary>
        /// <param name="languageType"></param>
        /// <returns></returns>
        public List<LocalizationData> LoadConfigEditor(LanguageType languageType)
        {
            if (IsConfigLoading)
                return null;

            IsConfigLoading = true;
            string[] languageNames = Enum.GetNames(typeof(LanguageType));
            string name = languageNames[(int)languageType];
            string configPath = "Assets/" + OUTPUTCONFIGPATH + name + "/" + name + ".txt";
#if UNITY_EDITOR
            TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(configPath);
#else
            TextAsset textAsset = ZMAsset.ZMAsset.LoadTextAsset(configPath);
#endif
            if (textAsset != null && textAsset.text != null)
            {
                string json = textAsset.text;
                List<LocalizationData> localizationDatalist = null;
                localizationDatalist = JsonConvert.DeserializeObject<List<LocalizationData>>(json);
                IsConfigLoading = false;
                return localizationDatalist;
            }
            IsConfigLoading = false;
            return null;
        }
    }
}
