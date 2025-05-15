using UnityEngine;
using ZM.ZMAsset;

namespace ZMGC.Battle {
    /// <summary>
    /// 管理英雄逻辑
    /// </summary>
    public class HeroLogicCtrl : ILogicBehaviour {
        #region 属性和字段

        public HeroLogic HeroLogic { get; private set; }

        #endregion

        #region life-cycle

        public void OnCreate() { }

        public void OnDestroy() { }

        #endregion

        #region public接口

        public void OnLogicFrameUpdate() {
            HeroLogic.OnLogicFrameUpdate();
        }

        public void InitHero() {
            var heroID = HeroIDConfig.TestHeroID;
            var goHero = ZMAsset.Instantiate($"{AssetsPathConfig.Game_Hero_Prefabs}{heroID}.prefab", null);
            var heroRender = goHero.GetComponent<HeroRender>();
            HeroLogic heroLogic = new HeroLogic(heroID, heroRender);
            heroRender.SetLogicObject(heroLogic);

            //
            heroLogic.OnCreate();
            heroRender.OnCreate();
            HeroLogic = heroLogic;
            TryFollowTarget(heroRender.transform);
        }

        #endregion

        #region private

        private void TryFollowTarget(Transform followTarget) {
            const string camName = "Main Camera";
            var goCamera = GameObject.Find(camName);
            if (goCamera == null) {
                Debug.LogError($"找不到相机 {camName}");
                return;
            }
            CameraFollow cameraFollow = goCamera.GetComponent<CameraFollow>();

            if (cameraFollow == null) {
                Debug.LogError($"相机 {camName} 上没有 CameraFollow 组件");
                return;
            }
            cameraFollow.trFollowTarget = followTarget;
        }

        #endregion
    }
}