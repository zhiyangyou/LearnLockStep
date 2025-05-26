/*--------------------------------------------------------------------------------------
* Title: 业务逻辑脚本自动生成工具
* Author: 铸梦xy
* Date:2025/5/23 9:33:19
* Description:业务逻辑层,主要负责游戏的业务逻辑处理
* Modify:
* 注意:以下文件为自动生成，强制再次生成将会覆盖
----------------------------------------------------------------------------------------*/

using System.Threading.Tasks;
using Fantasy;
using UnityEngine;
using ZM.ZMAsset;

namespace ZMGC.Hall {
    public class MapLogicCtrl : ILogicBehaviour {
        #region 属性字段

        private MapMsgMgr _mapMsgMgr = null;

        private AssetsRequest _homeMapAssetRequest;

        public Map CurMap { get; private set; }

        MapType _curMapType;

        public Vector3 CurMapInitPos { get; private set; }
        
        #endregion

        #region public

        public void OnCreate() {
            _mapMsgMgr = HallWorld.GetExitsMsgMgr<MapMsgMgr>();
            _curMapType = MapType.None;
        }

        public void OnDestroy() {
            OnRelease();
        }

        public async Task Init() {
            await LoadMapAsync(MapType.Home);
        }

        #endregion

        public Vector3? GetMapEntryPos(MapType originMapType) {
            foreach (var entry in CurMap._ListAllMapEntry) {
                if ((int)entry.GotoMapType == (int)originMapType) {
                    return entry.DoorPos;
                }
            }
            return null;
        }

        public async Task LoadMapAsync(MapType gotoMapType) {
            if (CurMap != null && CurMap.MapType == gotoMapType) {
                Debug.LogError($"地图:{gotoMapType} 已经加载");
                return;
            }
            Rcv_EnterMap enterResp = await _mapMsgMgr.SendEnterMap(
                HallWorld.GetExitsDataMgr<UserDataMgr>().account_id,
                _curMapType, gotoMapType);
            CurMapInitPos = enterResp.role_init_pos.ToVector3();
            if (enterResp.ErrorCode == 0) {
                var lastMapAssetRequest = _homeMapAssetRequest;
                await _LoadMap(gotoMapType.ToString());
                ReleaseMapAsset(lastMapAssetRequest);
            }
            else {
                ToastManager.ShowToast($"进入地图失败 code:{enterResp.ErrorCode}");
            }
        }

        #region private

        private async Task _LoadMap(string mapName) {
            UIEventControl.DispensEvent(UIEventEnum.BlackScreen, BlackScreenType.Show);
            mapName = mapName.EndsWith(".prefab") ? mapName : $"{mapName}.prefab";
            var path = $"{AssetsPathConfig.Hall_Map_Prefabs}{mapName}";
            // Debug.LogError($"load path {path}");
            _homeMapAssetRequest = await ZMAsset.InstantiateAsync(path);
            GameObject goMap = _homeMapAssetRequest.obj;
            goMap.transform.SetParentToSceneRoot();
            CurMap = goMap.GetComponent<Map>();
            CurMap.Init();

            UIEventControl.DispensEvent(UIEventEnum.BlackScreen, BlackScreenType.Hide);
        }

        private void ReleaseMapAsset(AssetsRequest _curAssetRequest) {
            if (_curAssetRequest != null) {
                _curAssetRequest.Release();
                _curAssetRequest = null;
            }
        }

        private void OnRelease() {
            ReleaseMapAsset(_homeMapAssetRequest);
        }

        #endregion
    }
}