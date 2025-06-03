namespace ServerShareToClient {
    /// <summary>
    /// 和客户端保持同步的一些配置
    /// </summary>
    public static class GameConstConfig { 

        /// <summary>
        /// FixedUpdate每隔这个数值, 向后端同步一下这个状态
        /// </summary>
        public static int MaxSyncStateCount => 5;
    


        /// <summary>
        /// 逻辑帧间隔
        /// </summary>
        public static float LogicFrameInterval = 0.066f;

        /// <summary>
        /// 逻辑帧间隔 （毫秒）
        /// </summary>
        public static int LogicFrameIntervalMS = 66;
    }
}