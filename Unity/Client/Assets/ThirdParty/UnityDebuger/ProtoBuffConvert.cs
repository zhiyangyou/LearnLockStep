/*------------------------------------------------------------------
*
* Title: ��ҵ����־ϵͳ 
*
* Description: ֧�ֱ����ļ�д�롢�Զ�����ɫ��־��FPSʵʱ��ʾ���ֻ���־����ʱ�鿴����־��������޳���ProtoBuffתJson
* 
* Author: ��Ѷ���� ����xy
*
* Date: 2023.8.13
*
* Modify: 
-------------------------------------------------------------------*/
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class ProtoBuffConvert 
{ 
    public static string ToJson<T>(T proto) {
        return (JsonConvert.SerializeObject(proto));
    }
}
