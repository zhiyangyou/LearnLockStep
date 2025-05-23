/*--------------------------------------------------------------------------------------
* Title: 业务逻辑脚本自动生成工具
* Author: 铸梦xy
* Date:2025/5/23 9:33:19
* Description:业务逻辑层,主要负责游戏的业务逻辑处理
* Modify:
* 注意:以下文件为自动生成，强制再次生成将会覆盖
----------------------------------------------------------------------------------------*/

using System.Threading.Tasks;
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


        #region private

        private async Task LoadMap(string mapName) {
            mapName = mapName.EndsWith(".prefab") ? mapName : $"{mapName}.prefab";
            _homeMapAssetRequest = await ZMAsset.InstantiateAsync($"{AssetsPathConfig.Hall_Map_Prefabs}{mapName}");
            GameObject goMap = _homeMapAssetRequest.obj;
            CurMap = goMap.GetComponent<Map>();
            CurMap.Init();
        }


        private void InitMapRoleEnv() {
            
        }
        private void ReleaseMapAsset() {
            if (_homeMapAssetRequest != null) {
                _homeMapAssetRequest.Release();
                _homeMapAssetRequest = null;
            }
        }

        private void OnRelease() {
            ReleaseMapAsset();
        }

        #endregion
    }
}