/*--------------------------------------------------------------------------------------
* Title: 业务逻辑脚本自动生成工具
* Author: 铸梦xy
* Date:2025/5/27 17:45:19
* Description:业务逻辑层,主要负责游戏的业务逻辑处理
* Modify:
* 注意:以下文件为自动生成，强制再次生成将会覆盖
----------------------------------------------------------------------------------------*/

using Fantasy;
using Fantasy.Async;
using UnityEngine;

namespace ZMGC.Hall {
    public class TeamLogicCtrl : ILogicBehaviour {
        private TeamMsgMgr _teamMsgMgr = null;
        private TeamDataMgr _teamDataMgr = null;
        private UserDataMgr _userDataMgr = null;

        public void OnCreate() {
            _teamMsgMgr = HallWorld.GetExitsMsgMgr<TeamMsgMgr>();
            _teamDataMgr = HallWorld.GetExitsDataMgr<TeamDataMgr>();
            _userDataMgr = HallWorld.GetExitsDataMgr<UserDataMgr>();
        }

        public async FTask<bool> JoinTeam(string strTeamId) {
            if (string.IsNullOrEmpty(strTeamId) || strTeamId.Length < 5) {
                Debug.LogError($"错误的teamid :{strTeamId}");
                return false;
            }
            if (!int.TryParse(strTeamId, out int iTeamID)) {
                Debug.LogError($"错误的teamID :{strTeamId}");
                return false;
            }
            var mapType = UserDataMgr.TeamFightMapType;
            var account_Id = _userDataMgr.account_id;
            var resp = await _teamMsgMgr.SendMsg_JoinTeam(mapType, account_Id, iTeamID);
            var success = resp.ErrorCode == ServerShareToClient.ErrorCode.Success;
            var strTip = success ? "加入队伍成功" : $"加入队伍失败{resp.ErrorCode}";
            ToastManager.ShowToast(strTip);
            if (success) {
                foreach (var roleData in resp.team_role_list) {
                    _teamDataMgr.CacheTeamRole(roleData, resp.team_id);
                }
                UIEventControl.DispensEvent(UIEventEnum.RefreshTeamList);
            }
            return success;
        }

        public async FTask<bool> CreateTeam(MapType mapType, long account_id) {
            var resp = await _teamMsgMgr.SendMsg_CreateTeam(mapType, account_id);
            _teamDataMgr.CacheTeamRole(resp.role_data, resp.team_id);
            var success = resp.ErrorCode == ServerShareToClient.ErrorCode.Success;

            ToastManager.ShowToast(success ? "创建队伍成功" : $"创建队伍失败code:{resp.ErrorCode}");
            if (success) {
                UIEventControl.DispensEvent(UIEventEnum.RefreshTeamList);
            }
            return success;
        }

        public void OnDestroy() { }
    }
}