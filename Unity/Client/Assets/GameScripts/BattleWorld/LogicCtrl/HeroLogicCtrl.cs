using System.Collections.Generic;
using UnityEngine;
using ZM.ZMAsset;
using ZMGC.Hall;

namespace ZMGC.Battle {
    /// <summary>
    /// 管理英雄逻辑
    /// </summary>
    public class HeroLogicCtrl : ILogicBehaviour {
        #region 属性和字段

        // public HeroLogic HeroLogic { get; private set; }
        private HeroDataMgr _heroDataMgr = null;
        private UserDataMgr _userDataMgr = null;

        /// <summary>
        /// key: account_ID
        /// </summary>
        public Dictionary<long, HeroLogic> ListHeroLogics { get; private set; } = null; // 所有玩家

        public HeroLogic LocalHeroLogic { get; private set; } = null; // 本地玩家
        public HeroLogic ChaseHeroLogic { get; private set; } = null; // 被怪物跟踪的玩家

        #endregion

        #region life-cycle

        public void OnCreate() {
            _heroDataMgr = BattleWorld.GetExitsDataMgr<HeroDataMgr>();
            _userDataMgr = BattleWorld.GetExitsDataMgr<UserDataMgr>();
            ListHeroLogics = new();
        }

        public void OnDestroy() {
            ListHeroLogics.Clear();
            ListHeroLogics = null;
            LocalHeroLogic = null;
            ChaseHeroLogic = null;
        }

        #endregion

        #region public接口

        public void OnLogicFrameUpdate() {
            foreach (var kv in ListHeroLogics) {
                var singleHero = kv.Value;
                singleHero.OnLogicFrameUpdate();
            }
        }

        public void InitHero() {
            foreach (var roleData in _heroDataMgr.battleRoleDatas) {
                var heroID = roleData.role_id;
                var accountID = roleData.account_id;
                bool isSelfPlayer = _userDataMgr.account_id == accountID;
                bool isTeamLeader = roleData.account_id == _heroDataMgr.TeamLeader.account_id;
                var goHero = ZMAsset.Instantiate($"{AssetsPathConfig.Game_Hero_Prefabs}{heroID}.prefab", null);
                var heroRender = goHero.GetComponent<HeroRender>();
                goHero.name = $"lockstep_player_{heroID}_{roleData.role_name}";
                HeroLogic heroLogic = new HeroLogic(heroID, accountID, heroRender);
                heroLogic.SetIsSelfPlayer(isSelfPlayer);
                ListHeroLogics.Add(accountID, heroLogic);
                heroRender.SetLogicObject(heroLogic);

                //
                heroLogic.OnCreate();
                heroRender.OnCreate();

                if (isSelfPlayer) {
                    LocalHeroLogic = heroLogic;
                }
                if (isTeamLeader) {
                    Debug.LogError($"ChaseHeroLogic = heroLogic {heroLogic!=null}");
                    ChaseHeroLogic = heroLogic;
                }
                // HeroLogic = heroLogic;

                // 只有自己才需要触发相机跟随
                if (isSelfPlayer) {
                    TryFollowTarget(heroRender.transform);
                }
            }
            if (LocalHeroLogic == null) {
                Debug.LogError("错误,没有本地玩家");
            }
            if (ChaseHeroLogic == null) {
                Debug.LogError("错误,没有怪物仇恨 玩家");
            }
        }

        public HeroLogic GetHeroLogic(long accountID) {
            if (ListHeroLogics.TryGetValue(accountID, out var value)) {
                return value;
            }
            Debug.LogError($"找不到玩家:{accountID}");
            return null;
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