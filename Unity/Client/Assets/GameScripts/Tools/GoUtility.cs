using UnityEngine;

public static class GameObjectEx {
    public static void ChangeGoLayer(this GameObject go, int targetLayer) {
        if (go == null) return;
        // 深度遍历核心逻辑
        SetLayerRecursively(go.transform, targetLayer);
    }

    private static void SetLayerRecursively(Transform parent, int layer) {
        parent.gameObject.layer = layer; // 修改当前物体层级
        foreach (Transform child in parent) // 遍历所有子物体
        {
            SetLayerRecursively(child, layer); // 递归调用
        }
    }
}