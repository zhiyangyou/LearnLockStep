/*--------------------------------------------------------------------------------------
* Title: 数据脚本自动生成工具
* Author: 铸梦xy
* Date:2024/12/25 20:49:27
* Description:数据层,主要负责游戏数据的存储、更新和获取
* Modify:
* 注意:以下文件为自动生成，强制再次生成将会覆盖
----------------------------------------------------------------------------------------*/

using System.Collections.Generic;
using Fantasy;
using UnityEngine;

namespace ZMGC.Hall {
    public class UserDataMgr : IDataBehaviour {
        #region 属性和字段

        public long account_id;

        public long gold;
        public long diamonds;
        public long level;

        public List<RoleData> RoleDatas = new();

        private List<int> _listRoleIDs = new() {
            HeroIDConfig.HeroID_神枪手,
            HeroIDConfig.HeroID_鬼剑士,
        };

        public IEnumerable<int> RoleIDs => _listRoleIDs;

        #endregion

        public string UserName;

        public int CurSelectRoleID { get; set; }

        public int CurSelectRoleIndex = 0;
        
        #region public

        public void InitByLoginData(Rcv_LoginGate loginData) {
            this.level = loginData.level;
            this.diamonds = loginData.diamond;
            this.gold = loginData.gold;
            this.RoleDatas.Clear();
            this.RoleDatas.AddRange(loginData.role_datas);
        }

        public void AddRoleData(RoleData roleData) {
            var hasExist = this.RoleDatas.Exists(data => data.role_name == roleData.role_name);
            if (hasExist) {
                Debug.LogError($"本地数据添加了重复角色名的roleData {roleData.role_name}");
                return;
            }
            else {
                this.RoleDatas.Add(roleData);
            }
        }

        public void OnCreate() { }

        public void OnDestroy() { }

        #endregion
    }
}