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

        public static MapType TeamFightMapType = MapType.Dungeons; 
        
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

        /// <summary>
        /// 创建角色的时候, 当前选择的角色ID
        /// </summary>
        public int CurSelectRoleID {
            get {
                if (CurSelectRoleIndex >= 0 && CurSelectRoleIndex < RoleDatas.Count) {
                    return RoleDatas[CurSelectRoleIndex].role_id;
                }
                return -1;
            }
        }

        public int CurSelectRoleIndex = 0;

        #region public

        public bool HasSelectAnyRole() {
            return GetCurSelectRoleData() != null;
        }

        public RoleData GetCurSelectRoleData() {
            if (CurSelectRoleIndex < 0 || CurSelectRoleIndex >= RoleDatas.Count) {
                Debug.LogError("当前没有选择任何角色");
                return null;
            }
            return RoleDatas[CurSelectRoleIndex];
        }

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