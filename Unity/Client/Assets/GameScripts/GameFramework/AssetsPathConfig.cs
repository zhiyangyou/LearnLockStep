public static class AssetsPathConfig {
    public const string GameDataBasePath = "Assets/GameData";

    public static readonly string Game = $"{GameDataBasePath}/Game";
    public static readonly string Game_Data_Path = $"{GameDataBasePath}/Data/";
    public static readonly string Game_Prefabs = $"{Game}/Prefabs";
    public static readonly string Game_Hero_Prefabs = $"{Game_Prefabs}/Hero";
    public static readonly string Game_Monster_Prefabs = $"{Game_Prefabs}/Monster";

    public static readonly string Hall = $"{GameDataBasePath}/Hall";
    public static readonly string Hall_Prefabs = $"{Hall}/Prefabs";
    public static readonly string Skill_Data_Path = $"{Game}/SkillSystem/SkillData/";
    public static readonly string Buff_Data_Path = $"{Game}/SkillSystem/BuffData";
    public static readonly string Game_Audio_Path = $"{Game}/Sound/";
}