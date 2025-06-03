using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Fantasy;
using FixMath;
using ServerShareToClient;
using UnityEngine;
using ZMGC.Hall;

namespace ZMGC.Battle {
    /// <summary>
    ///  战斗业务逻辑
    /// </summary>
    public class BattleLogicCtrl : ILogicBehaviour {
        #region 属性字段

        private object lockObjReleaseSkillUniqueID = new();
        private long _GetUniqueID_FrameEventOpContextObj = 0;

        public long GetUniqueID_FrameEventOpContext {
            get {
                lock (lockObjReleaseSkillUniqueID) {
                    _GetUniqueID_FrameEventOpContextObj++;
                }
                return _GetUniqueID_FrameEventOpContextObj;
            }
        }

        private ConcurrentDictionary<long, object> _dicAllContextObjs = new();

        private HeroLogicCtrl _heroLogicCtrl;
        private MonsterLogicCtrl _monsterLogicCtrl;
        private BattleWorld _battleWorld;
        private BattleDataMgr _battleDataMgr;
        private BattleMsgMgr _battleMsgMgr;
        private long _accountID = 0;

        #endregion

        #region public

        public void OnCreate() {
            _heroLogicCtrl = BattleWorld.GetExitsLogicCtrl<HeroLogicCtrl>();
            _monsterLogicCtrl = BattleWorld.GetExitsLogicCtrl<MonsterLogicCtrl>();
            _battleDataMgr = BattleWorld.GetExitsDataMgr<BattleDataMgr>();
            _battleMsgMgr = BattleWorld.GetExitsMsgMgr<BattleMsgMgr>();
            _accountID = HallWorld.GetExitsDataMgr<UserDataMgr>().account_id;
            _battleWorld = WorldManager.DefaultGameWorld as BattleWorld;
            if (_battleWorld == null) {
                Debug.LogError("_battleWorld == null");
            }
        }

        public void OnDestroy() {
            ResetContexts();
        }


        /// <summary>
        /// 根据当前对象类型,获取攻击目标
        /// </summary>
        /// <returns></returns>
        public List<LogicActor> GetEnemyList(LogicObjectType selfType) {
            List<LogicActor> listEnemy = new();
            if (selfType == LogicObjectType.Hero) {
                foreach (var monsterLogicActor in _monsterLogicCtrl.ListMonsterLogic) {
                    listEnemy.Add(monsterLogicActor);
                }
            }
            else if (selfType == LogicObjectType.Monster) {
                listEnemy.AddRange(_heroLogicCtrl.ListHeroLogics.Values);
            }
            else {
                Debug.LogError($"不支持的LogicType类型:{selfType}");
            }
            return listEnemy;
        }

        public void OnLogicFrameUpdateByServer(Msg_S2C_FrameOpEvent message) {
            // 使用本地
            if (LogicFrameConfig.UseLocalFrameUpdate) return;


            LogicFrameConfig.LogicFrameID = message.logic_frame_id;
            _battleDataMgr.BattleState = BattleStateEnum.Start;
            _battleDataMgr.BattleID = message.battle_id;

            // 更新玩家输入
            foreach (FrameOperateData frameOpData in message.frame_operate_datas) {
                _heroLogicCtrl.GetHeroLogic(frameOpData.account_id)?.LogicFrameEvent_NetInput(frameOpData);
            }

            // 
            _battleWorld.OnLigicFrameUpdate();
        }


        public void FrameOP_MoveDataInput(FixIntVector3 inputDir) {
            // Debug.LogError($"inputDir:{inputDir}");
            SendFrameOpData(EBattlePlayerOpType.InputMove, inputDir, 0, FixIntVector3.zero, EBattleOperateSkillType.None);
        }


        /// <summary>
        /// 释放技能的输入
        /// </summary>
        /// <param name="skillID"></param>
        /// <param name="guidePos">引导位置</param>
        public void ReleaseSkillInput(int skillID, FixIntVector3 guidePos, EBattleOperateSkillType skillType, OnReleaseSkillResult followContextObj) {
            var releaseSkillContextID = Save(followContextObj);
            SendFrameOpData(EBattlePlayerOpType.ReleaseSkill, FixIntVector3.zero, skillID, guidePos, skillType, releaseSkillContextID);
        }

        #endregion

        #region private

        public void ResetContexts() {
            this._GetUniqueID_FrameEventOpContextObj = 0;
            _dicAllContextObjs.Clear();
        }

        public long Save(object objContext) {
            var uniqueID = GetUniqueID_FrameEventOpContext;
            _dicAllContextObjs.TryAdd(uniqueID, objContext);
            return uniqueID;
        }

        public object LoadContextObj(long uniqueID) {
            _dicAllContextObjs.TryRemove(uniqueID, out var ret);
            return ret;
        }

        private void SendFrameOpData(
            EBattlePlayerOpType opType,
            FixIntVector3 inputDir,
            int skillID,
            FixIntVector3 skillPos,
            EBattleOperateSkillType skillType,
            long releaseSkillContextID = 0
        ) {
            if (_battleDataMgr.BattleState != BattleStateEnum.Start) {
                return;
            }
            FrameOperateData opData = new FrameOperateData();
            opData.operate_type = (int)opType;
            opData.account_id = _accountID;
            switch (opType) {
                case EBattlePlayerOpType.None:
                    break;
                case EBattlePlayerOpType.InputMove:
                    opData.input_dir = inputDir.ToCSVector3();
                    // Debug.LogError($"SendFrameOpData input_dir:{opData.input_dir.ToStr()} inputDir:{inputDir}"); // TODO 太高频了!!!
                    break;
                case EBattlePlayerOpType.ReleaseSkill:
                    opData.skillId = skillID;
                    opData.skillPos = skillPos.ToCSVector3();
                    opData.skillType = (int)skillType;
                    opData.frame_op_context_object_id = releaseSkillContextID;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(opType), opType, null);
            }

            // TODO ??? 不应该是一次性收集多个, 然后统一发送吗??
            _battleDataMgr.AddFrameOpData(opData);
            _battleMsgMgr.SendMsg_FrameOpEvent(_battleDataMgr.BattleID, _battleDataMgr.ListFrameOpDatas);
            _battleDataMgr.ClearFrameOpData();
        }

        #endregion
    }
}