/*--------------------------------------------------------------------------------------
* Title: 业务逻辑脚本自动生成工具
* Author: 铸梦xy
* Date:2025/5/23 9:57:04
* Description:业务逻辑层,主要负责游戏的业务逻辑处理
* Modify:
* 注意:以下文件为自动生成，强制再次生成将会覆盖
----------------------------------------------------------------------------------------*/

using System.Threading.Tasks;
using UnityEngine;
using ZM.ZMAsset;

namespace ZMGC.Hall {
    public class HallRoleLogicCtrl : ILogicBehaviour {
        #region 属性字段

        private AssetsRequest _roleAssetRequest = null;

        private UserDataMgr _userDataMgr = null;

        #endregion


        #region public

        public async Task Init() {
            var roleData = _userDataMgr.GetCurSelectRoleData();
            if (roleData == null) {
                return;
            }
            string rolePrefabName = $"Role_{roleData.role_id}.prefab";
            _roleAssetRequest = await ZMAsset.InstantiateAsync($"{AssetsPathConfig.Hall_Role_Prefabs}{rolePrefabName}");
            GameObject goRole = _roleAssetRequest.obj;
            goRole.SetActive(true);
            goRole.transform.ReSetParent(HallWorld.GetExitsLogicCtrl<MapLogicCtrl>().CurMap.trRoleInitPos);
            goRole.name = rolePrefabName;
            goRole.transform.localScale = Vector3.one * 0.6f;
            if (goRole.GetComponent<Role_Hall>() == null) {
                goRole.AddComponent<Role_Hall>();
            }
        }

        public void OnCreate() {
            _userDataMgr = HallWorld.GetExitsDataMgr<UserDataMgr>();
        }

        public void OnDestroy() {
            ReleaseRoleAsset();
        }

        #endregion

        #region private

        private void ReleaseRoleAsset() {
            if (_roleAssetRequest != null) {
                _roleAssetRequest.Release();
                _roleAssetRequest = null;
            }
        }

        #endregion
    }
}