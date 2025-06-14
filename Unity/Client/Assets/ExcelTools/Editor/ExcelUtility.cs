﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Excel;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System;
using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;

public class ExcelUtility
{

    /// <summary>
    /// 表格数据集合
    /// </summary>
    private DataSet mResultSet;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="excelFile">Excel file.</param>
    public ExcelUtility(string excelFile)
    {
        FileStream mStream = File.Open(excelFile, FileMode.Open, FileAccess.Read);
        IExcelDataReader mExcelReader = ExcelReaderFactory.CreateOpenXmlReader(mStream);
        mResultSet = mExcelReader.AsDataSet();
    }
    /// <summary>
    /// 转换为Byte文件
    /// </summary>
    /// <param name="bytePath"></param>
    /// <param name="encoding"></param>
    public void ConvertToDataListBinaryFile(string byteSavePath, Type type)
    {
        List<object> datalist = ConvertToList(type);
        if (!Directory.Exists(byteSavePath))
            Directory.CreateDirectory(byteSavePath);
        BinarySerialize(byteSavePath + "/" + type.Name + ".bytes", datalist);
    }
    /// <summary>
    /// 转为C#脚本
    /// </summary>
    /// <param name="dataClassName"></param>
    /// <param name="savePath"></param>
    public void ConverToCSharpFile(string dataClassName, string savePath)
    {
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);
        ConvertToCS(dataClassName, savePath);
    }
    /// <summary>
    /// 转换为Txt文件
    /// </summary>
    /// <param name="bytePath"></param>
    /// <param name="encoding"></param>
    public void ConvertToTxt(string txtPath, Encoding encoding)
    {
        string resultxt = SplitExcelTabByChar("\t");
        //写入文件
        using (FileStream fileStream = new FileStream(txtPath, FileMode.Create, FileAccess.Write))
        {
            using (TextWriter textWriter = new StreamWriter(fileStream, encoding))
            {
                textWriter.Write(resultxt);
            }
        }
        AssetDatabase.Refresh();
    }
    /// <summary>
    /// 转换为实体类列表
    /// </summary>
    public List<T> ConvertToList<T>()
    {
        //判断Excel文件中是否存在数据表
        if (mResultSet.Tables.Count < 1)
            return null;
        //默认读取第一个数据表
        DataTable mSheet = mResultSet.Tables[0];

        //判断数据表内是否存在数据
        if (mSheet.Rows.Count < 1)
            return null;

        //读取数据表行数和列数
        int rowCount = mSheet.Rows.Count;
        int colCount = mSheet.Columns.Count;

        //准备一个列表以保存全部数据
        List<T> list = new List<T>();

        //读取数据
        for (int i = 2; i < rowCount; i++)
        {
            //创建实例
            Type t = typeof(T);
            ConstructorInfo ct = t.GetConstructor(System.Type.EmptyTypes);
            T target = (T)ct.Invoke(null);
            for (int j = 0; j < colCount; j++)
            {
                //读取第1行数据作为表头字段
                string field = mSheet.Rows[1][j].ToString();
                object value = mSheet.Rows[i][j];
                //设置属性值
                SetTargetFields(target, field, value);
            }

            //添加至列表
            list.Add(target);
        }

        return list;
    }

    /// <summary>
    /// 转换为实体类列表
    /// </summary>
    public List<object> ConvertToList(Type type)
    {
        //判断Excel文件中是否存在数据表
        if (mResultSet.Tables.Count < 1)
            return null;
        //默认读取第一个数据表
        DataTable mSheet = mResultSet.Tables[0];

        //判断数据表内是否存在数据
        if (mSheet.Rows.Count < 1)
            return null;

        //读取数据表行数和列数
        int rowCount = mSheet.Rows.Count;
        int colCount = mSheet.Columns.Count;

        //准备一个列表以保存全部数据
        List<object> list = new List<object>();

        //读取数据
        for (int i = 3; i < rowCount; i++)
        {
            //创建实例
            Type t = type;

            ConstructorInfo ct = t.GetConstructor(System.Type.EmptyTypes);
            object target = ct.Invoke(null);
            for (int j = 0; j < colCount; j++)
            {
                //读取第1行数据作为表头字段
                string field = mSheet.Rows[1][j].ToString();
                object value = mSheet.Rows[i][j];
                //设置属性值
                SetTargetFields(target, field, value);
            }

            //添加至列表
            list.Add(target);
        }

        return list;
    }

    /// <summary>
    /// 转换为CS数据脚本
    /// </summary>
    /// <param name="dataClassName"></param>
    /// <param name="savePath"></param>
    public void ConvertToCS(string dataClassName, string savePath)
    {
        //判断Excel文件中是否存在数据表
        if (mResultSet.Tables.Count < 1)
            return;
        //默认读取第一个数据表
        DataTable mSheet = mResultSet.Tables[0];

        //判断数据表内是否存在数据
        if (mSheet.Rows.Count < 1)
            return;

        //读取数据表行数和列数
        int rowCount = mSheet.Rows.Count;
        int colCount = mSheet.Columns.Count;

        //准备一个列表以保存全部数据
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("[System.Serializable]");
        sb.AppendLine($"public class {dataClassName}");
        sb.AppendLine("{");
        //读取数据
        for (int i = 0; i < colCount; i++)
        {
            //读取第1行数据作为表头字段
            string fieldName = mSheet.Rows[1][i].ToString();
            string fieldType = mSheet.Rows[2][i].ToString();
            sb.AppendLine($"\tpublic {fieldType} {fieldName};");
        }
        sb.AppendLine("}");
        savePath = savePath + dataClassName + ".cs";
        File.WriteAllText(savePath, sb.ToString());
    }
    /// <summary>
    /// 转换为Json
    /// </summary>
    /// <param name="JsonPath">Json文件路径</param>
    /// <param name="Header">表头行数</param>
    public void ConvertToJson(string JsonPath, Encoding encoding)
    {
        //判断Excel文件中是否存在数据表
        if (mResultSet.Tables.Count < 1)
            return;

        //默认读取第一个数据表
        DataTable mSheet = mResultSet.Tables[0];

        //判断数据表内是否存在数据
        if (mSheet.Rows.Count < 1)
            return;

        //读取数据表行数和列数
        int rowCount = mSheet.Rows.Count;
        int colCount = mSheet.Columns.Count;

        //准备一个列表存储整个表的数据
        List<Dictionary<string, object>> table = new List<Dictionary<string, object>>();

        //读取数据
        for (int i = 2; i < rowCount; i++)
        {
            //准备一个字典存储每一行的数据
            Dictionary<string, object> row = new Dictionary<string, object>();
            for (int j = 0; j < colCount; j++)
            {
                //读取第1行数据作为表头字段
                string field = mSheet.Rows[1][j].ToString();
                //Key-Value对应
                row[field] = mSheet.Rows[i][j];
            }

            //添加到表数据中
            table.Add(row);
        }

        //生成Json字符串
        string json = JsonConvert.SerializeObject(table, Newtonsoft.Json.Formatting.Indented);
        //写入文件
        using (FileStream fileStream = new FileStream(JsonPath, FileMode.Create, FileAccess.Write))
        {
            using (TextWriter textWriter = new StreamWriter(fileStream, encoding))
            {
                textWriter.Write(json);
            }
        }
        AssetDatabase.Refresh();
    }

    /// <summary>
	/// 转换为lua
	/// </summary>
	/// <param name="luaPath">lua文件路径</param>
	public void ConvertToLua(string luaPath, Encoding encoding)
    {
        //判断Excel文件中是否存在数据表
        if (mResultSet.Tables.Count < 1)
            return;

        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("local datas = {");
        stringBuilder.Append("\r\n");

        //读取数据表
        foreach (DataTable mSheet in mResultSet.Tables)
        {
            //判断数据表内是否存在数据
            if (mSheet.Rows.Count < 1)
                continue;

            //读取数据表行数和列数
            int rowCount = mSheet.Rows.Count;
            int colCount = mSheet.Columns.Count;

            //准备一个列表存储整个表的数据
            List<Dictionary<string, object>> table = new List<Dictionary<string, object>>();

            //读取数据
            for (int i = 1; i < rowCount; i++)
            {
                //准备一个字典存储每一行的数据
                Dictionary<string, object> row = new Dictionary<string, object>();
                for (int j = 0; j < colCount; j++)
                {
                    //读取第1行数据作为表头字段
                    string field = mSheet.Rows[0][j].ToString();
                    //Key-Value对应
                    row[field] = mSheet.Rows[i][j];
                }
                //添加到表数据中
                table.Add(row);
            }
            stringBuilder.Append(string.Format("\t\"{0}\" = ", mSheet.TableName));
            stringBuilder.Append("{\r\n");
            foreach (Dictionary<string, object> dic in table)
            {
                stringBuilder.Append("\t\t{\r\n");
                foreach (string key in dic.Keys)
                {
                    if (dic[key].GetType().Name == "String")
                        stringBuilder.Append(string.Format("\t\t\t\"{0}\" = \"{1}\",\r\n", key, dic[key]));
                    else
                        stringBuilder.Append(string.Format("\t\t\t\"{0}\" = {1},\r\n", key, dic[key]));
                }
                stringBuilder.Append("\t\t},\r\n");
            }
            stringBuilder.Append("\t}\r\n");
        }

        stringBuilder.Append("}\r\n");
        stringBuilder.Append("return datas");

        //写入文件
        using (FileStream fileStream = new FileStream(luaPath, FileMode.Create, FileAccess.Write))
        {
            using (TextWriter textWriter = new StreamWriter(fileStream, encoding))
            {
                textWriter.Write(stringBuilder.ToString());
            }
        }
        AssetDatabase.Refresh();
    }
    /// <summary>
    /// 转换为CSV
    /// </summary>
    public void ConvertToCSV(string CSVPath, Encoding encoding)
    {
        string resultxt = SplitExcelTabByChar(",");
        if (string.Equals(resultxt, ""))
            return;
        //写入文件
        using (FileStream fileStream = new FileStream(CSVPath, FileMode.Create, FileAccess.Write))
        {
            using (TextWriter textWriter = new StreamWriter(fileStream, encoding))
            {
                textWriter.Write(resultxt);
            }
        }
        AssetDatabase.Refresh();
    }
    /// <summary>
    /// 导出为Xml
    /// </summary>
    public void ConvertToXml(string XmlFile)
    {
        //判断Excel文件中是否存在数据表
        if (mResultSet.Tables.Count < 1)
            return;

        //默认读取第一个数据表
        DataTable mSheet = mResultSet.Tables[0];

        //判断数据表内是否存在数据
        if (mSheet.Rows.Count < 1)
            return;

        //读取数据表行数和列数
        int rowCount = mSheet.Rows.Count;
        int colCount = mSheet.Columns.Count;

        //创建一个StringBuilder存储数据
        StringBuilder stringBuilder = new StringBuilder();
        //创建Xml文件头
        stringBuilder.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        stringBuilder.Append("\r\n");
        //创建根节点
        stringBuilder.Append("<Table>");
        stringBuilder.Append("\r\n");
        //读取数据
        for (int i = 1; i < rowCount; i++)
        {
            //创建子节点
            stringBuilder.Append("  <Row>");
            stringBuilder.Append("\r\n");
            for (int j = 0; j < colCount; j++)
            {
                stringBuilder.Append("   <" + mSheet.Rows[0][j].ToString() + ">");
                stringBuilder.Append(mSheet.Rows[i][j].ToString());
                stringBuilder.Append("</" + mSheet.Rows[0][j].ToString() + ">");
                stringBuilder.Append("\r\n");
            }
            //使用换行符分割每一行
            stringBuilder.Append("  </Row>");
            stringBuilder.Append("\r\n");
        }
        //闭合标签
        stringBuilder.Append("</Table>");
        //写入文件
        using (FileStream fileStream = new FileStream(XmlFile, FileMode.Create, FileAccess.Write))
        {
            using (TextWriter textWriter = new StreamWriter(fileStream, Encoding.GetEncoding("utf-8")))
            {
                textWriter.Write(stringBuilder.ToString());
            }
        }
        AssetDatabase.Refresh();
    }
    /// <summary>
    /// 通过字符串拆分Excel表
    /// </summary>
    public string SplitExcelTabByChar(string split = ",")
    {
        //判断Excel文件中是否存在数据表
        if (mResultSet.Tables.Count < 1)
            return "";

        //默认读取第一个数据表
        DataTable mSheet = mResultSet.Tables[0];

        //判断数据表内是否存在数据
        if (mSheet.Rows.Count < 1)
            return "";

        //读取数据表行数和列数
        int rowCount = mSheet.Rows.Count;
        int colCount = mSheet.Columns.Count;

        //创建一个StringBuilder存储数据
        StringBuilder stringBuilder = new StringBuilder();

        //读取数据
        for (int i = 1; i < rowCount; i++)
        {
            for (int j = 0; j < colCount; j++)
            {
                //使用","分割每一个数值
                if (j == colCount - 1)
                    stringBuilder.Append(mSheet.Rows[i][j]);
                else
                    stringBuilder.Append(mSheet.Rows[i][j] + split);

            }
            //使用换行符分割每一行
            stringBuilder.Append("\r\n");
        }
        return stringBuilder.ToString();
    }
    /// <summary>
    /// 设置目标实例的属性
    /// </summary>
    private void SetTargetProperty(object target, string propertyName, object propertyValue)
    {
        //获取类型
        Type mType = target.GetType();
        //获取属性集合
        PropertyInfo[] mPropertys = mType.GetProperties();
        foreach (PropertyInfo property in mPropertys)
        {
            if (property.Name == propertyName)
            {
                property.SetValue(target, Convert.ChangeType(propertyValue, property.PropertyType), null);
            }
        }
    }

    /// <summary>
    /// 设置目标实例的属性
    /// </summary>
    private void SetTargetFields(object target, string propertyName, object propertyValue)
    {
        //获取类型
        Type mType = target.GetType();
        //获取属性集合
        FieldInfo[] mFieldInfos = mType.GetFields();
        for (int i = 0; i < mFieldInfos.Length; i++)
        {
            if (mFieldInfos[i].Name == propertyName)
            {

                if (mFieldInfos[i].FieldType.IsArray)
                {
                    int[] values = SplieStringToArray<int>(propertyValue.ToString(), ',');
                    mFieldInfos[i].SetValue(target, Convert.ChangeType(values, mFieldInfos[i].FieldType));

                }
                else
                {
                    mFieldInfos[i].SetValue(target, Convert.ChangeType(propertyValue, mFieldInfos[i].FieldType));
                }
            }
        }
    }

    /// <summary>
    /// 数值类型转换  //把字符串拆成数组
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="str"></param>
    /// <param name="splitechar"></param>
    /// data.rewardNumbers = ToolsUtil.SplieStringToArray<int>(colums[5]) //传什么转什么
    public static T[] SplieStringToArray<T>(string str, char splitechar = ',')
    {
        //拆分  T代表泛型
        string[] strArray = str.Split(splitechar);
        T[] tArray = new T[strArray.Length];
        for (int i = 0; i < strArray.Length; i++)
        {
            //类型转换
            tArray[i] = (T)System.Convert.ChangeType(strArray[i], typeof(T));
        }
        return tArray;
    }
    /// <summary>
    /// 序列化为二进制
    /// </summary>
    /// <param name="path"></param>
    /// <param name="obj"></param>
    public static void BinarySerialize(string path,object obj)
    {
        try
        {
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            fs.Seek(0, SeekOrigin.Begin);
            fs.SetLength(0);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, obj);
            fs.Close();
            fs.Dispose();
        }
        catch (Exception e)
        {
            Debug.LogError("BinarySerilize Fiale! Error:" + e.ToString());
        }
        
    }


}
