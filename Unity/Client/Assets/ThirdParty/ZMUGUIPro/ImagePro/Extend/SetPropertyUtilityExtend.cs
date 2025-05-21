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
using UnityEngine;

 namespace ZM.UGUIPro {	
	internal static class SetPropertyUtilityExtend
	{
	    public static bool SetColor(ref Color currentValue, Color newValue)
	    {
	        if (currentValue.r == newValue.r && currentValue.g == newValue.g && currentValue.b == newValue.b && currentValue.a == newValue.a)
	            return false;
	
	        currentValue = newValue;
	        return true;
	    }
	
	    public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
	    {
	        if (currentValue.Equals(newValue))
	            return false;
	
	        currentValue = newValue;
	        return true;
	    }
	
	    public static bool SetClass<T>(ref T currentValue, T newValue) where T : class
	    {
	        if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue)))
	            return false;
	
	        currentValue = newValue;
	        return true;
	    }
	}
}
