using System;
using ZMGC.Battle;
using UnityEngine;
using ZM.ZMAsset;

namespace ZMGC.Battle
{
    public class BattleWorld : World
    {
        #region 属性和字段

        /// <summary>
        /// 逻辑帧累计运行时间
        /// </summary>
        private float _accLogicRealTime;

        /// <summary>
        /// 下一个逻辑帧开始的时间
        /// </summary>
        private float _nextLogicFrameTime;

        /// <summary>
        /// deltaTime, 逻辑帧的增量时间
        /// </summary>
        private float _logicDeltaTime;

        public HeroLogicCtrl HeroLogicCtrl { get; private set; }
        public MonsterLogicCtrl MonsterLogicCtrl { get; private set; }

        #endregion

        #region override

        private static Type[] CtrlTypes = new Type[]
        {
        };

        private static Type[] DataTypes = new Type[]
        {
        };

        private static Type[] MsgTypes = new Type[]
        {
        };

        public override WorldEnum WorldEnum => WorldEnum.BattleWorld;

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

        #endregion

        #region life-cycle

        public override void OnCreate()
        {
            // Application.targetFrameRate = 60;
            base.OnCreate();
            HeroLogicCtrl = BattleWorld.GetExitsLogicCtrl<HeroLogicCtrl>();
            MonsterLogicCtrl = BattleWorld.GetExitsLogicCtrl<MonsterLogicCtrl>();
            HeroLogicCtrl.InitHero();
            MonsterLogicCtrl.InitMonster();
            UIModule.PopUpWindow<BattleWindow>();
            _accLogicRealTime = 0f;
            _nextLogicFrameTime = 0f;
        }


        /// <summary>
        /// Unity驱动的渲染更新
        /// </summary>
        public override void OnUpdate()
        {
            base.OnUpdate();
            _accLogicRealTime += Time.deltaTime;

            // 当前逻辑帧时间大于下一个逻辑帧时间, 需要更新逻辑帧
            // 另外作用: 追帧 && 保证所有设备的逻辑帧的帧数的一致性
            // 
            while (_accLogicRealTime > _nextLogicFrameTime)
            {
                OnLigicFrameUpdate();
                _nextLogicFrameTime += LogicFrameConfig.LogicFrameInterval;
                // 逻辑帧ID 进行自增
                LogicFrameConfig.LogicFrameID++;
            }

            _logicDeltaTime = (_accLogicRealTime + LogicFrameConfig.LogicFrameInterval - _nextLogicFrameTime) / LogicFrameConfig.LogicFrameInterval;
        }


        /// <summary>
        /// 应该通过服务端负责调用.
        /// </summary>
        public void OnLigicFrameUpdate()
        {
            HeroLogicCtrl.OnLogicFrameUpdate();
            MonsterLogicCtrl.OnLogicFrameUpdate();
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

        #endregion
    }
}