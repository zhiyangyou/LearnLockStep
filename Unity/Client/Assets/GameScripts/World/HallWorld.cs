using System;
using System.Collections.Generic;
using Fantasy;
using GameScripts;
using UnityEngine;
using ZMGC.Battle;

namespace ZMGC.Hall {
    public class HallWorld : World {
        #region override

        private static Type[] CtrlTypes = new Type[] {
            typeof(UserLogicCtrl),
        };

        private static Type[] DataTypes = new Type[] {
            typeof(UserDataMgr)
        };

        private static Type[] MsgTypes = new Type[] {
            typeof(UserMsgMgr)
        };

        public override void OnCreate() {
            base.OnCreate();
            // pop first winow
            UIModule.Instance.PopUpWindow<LoginWindow>();
            AudioController.GetInstance().PlayMusicFade($"{AssetsPathConfig.Game_Audio_Path}BG/Login.mp3", 2f);
        }

        public override void OnUpdate() {
            base.OnUpdate();
        }

        public override void OnDestroy() {
            base.OnDestroy();
        }

        public override void OnDestroyPostProcess(object args) {
            Debug.LogError("HallWorld OnDestroyPostProcess");
            base.OnDestroyPostProcess(args);
        }

        public override Type[] GetLogicBehaviourExecution() {
            return CtrlTypes;
        }

        public override Type[] GetDataBehaviourExecution() {
            return DataTypes;
        }

        public override Type[] GetMsgBehaviourExecution() {
            return MsgTypes;
        }

        public override WorldEnum WorldEnum => WorldEnum.HallWorld;

        #endregion

        #region public

        public static void EnterHallWorld() {
            UIModule.Instance.PopUpWindow<LoadingWindow>();
            LoadSceneManager.Instance.LoadSceneAsync($"{AssetsPathConfig.Scene_Path}Hall", async () => {
                UIModule.Instance.DestroyAllWindow();
                UIModule.Instance.PopUpWindow<HallWindow>();
                UIModule.Instance.PopUpWindow<TeamWindow>(); // 组队弹窗
                await GetExitsLogicCtrl<MapLogicCtrl>().Init();
                UserDataMgr userData = HallWorld.GetExitsDataMgr<UserDataMgr>();
                await GetExitsLogicCtrl<HallRoleLogicCtrl>().InitSelfRole(userData.CurSelectRoleID);
            });
        }


        public static void EnterBattleWorld(List<RoleData> teamMembers) {
            UIModule.Instance.PopUpWindow<DungeonLoadingWindow>().InitView(teamMembers);
            LoadSceneManager.Instance.LoadSceneAsync($"{AssetsPathConfig.Scene_Path}Battle", async () => { },
                (loadProgress) => {
                    var userDataMgr = GetExitsDataMgr<UserDataMgr>();
                    var teamDataMgr = GetExitsDataMgr<TeamDataMgr>();
                    var _dungeonsSelectMsgMgr = GetExitsMsgMgr<DungeonsSelectMsgMgr>();
                    _dungeonsSelectMsgMgr.SendMessage_LoadDungeonProgess(teamDataMgr.TeamID, userDataMgr.account_id, loadProgress);
                });
        }

        #endregion
    }
}