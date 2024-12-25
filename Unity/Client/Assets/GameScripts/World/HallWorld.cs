using System;
using UnityEngine;

namespace ZMGC.Hall
{
    public class HallWorld : World
    {
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
            UIModule.Instance.PopUpWindow<CreateRuleWindow>();
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
    }
}