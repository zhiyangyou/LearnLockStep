using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public static class PrefabLayerModifier
{
    /// <summary>
    /// 递归修改指定文件夹下所有Prefab的Layer
    /// </summary>
    /// <param name="folderPath">Assets下的相对路径（如："Assets/MyPrefabs"）</param>
    /// <param name="targetLayerName">目标Layer名称</param>
    public static void ModifyPrefabLayers(string folderPath, string targetLayerName)
    {
        // 验证Layer是否存在
        int targetLayer = LayerMask.NameToLayer(targetLayerName);
        if (targetLayer == -1)
        {
            Debug.LogError($"Layer '{targetLayerName}' does not exist! Please create it first.");
            return;
        }

        // 获取所有Prefab的路径
        List<string> prefabPaths = new List<string>();
        CollectPrefabsRecursively(folderPath, prefabPaths);

        int modifiedCount = 0;

        // 遍历所有Prefab
        foreach (string prefabPath in prefabPaths)
        {
            // 加载Prefab
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefab == null) continue;

            bool modified = false;

            // 修改Prefab根物体的Layer
            if (prefab.layer != targetLayer)
            {
                prefab.layer = targetLayer;
                modified = true;
            }

            // 递归修改所有子物体的Layer
            Transform[] children = prefab.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in children)
            {
                if (child.gameObject.layer != targetLayer)
                {
                    child.gameObject.layer = targetLayer;
                    modified = true;
                }
            }

            if (modified)
            {
                // 保存修改
                EditorUtility.SetDirty(prefab);
                modifiedCount++;
                Debug.Log($"Modified: {prefabPath}", prefab);
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"Complete! Modified {modifiedCount}/{prefabPaths.Count} prefabs in '{folderPath}'");
    }

    // 递归收集所有Prefab路径
    private static void CollectPrefabsRecursively(string folderPath, List<string> results)
    {
        if (!folderPath.StartsWith("Assets/"))
        {
            folderPath = "Assets/" + folderPath;
        }

        // 获取所有.prefab文件
        string[] files = Directory.GetFiles(folderPath, "*.prefab", SearchOption.AllDirectories);
        foreach (string file in files)
        {
            string assetPath = file.Replace("\\", "/");
            results.Add(assetPath);
        }

        Debug.Log($"Found {files.Length} prefabs in '{folderPath}'");
    }

    #if UNITY_EDITOR
    // 添加菜单项方便测试
    [MenuItem("Tools/Modify Prefab Layers")]
    private static void ModifyPrefabLayersMenu()
    {
        string folderPath = EditorUtility.OpenFolderPanel("Select Prefab Folder", "Assets", "");
        if (string.IsNullOrEmpty(folderPath)) return;

        // 转换为相对路径
        folderPath = "Assets" + folderPath.Substring(Application.dataPath.Length);

        string layerName = EditorInputDialog.Show("Input Layer Name", "Enter target layer name:", "Default");
        if (!string.IsNullOrEmpty(layerName))
        {
            ModifyPrefabLayers(folderPath, layerName);
        }
    }
    #endif
}

// 辅助类：简单的输入对话框
public class EditorInputDialog : EditorWindow
{
    private string inputText;
    private System.Action<string> onOk;

    public static string Show(string title, string message, string defaultValue)
    {
        string ret = defaultValue;
        var window = CreateInstance<EditorInputDialog>();
        window.titleContent = new GUIContent(title);
        window.inputText = defaultValue;
        
        window.ShowModalUtility();

        // 这里简化处理，实际应该用回调
        // 完整实现需要更复杂的逻辑
        return window.inputText;
    }

    void OnGUI()
    {
        GUILayout.Label("Enter layer name:");
        inputText = EditorGUILayout.TextField(inputText);

        if (GUILayout.Button("OK"))
        {
            Close();
        }
    }
}