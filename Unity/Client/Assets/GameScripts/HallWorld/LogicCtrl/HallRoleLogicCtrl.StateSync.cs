// 分部类: 负责大厅角色的状态同步逻辑

using System.Threading.Tasks;
using Fantasy;

namespace ZMGC.Hall {
    public partial class HallRoleLogicCtrl {
        public async Task SyncRoleState(StateSyncData data, long statePackID) {
            var send = new Send_StateSync();
            send.state_pack_id = statePackID;
            send.role_sync_data = data;

            data.map_type = (int)_mapLogicCtrl.CurMap.MapType;
            data.player_id = _userDataMgr.account_id;

            var resp = await NetworkManager.Instance.SendCallMessage<Rcv_StateSync>(send);

            // 同步角色状态
            CurRoleHall.SyncPosition(resp.role_sync_data.position, resp.role_sync_data.input_dir);
        }
    }
}