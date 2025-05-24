/*--------------------------------------------------------------------------------------
* Title: 业务逻辑脚本自动生成工具
* Author: 铸梦xy
* Date:2025/5/23 9:33:19
* Description:业务逻辑层,主要负责游戏的业务逻辑处理
* Modify:
* 注意:以下文件为自动生成，强制再次生成将会覆盖
----------------------------------------------------------------------------------------*/

using System.Threading.Tasks;
using Sirenix.OdinInspector.Editor;
using UnityEngine;
using ZM.ZMAsset;

namespace ZMGC.Hall {
    public class MapLogicCtrl : ILogicBehaviour {
        #region 属性字段

        private AssetsRequest _homeMapAssetRequest;

        public Map CurMap { get; private set; }

        #endregion

        #region public

        public void OnCreate() { }

        public void OnDestroy() {
            OnRelease();
        }

        public async Task Init() {
            await LoadMap("Home");
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

        public async Task LoadMapAsync(MapType mapType, DoorType doorType) {
            if (CurMap.MapType == mapType) {
                Debug.LogError($"地图:{mapType} 已经加载");
                return;
            }
            var lastMapAssetRequest = _homeMapAssetRequest;
            await LoadMap(mapType.ToString());
            ReleaseMapAsset(lastMapAssetRequest);
        }

        #region private

        private async Task LoadMap(string mapName) {
            mapName = mapName.EndsWith(".prefab") ? mapName : $"{mapName}.prefab";
            var path = $"{AssetsPathConfig.Hall_Map_Prefabs}{mapName}";
            // Debug.LogError($"load path {path}");
            _homeMapAssetRequest = await ZMAsset.InstantiateAsync(path);
            GameObject goMap = _homeMapAssetRequest.obj;
            goMap.transform.SetParentToSceneRoot();
            CurMap = goMap.GetComponent<Map>();
            CurMap.Init();
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