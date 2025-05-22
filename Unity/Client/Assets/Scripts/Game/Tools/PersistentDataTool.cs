using UnityEngine;

public static class PersistentDataTool {
    public static void SaveStr(string key, string value) {
        PlayerPrefs.SetString(key, value);
    }

    public static string LoadStr(string key) {
        return PlayerPrefs.GetString(key, "");
    }
}