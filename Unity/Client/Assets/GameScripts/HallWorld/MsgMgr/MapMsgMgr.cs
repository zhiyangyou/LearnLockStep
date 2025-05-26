/*--------------------------------------------------------------------------------------
* Title: 网络消息层脚本自动生成工具
* Author: 铸梦xy
* Date:2025/5/26 11:14:42
* Description:网络消息层,主要负责游戏网络消息的收发
* Modify:
* 注意:以下文件为自动生成，强制再次生成将会覆盖
----------------------------------------------------------------------------------------*/

using Fantasy;
using Fantasy.Async;

namespace ZMGC.Hall {
    public class MapMsgMgr : IMsgBehaviour {
        public void OnCreate() { }

        public void OnDestroy() { }

        public async FTask<Rcv_EnterMap> SendEnterMap(long account_id, MapType curMapType, MapType gotoMapType) {
            Send_EnterMap send = new Send_EnterMap() {
                map_type = (int)gotoMapType,
                player_id = account_id,
                cur_map = (int)curMapType,
            };
            return await NetworkManager.Instance.SendCallMessage<Rcv_EnterMap>(send);
        }
    }
}