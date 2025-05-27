/*--------------------------------------------------------------------------------------
* Title: 数据脚本自动生成工具
* Author: 铸梦xy
* Date:2025/5/27 18:19:20
* Description:数据层,主要负责游戏数据的存储、更新和获取
* Modify:
* 注意:以下文件为自动生成，强制再次生成将会覆盖
----------------------------------------------------------------------------------------*/

using System.Collections.Generic;
using Fantasy;

namespace ZMGC.Hall {
    public class TeamDataMgr : IDataBehaviour {
        public int TeamID { get; private set; }
        public List<RoleData> RoleDatas { get; private set; }


        public void CacheTeamRole(RoleData roleData, int team_id) {
            this.TeamID = team_id;
            this.RoleDatas.Add(roleData);
        }


        public void OnCreate() {
            RoleDatas = new();
        }

        public void OnDestroy() { }
    }
}