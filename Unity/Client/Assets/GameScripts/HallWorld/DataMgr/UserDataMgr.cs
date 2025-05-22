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

        private List<int> _listRoleIDs = new() {
            HeroIDConfig.HeroID_神枪手,
            HeroIDConfig.HeroID_鬼剑士,
        };

        public IEnumerable<int> RoleIDs => _listRoleIDs;

        #endregion

        public string UserName;
        
        public int CurSelectRoleID { get; set; }

        #region public

        public void InitByLoginData(Rcv_LoginGate loginData) {
            this.level = loginData.level;
            this.diamonds = loginData.diamond;
            this.gold = loginData.gold;
        }

        public void OnCreate() { }

        public void OnDestroy() { }

        #endregion
    }
}