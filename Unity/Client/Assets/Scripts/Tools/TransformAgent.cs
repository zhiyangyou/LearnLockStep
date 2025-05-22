using UnityEngine;

public static class TransformAgent  
{
    public static void NormalizeTr(this Transform self) {
        self.transform.localPosition = Vector3.zero;
        self.transform.localRotation = Quaternion.identity;
        self.transform.localScale = Vector3.one;
    }
    public static void ReSetParent(this Transform self,Transform parent)
    {
        self.SetParent(parent);
        self.NormalizeTr();
    }
}
