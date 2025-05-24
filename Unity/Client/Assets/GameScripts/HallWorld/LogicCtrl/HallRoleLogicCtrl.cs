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
    public partial class HallRoleLogicCtrl : ILogicBehaviour {
        #region 属性字段

        private AssetsRequest _roleAssetRequest = null;

        private UserDataMgr _userDataMgr = null;

        private MapLogicCtrl _mapLogicCtrl = null;
        private Role_Hall CurRoleHall { get; set; }
        

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
            goRole.ChangeGoLayer(LayerMask.NameToLayer("World"));
            goRole.name = rolePrefabName;
            Role_Hall roleHall = goRole.GetComponent<Role_Hall>();
            if (roleHall == null) {
                roleHall = goRole.AddComponent<Role_Hall>();
            }
            CurRoleHall = roleHall;

            _mapLogicCtrl = HallWorld.GetExitsLogicCtrl<MapLogicCtrl>();
        }

        public void InitRoleEnv(Vector3 initPos) {
            CurRoleHall.Init();
            CurRoleHall.enabled = true;
            CurRoleHall.transform.SetParentToSceneRoot();
            CurRoleHall.transform.position = initPos;
            CurRoleHall.transform.localScale = Vector3.one * 0.6f;

            var goMainCam = GameObject.Find("Main Camera");

            if (goMainCam != null) {
                var follow = goMainCam.GetComponent<CameraFollow>();
                var mapCtrl = HallWorld.GetExitsLogicCtrl<MapLogicCtrl>();
                var map = mapCtrl.CurMap;
                follow.Init(CurRoleHall.transform, map.roleMoveMinPos, map.roleMoveMaxPos, 6f, map.cameraInitY);
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
                var go = _roleAssetRequest.obj;
                if (go != null && go.TryGetComponent<Role_Hall>(out var item)) {
                    item.enabled = false;
                }
                _roleAssetRequest.Release();
                _roleAssetRequest = null;
            }
        }

        #endregion
    }
}