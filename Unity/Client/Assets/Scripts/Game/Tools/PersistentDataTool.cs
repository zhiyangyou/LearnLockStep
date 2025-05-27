using UnityEngine;

public static class PersistentDataTool {
    private static string ProcessKey(string key) {
#if UNITY_EDITOR
        return $"{Application.dataPath}-{key}";
#else
        return key;
#endif
    }

    public static void SaveStr(string key, string value) {
        PlayerPrefs.SetString(ProcessKey(key), value);
    }

    public static string LoadStr(string key) {
        return PlayerPrefs.GetString(ProcessKey(key), "");
    }
}