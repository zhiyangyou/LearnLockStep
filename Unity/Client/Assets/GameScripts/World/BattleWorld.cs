using System;
using ZMGC.Battle;
using UnityEngine;
using ZM.ZMAsset;

namespace ZMGC.Battle
{
    public class BattleWorld : World
    {
        public HeroLogicCtrl HeroLogicCtrl { get; private set; }
        public MonsterLogicCtrl MonsterLogicCtrl { get; private set; }

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
            HeroLogicCtrl = BattleWorld.GetExitsLogicCtrl<HeroLogicCtrl>();
            MonsterLogicCtrl = BattleWorld.GetExitsLogicCtrl<MonsterLogicCtrl>();
            HeroLogicCtrl.InitHero();
            MonsterLogicCtrl.InitMonster();
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