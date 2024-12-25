public static class AssetsPathConfig
{
    public const string GameDataBasePath = "Assets/GameData";

    public static readonly string Game = $"{GameDataBasePath}/Game";
    public static readonly string Game_Prefabs = $"{Game}/Prefabs";
    public static readonly string Game_Hero_Prefabs = $"{Game_Prefabs}/Hero";

    public static readonly string Hall = $"{GameDataBasePath}/Hall";
    public static readonly string Hall_Prefabs = $"{Hall}/Prefabs";
}