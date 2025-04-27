using System.Collections.Generic;
using UnityEngine;

namespace ZMGC.Battle
{
    /// <summary>
    ///  战斗业务逻辑
    /// </summary>
    public class BattleLogicCtrl : ILogicBehaviour
    {
        #region 属性字段

        private HeroLogicCtrl _heroLogicCtrl;
        private MonsterLogicCtrl _monsterLogicCtrl;

        #endregion

        #region public

        public void OnCreate()
        {
            _heroLogicCtrl = BattleWorld.GetExitsLogicCtrl<HeroLogicCtrl>();
            _monsterLogicCtrl = BattleWorld.GetExitsLogicCtrl<MonsterLogicCtrl>();
        }

        public void OnDestroy()
        {
        }


        /// <summary>
        /// 根据当前对象类型,获取攻击目标
        /// </summary>
        /// <returns></returns>
        public List<LogicActor> GetEnemyList(LogicObjectType selfType)
        {
            List<LogicActor> listEnemy = new();
            if (selfType == LogicObjectType.Hero)
            {
                foreach (var monsterLogicActor in _monsterLogicCtrl.ListMonsterLogic)
                {
                    listEnemy.Add(monsterLogicActor);
                }
            }
            else if (selfType == LogicObjectType.Monster)
            {
                listEnemy.Add(_heroLogicCtrl.HeroLogic);
            }
            else
            {
                Debug.LogError($"不支持的LogicType类型:{selfType}");
            }
            return listEnemy;
        }

        #endregion
    }
}