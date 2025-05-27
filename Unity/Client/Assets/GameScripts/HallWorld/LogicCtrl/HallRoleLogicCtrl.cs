/*--------------------------------------------------------------------------------------
* Title: 业务逻辑脚本自动生成工具
* Author: 铸梦xy
* Date:2025/5/23 9:57:04
* Description:业务逻辑层,主要负责游戏的业务逻辑处理
* Modify:
* 注意:以下文件为自动生成，强制再次生成将会覆盖
----------------------------------------------------------------------------------------*/

using System.Collections.Generic;
using System.Threading.Tasks;
using Hotfix;
using TMPro;
using UnityEngine;
using ZM.ZMAsset;

namespace ZMGC.Hall {
    public partial class HallRoleLogicCtrl : ILogicBehaviour {
        #region 属性字段

        private AssetsRequest _selfRoleAssetRequest = null;

        private UserDataMgr _userDataMgr = null;

        private MapLogicCtrl _mapLogicCtrl = null;

        /// <summary>
        /// 自己的大厅角色
        /// </summary>
        private Role_Hall SelfRoleHall { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<long, Role_Hall> _otherRoleHallDic = new();

        #endregion


        #region public

        public async Task InitSelfRole(int roleID) {
            var roleData = _userDataMgr.GetCurSelectRoleData();
            if (roleData == null) {
                return;
            }
            _selfRoleAssetRequest = await ZMAsset.InstantiateAsync(GetRoleAssetPath(roleData.role_id));
            _mapLogicCtrl = HallWorld.GetExitsLogicCtrl<MapLogicCtrl>();
            InitRoleEnv(_mapLogicCtrl.CurMapInitPos, RoleSource.Self, roleID);
        }

        public void InitRoleEnv(Vector3 initPos, RoleSource roleSource, int roleID) {
            SelfRoleHall = InitRoleGameObject($"self_role_{roleID}", _selfRoleAssetRequest.obj);
            SelfRoleHall.ActiveMove(true);
            SelfRoleHall.Init(roleID, roleSource);
            SelfRoleHall.enabled = true;
            SelfRoleHall.SyncPosition(initPos.ToCSVector3(), Vector3.zero.ToCSVector3());
            SelfRoleHall.transform.position = initPos;

            var goMainCam = GameObject.Find("Main Camera");

            if (goMainCam != null) {
                var follow = goMainCam.GetComponent<CameraFollow>();
                var mapCtrl = HallWorld.GetExitsLogicCtrl<MapLogicCtrl>();
                var map = mapCtrl.CurMap;
                follow.Init(SelfRoleHall.transform, map.roleMoveMinPos, map.roleMoveMaxPos, 6f, map.cameraInitY);
            }
        }


        public void OnCreate() {
            _userDataMgr = HallWorld.GetExitsDataMgr<UserDataMgr>();
        }

        public void OnDestroy() {
            ReleaseSelfRoleAsset();
        }

        #endregion

        #region private

        /// <summary>
        /// 寻找一个玩家的Role_Hall组件, 如果不存在,那么就进行创建
        /// </summary>
        /// <returns></returns>
        private Role_Hall GetOrCreateOtherRole(long account_id, int roleTypeID) {
            Role_Hall otherRole = null;
            if (_otherRoleHallDic.TryGetValue(account_id, out otherRole)) {
            }
            else {
                GameObject goRole = ZMAsset.Instantiate(GetRoleAssetPath(roleTypeID), null);
                otherRole = InitRoleGameObject($"other_role_{roleTypeID}_{account_id}", goRole);
                _otherRoleHallDic.Add(account_id, otherRole);
                otherRole.Init(roleTypeID, RoleSource.OtherPlayer);
            }
            return otherRole;
        }


        private string GetRoleAssetPath(int roleID) {
            return $"{AssetsPathConfig.Hall_Role_Prefabs}Role_{roleID}.prefab";
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private Role_Hall InitRoleGameObject(string goName, GameObject goRole) {
            goRole.SetActive(true);
            goRole.ChangeGoLayer(LayerMask.NameToLayer("World"));
            goRole.name = goName;
            Role_Hall roleHall = goRole.GetComponent<Role_Hall>();
            if (roleHall == null) {
                roleHall = goRole.AddComponent<Role_Hall>();
            }
            roleHall.ActiveMove(true);

            goRole.transform.SetParentToSceneRoot();
            goRole.transform.localScale = Vector3.one * 0.6f;
            return roleHall;
        }

        private void ReleaseOtherRoleAsset(long account_id) {
            if (_otherRoleHallDic.TryGetValue(account_id, out var otherRole)) {
                otherRole.OnRlease();
                _otherRoleHallDic.Remove(account_id);
            }
        }


        private void ReleaseSelfRoleAsset() {
            if (_selfRoleAssetRequest != null) {
                var go = _selfRoleAssetRequest.obj;
                bool hasReleaseGo = false;
                if (go != null && go.TryGetComponent<Role_Hall>(out var item)) {
                    item.enabled = false;
                    item.OnRlease();
                    hasReleaseGo = true;
                }
                if (!hasReleaseGo) {
                    _selfRoleAssetRequest.Release();
                }
                _selfRoleAssetRequest = null;
            }
        }

        #endregion
    }
}