using System;
using System.Collections.Concurrent;
using FixMath;
using ServerShareToClient;
using UnityEngine;

namespace ZMGC.Battle {
    public class BattleWorld : World {
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

        private static Type[] s_OrderCtrlTypes = new Type[] {
            typeof(HeroLogicCtrl),
            typeof(MonsterLogicCtrl),
            typeof(BattleLogicCtrl),
        };

        private static Type[] s_OrderDataTypes = new Type[]
            { };

        private static Type[] s_OrderMsgTypes = new Type[]
            { };

        public override WorldEnum WorldEnum => WorldEnum.BattleWorld;

        public override Type[] GetLogicBehaviourExecution() {
            return s_OrderCtrlTypes;
        }

        public override Type[] GetDataBehaviourExecution() {
            return s_OrderDataTypes;
        }

        public override Type[] GetMsgBehaviourExecution() {
            return s_OrderMsgTypes;
        }

        #endregion

        #region life-cycle

        public override void OnCreate() {
            Debug.Log("节约笔记本性能, 限制帧率60");
            Application.targetFrameRate = 60;
            base.OnCreate();
            HeroLogicCtrl = BattleWorld.GetExitsLogicCtrl<HeroLogicCtrl>();
            MonsterLogicCtrl = BattleWorld.GetExitsLogicCtrl<MonsterLogicCtrl>();
            HeroLogicCtrl.InitHero();
            MonsterLogicCtrl.InitMonster();
            UIModule.PopUpWindow<BattleWindow>();
            BuffSystem.Instance.OnCreate();
            _accLogicRealTime = 0f;
            _nextLogicFrameTime = 0f;

            AudioController.GetInstance().PlayMusicFade($"{AssetsPathConfig.Game_Audio_Path}BG/jizhou.mp3", 2f);
        }


        /// <summary>
        /// Unity驱动的渲染更新
        /// </summary>
        public override void OnUpdate() {
            base.OnUpdate();


            if (!LogicFrameConfig.UseLocalFrameUpdate) {
                return;
            }

            _accLogicRealTime += Time.deltaTime;

            // 当前逻辑帧时间大于下一个逻辑帧时间, 需要更新逻辑帧
            // 另外作用: 追帧 && 保证所有设备的逻辑帧的帧数的一致性
            while (_accLogicRealTime > _nextLogicFrameTime) {
                OnLigicFrameUpdate();
                LogicFrameConfig.LogicFrameID++;
                _nextLogicFrameTime += GameConstConfig.LogicFrameInterval;
            }

            _logicDeltaTime = (_accLogicRealTime + GameConstConfig.LogicFrameInterval - _nextLogicFrameTime) / GameConstConfig.LogicFrameInterval;
        }


        /// <summary>
        /// 应该通过服务端负责调用.
        /// </summary>
        public void OnLigicFrameUpdate() {
            HeroLogicCtrl.OnLogicFrameUpdate();
            MonsterLogicCtrl.OnLogicFrameUpdate();
            // TODO 这边的逻辑有问题, 伤害判定丢帧:
            // MonsterLogicCtrl.OnLogicFrameUpdate 驱动了怪物的位置更新
            // HeroLogicCtrl.OnLogicFrameUpdate()  驱动了玩家伤害的判定
            // 位置更新在后会导致伤害判定丢失, 导致丢伤害.  
            // 正确的做法应该是 先统一更新全部位置, 然后再统一进行伤害判定
            LogicActionController.Instance.OnLogicFrameUpdate();
            BuffSystem.Instance.OnLogicFrameUpdate();
            LogicTimerManager.Instance.OnLogicFrameUpdate();
        }

        public override void OnDestroy() {
            base.OnDestroy();
            LogicActionController.Instance.OnDestory();
            BuffSystem.Instance.OnDestory();
            LogicTimerManager.Instance.OnDestory();
            Debug.LogError("BattleWorld.OnDestroy");
        }

        public override void OnDestroyPostProcess(object args) {
            base.OnDestroyPostProcess(args);
            Debug.LogError("BattleWorld.OnDestroyPostProcess");
        }

        #endregion
    }
}