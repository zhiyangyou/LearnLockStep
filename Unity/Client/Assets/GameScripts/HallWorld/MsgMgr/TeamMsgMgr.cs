/*--------------------------------------------------------------------------------------
* Title: 网络消息层脚本自动生成工具
* Author: 铸梦xy
* Date:2025/5/27 18:19:48
* Description:网络消息层,主要负责游戏网络消息的收发
* Modify:
* 注意:以下文件为自动生成，强制再次生成将会覆盖
----------------------------------------------------------------------------------------*/

using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;

namespace ZMGC.Hall {


    public class TeamMsgMgr : IMsgBehaviour {
        public void OnCreate() { }

        public void OnDestroy() { }


        #region public

        public async FTask<Rcv_CreateTeam> SendMsg_CreateTeam(MapType mapType, long account_id) {
            Send_CreateTeam msg = new();
            msg.account_id = account_id;
            msg.map_type = (int)mapType;
            var resp = await NetworkManager.Instance.SendCallMessage<Rcv_CreateTeam>(msg);
            return resp;
        }

        public async FTask<Rcv_JoinTeam> SendMsg_JoinTeam(MapType mapType, long account_id, int team_id) {
            Send_JoinTeam msg = new();
            msg.team_id = team_id;
            msg.account_id = account_id;
            msg.map_type = (int)mapType;
            var resp = await NetworkManager.Instance.SendCallMessage<Rcv_JoinTeam>(msg);
            return resp;
        }

        #endregion
    }
}