using System.Collections.Generic;
using FixIntPhysics;
using UnityEngine;
using ZM.ZMAsset;

namespace ZMGC.Battle
{
    /// <summary>
    /// 怪物逻辑
    /// </summary>
    public class MonsterLogicCtrl : ILogicBehaviour
    {
        #region 属性和字段

        /// <summary>
        /// 怪物列表
        /// </summary>
        public List<MonsterLogic> ListMonsterLogic { get; private set; } = new();

        private List<Vector3> _listMonsterPos = new List<Vector3>()
        {
            Vector3.zero,
        };

        private List<int> _listMonsterIDs = new List<int>()
        {
            20001,
        };

        #endregion

        #region life-cycle

        public void OnCreate()
        {
            // Debug.LogError("on create monster ... ");
        }

        public void OnDestroy()
        {
        }

        #endregion

        #region public

        public void InitMonster()
        {
            var index = 0;
            foreach (var pos in _listMonsterPos)
            {
                var monsterID = _listMonsterIDs[index];
                var goMonster = ZMAsset.Instantiate($"{AssetsPathConfig.Game_Monster_Prefabs}/{monsterID}.prefab", null);
                // 初始化
                goMonster.transform.position = pos;
                //
                BoxColliderGizmo boxInfo = goMonster.GetComponent<BoxColliderGizmo>();
                boxInfo.enabled = false;
                FixIntBoxCollider fixIntBoxCollider = new FixIntBoxCollider(boxInfo.mSize, boxInfo.mConter);
                fixIntBoxCollider.SetBoxData(boxInfo.mConter, boxInfo.mSize);
                fixIntBoxCollider.UpdateColliderInfo(goMonster.transform.position, boxInfo.mSize);
                //
                MonsterRender monsterRender = goMonster.GetComponent<MonsterRender>();
                MonsterLogic monsterLogic = new MonsterLogic(monsterID, monsterRender, fixIntBoxCollider);
                monsterLogic.OnCreate();
                monsterRender.OnCreate();

                ListMonsterLogic.Add(monsterLogic);

                index++;
            }
        }

        #endregion
    }
}