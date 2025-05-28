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
using UnityEngine;

namespace ZMGC.Hall {
    public class TeamDataMgr : IDataBehaviour {
        public int TeamID { get; private set; }
        public List<RoleData> RoleDatas { get; private set; }


        public void ClearTeam() {
            TeamID = -1;
            RoleDatas.Clear();
        }

        public void RemoveTeamRole(RoleData roleData) {
            if (roleData == null) {
                return;
            }
            else {
                if (RoleDatas != null) {
                    for (int i = RoleDatas.Count - 1; i >= 0; i--) {
                        if (roleData.role_id == RoleDatas[i].role_id) {
                            RoleDatas.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
        }

        public void AddTeamRole(RoleData roleData) {
            if (roleData != null) {
                bool hasRepeate = false;
                foreach (var data in this.RoleDatas) {
                    if (data.role_id == roleData.role_id) {
                        hasRepeate = true;
                        break;
                    }
                }
                if (!hasRepeate) {
                    RoleDatas.Add(roleData);
                }
                else {
                    Debug.LogError("添加缓存队友数据失败, 存在同名玩家");
                }
            }
        }

        public void CacheTeamRole(RoleData roleData, int team_id) {
            if (team_id > 0) {
                this.TeamID = team_id;
                this.RoleDatas.Add(roleData);
            }
        }


        public void OnCreate() {
            RoleDatas = new();
        }

        public void OnDestroy() { }
    }
}