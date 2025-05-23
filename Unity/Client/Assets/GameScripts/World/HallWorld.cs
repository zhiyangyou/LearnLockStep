using System;
using GameScripts;
using UnityEngine;
using ZMGC.Battle;

namespace ZMGC.Hall
{
    public class HallWorld : World
    {
        #region override

        private static Type[] CtrlTypes = new Type[]
        {
            typeof(UserLogicCtrl),
        };

        private static Type[] DataTypes = new Type[]
        {
            typeof(UserDataMgr)
        };

        private static Type[] MsgTypes = new Type[]
        {
            typeof(UserMsgMgr)
        };

        public override void OnCreate()
        {
            base.OnCreate();
            // pop first winow
            UIModule.Instance.PopUpWindow<LoginWindow>();
            AudioController.GetInstance().PlayMusicFade($"{AssetsPathConfig.Game_Audio_Path}BG/Login.mp3",2f);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override void OnDestroyPostProcess(object args)
        {
            Debug.LogError("HallWorld OnDestroyPostProcess");
            base.OnDestroyPostProcess(args);
        }

        public override Type[] GetLogicBehaviourExecution()
        {
            return CtrlTypes;
        }

        public override Type[] GetDataBehaviourExecution()
        {
            return DataTypes;
        }

        public override Type[] GetMsgBehaviourExecution()
        {
            return MsgTypes;
        }

        public override WorldEnum WorldEnum => WorldEnum.HallWorld;

        #endregion

        #region public

        public static void EnterBattleWorld()
        {
            LoadSceneManager.Instance.LoadSceneAsync("Hall", () =>
            { 
                UIModule.Instance.DestroyAllWindow();
                UIModule.Instance.PopUpWindow<HallWindow>();
                HallWorld.GetExitsLogicCtrl<MapLogicCtrl>().Init();
                HallWorld.GetExitsLogicCtrl<HallRoleLogicCtrl>().Init();
            });
        }

        #endregion
    }
}