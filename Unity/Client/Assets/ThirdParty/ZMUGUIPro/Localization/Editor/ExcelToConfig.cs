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
using Excel;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using ZM.UGUIPro;

public class ExcelToConfig
{
    /// <summary>
    /// �������� ����ʵ�ʿ����п���ĳ�л���ڲ������������
    /// </summary>
    private static List<string> mParseKeyList = new List<string>() { "Chinese", "English","Thai" };
    /// <summary>
    /// Excel�ļ���ȡ·��
    /// </summary>
    public static string ExcelLaodPath= "Assets/ZMUGUIPro/Localization/ExcelData/";
    /// <summary>
    /// �ļ����·��
    /// </summary>
    public static string OutPutPath = "ZMUGUIPro/Localization/ExcelData/";
    [MenuItem("ZMFrame/����Excel����������")]
    static void GeneratorLoaclization()
    {
        DataSet resultSet = LaodExcel(ExcelLaodPath+"�����ĵ�.xlsx");
        GenerateLocalizationCfg(resultSet);
    }

    /// <summary>
    /// ���캯��
    /// </summary>
    /// <param name="excelFile">Excel file.</param> 
    public static DataSet LaodExcel(string excelFile)
    {
        FileStream mStream = File.Open(excelFile, FileMode.Open, FileAccess.Read);
        IExcelDataReader mExcelReader = ExcelReaderFactory.CreateOpenXmlReader(mStream);
        return mExcelReader.AsDataSet();
    }
    /// <summary>
    /// ���ɶ���������
    /// </summary>
    public static void GenerateLocalizationCfg(DataSet resultSet)
    {

        //�ж�Excel�ļ����Ƿ�������ݱ�
        if (resultSet.Tables.Count < 1)
            return;
        //Ĭ�϶�ȡ��һ�����ݱ�
        DataTable mSheet = resultSet.Tables[0];
        //�ж����ݱ����Ƿ��������
        if (mSheet.Rows.Count < 1)
            return;

        //��ȡ���ݱ�����������
        int rowCount = mSheet.Rows.Count;
        int colCount = mSheet.Columns.Count;

        //׼��һ���б��Ա���ȫ������
        Dictionary<string, List<LocalizationData>> localizationDataListDic = new Dictionary<string, List<LocalizationData>>();
        for (int i = 1; i < colCount; i++)
        {
            List<LocalizationData> dataList = new List<LocalizationData>();

            string cloName = mSheet.Rows[0][i].ToString();
            int startIndex = cloName.LastIndexOf(@"��");
            if (startIndex > 0)
                cloName = cloName.Substring(0, startIndex - 1);

            if (cloName != "" && mParseKeyList.Contains(cloName))
            {
                localizationDataListDic.Add(cloName, dataList);
            }
        }

        //��ȡ����
        for (int i = 1; i < rowCount; i++)
        {
            for (int j = 1; j < colCount; j++)
            {
                //����ʵ��
                LocalizationData data = new LocalizationData();
              
                data.Key = mSheet.Rows[i][colCount - 1].ToString();
                data.value = mSheet.Rows[i][j].ToString();
                Debug.Log("key:" + data.Key);
                if (!string.IsNullOrEmpty(data.value) && mSheet.Rows[0][j].ToString() != "" && !string.IsNullOrEmpty(data.Key))
                {
                    string cloName = mSheet.Rows[0][j].ToString();
                    int startIndex = cloName.LastIndexOf(@"��");
                    if (startIndex > 0)
                        cloName = cloName.Substring(0, startIndex - 1);
                    if (localizationDataListDic.ContainsKey(cloName))
                        localizationDataListDic[cloName].Add(data);  //������б�

                }
            }
        }
        foreach (var item in localizationDataListDic)
        {
            //����Json�ַ���
            string json = JsonConvert.SerializeObject(item.Value, Newtonsoft.Json.Formatting.Indented);
            int startIndex = item.Key.LastIndexOf(@"��");
            string itemKey = item.Key;
            if (startIndex > 0)
            {
                itemKey = item.Key.Substring(0, startIndex - 1);
            }
            //�������������ļ�·��
            var savePath = OutPutPath + itemKey + "/";
            if (!Directory.Exists(Application.dataPath + "/" + savePath))
                Directory.CreateDirectory(Application.dataPath + "/" + savePath);
            json = json.Replace(@"\\n", @"\n");
            //д���ļ�
            using (FileStream fileStream = new FileStream(Application.dataPath + "/" + savePath + itemKey + "" + ".txt", FileMode.Create, FileAccess.Write))
            {
                using (TextWriter textWriter = new StreamWriter(fileStream, System.Text.Encoding.UTF8))
                {
                    textWriter.Write(json);
                }
            }
            Debug.Log("д����� Path��" + Application.dataPath + "/" + savePath + itemKey + ".txt");
        }
        AssetDatabase.Refresh();
    }
}
