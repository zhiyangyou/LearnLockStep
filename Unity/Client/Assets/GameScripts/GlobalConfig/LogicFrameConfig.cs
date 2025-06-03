public class LogicFrameConfig {
    /// <summary>
    /// 逻辑帧ID
    /// </summary>
    public static long LogicFrameID = 0;

    // 测试用
    public static bool UseLocalFrameUpdate => false;
    
    /// <summary>
    /// 最大预测的帧数, 不应该是一个固定数值, 而是一个根据目标帧数的动态数值
    /// </summary>
    public static long MaxPreMoveCount = 5;
}