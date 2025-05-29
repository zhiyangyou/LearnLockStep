using System.Collections.Generic;
using Fantasy;
using ServerShareToClient;

namespace ZMGC.Battle {
    /// <summary>
    /// 负责战斗模块的数据存储
    /// </summary>
    public class BattleDataMgr : IDataBehaviour {
        public BattleStateEnum BattleState = BattleStateEnum.None;
        public long BattleID = 0;

        public List<FrameOperateData> ListFrameOpDatas { get; private set; } = null;

        public void OnCreate() {
            BattleState = BattleStateEnum.None;
            ListFrameOpDatas = new();
        }

        public void OnDestroy() {
            ListFrameOpDatas.Clear();
            ListFrameOpDatas = null;
            BattleState = BattleStateEnum.None;
        }

        public void AddFrameOpData(FrameOperateData opData) {
            if (opData == null) {
                return;
            }
            ListFrameOpDatas.Add(opData);
        }

        public void ClearFrameOpData() {
            ListFrameOpDatas.Clear();
        }
    }
}