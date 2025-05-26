// 分部类: 负责大厅角色的状态同步逻辑

using System.Threading.Tasks;
using Fantasy;
using ServerShareToClient;

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
            SelfRoleHall.SyncPosition(resp.role_sync_data.position, resp.role_sync_data.input_dir);
        }


        public void SyncOtherRoleState(Msg_OtherPlayerStateSync otherData) {
            var roleData = otherData.role_data;
            var mapStatus = roleData.player_map_status;
            if (mapStatus == (int)PlayerMapStatus.InMap) {
                var otherRoleHall = GetOrCreateOtherRole(roleData.player_id, roleData.role_id);
                // TODO 更新位置角度
                otherRoleHall.SyncPosition(roleData.position, roleData.input_dir);
            }
            else if (mapStatus == (int)PlayerMapStatus.OutMap) {
                ReleaseOtherRoleAsset(roleData.player_id);
            }
        }
    }
}