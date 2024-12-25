using System;
using UnityEngine;
using ZM.ZMAsset;

namespace ZMGC.Battle
{
    public class BattleWorld : World
    {
        private static Type[] CtrlTypes = new Type[]
        {
        };

        private static Type[] DataTypes = new Type[]
        {
        };

        private static Type[] MsgTypes = new Type[]
        {
        };

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

        public override WorldEnum WorldEnum => WorldEnum.BattleWorld;

        public override void OnCreate()
        {
            base.OnCreate();
            Debug.LogError("BattleWorld.OnCreate");
            ZMAsset.Instantiate($"{AssetsPathConfig.Game_Hero_Prefabs}/1000.prefab", null);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Debug.LogError("BattleWorld.OnDestroy");
        }

        public override void OnDestroyPostProcess(object args)
        {
            base.OnDestroyPostProcess(args);
            Debug.LogError("BattleWorld.OnDestroyPostProcess");
        }
    }
}