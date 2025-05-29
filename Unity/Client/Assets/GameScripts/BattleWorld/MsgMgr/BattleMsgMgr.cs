using System.Collections.Generic;
using Fantasy;
using UnityEngine.UIElements;

namespace ZMGC.Battle {
    /// <summary>
    /// 战斗消息层
    /// </summary>
    public class BattleMsgMgr : IMsgBehaviour {
        public void OnCreate() { }

        public void OnDestroy() { }

        public void SendMsg_FrameOpEvent(long battleID, List<FrameOperateData> opDatas) {
            var msg = new Msg_C2S_FrameOpEvent();
            msg.battle_id = battleID;
            msg.frame_operate_datas = opDatas;
            NetworkManager.Instance.Send(msg);
        }
    }
}