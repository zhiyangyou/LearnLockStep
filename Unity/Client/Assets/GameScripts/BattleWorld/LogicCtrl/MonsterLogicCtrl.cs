using System;
using System.Collections.Generic;
using FixIntPhysics;
using FixMath;
using UnityEngine;
using ZM.ZMAsset;

namespace ZMGC.Battle {
    /// <summary>
    /// 怪物逻辑
    /// </summary>
    public class MonsterLogicCtrl : ILogicBehaviour {
        #region 属性和字段

        /// <summary>
        /// 怪物列表
        /// </summary>
        public List<MonsterLogic> ListMonsterLogic { get; private set; } = new();

        private List<(Vector3, int)> _listMonsterPosAndID = new() {
            (new Vector3(-2, 0, 0), 20001),
            (new Vector3(0, 0, 0), 20005),
            (new Vector3(2, 0, 0), 30001),
        };

        #endregion

        #region life-cycle

        public void OnCreate() {
            // Debug.LogError("on create monster ... ");
        }

        public void OnDestroy() { }

        #endregion

        #region public

        public void OnLogicFrameUpdate() {
            for (int i = ListMonsterLogic.Count - 1; i >= 0; i--) {
                var monsterLogic = ListMonsterLogic[i];
                monsterLogic.OnLogicFrameUpdate();
            }
        }

        public void InitMonster() {
            var index = 0;
            foreach (var tp in _listMonsterPosAndID) {
                var pos = tp.Item1;
                var monsterID = tp.Item2;
                FixIntVector3 logicPos = new FixIntVector3(pos);
                var goMonster = ZMAsset.Instantiate($"{AssetsPathConfig.Game_Monster_Prefabs}{monsterID}.prefab", null);
                // 初始化
                BoxColliderGizmo boxInfo = goMonster.GetComponent<BoxColliderGizmo>();
                boxInfo.enabled = false;
                FixIntBoxCollider fixIntBoxCollider = new FixIntBoxCollider(boxInfo.mSize, boxInfo.mConter);
                fixIntBoxCollider.SetBoxData(boxInfo.mConter, boxInfo.mSize);
                fixIntBoxCollider.UpdateColliderInfo(pos, boxInfo.mSize);
                //
                MonsterRender monsterRender = goMonster.GetComponent<MonsterRender>();
                MonsterLogic monsterLogic = new MonsterLogic(monsterID, monsterRender, fixIntBoxCollider, logicPos);
                monsterRender.SetLogicObject(monsterLogic);
                monsterLogic.OnCreate();
                monsterRender.OnCreate();

                ListMonsterLogic.Add(monsterLogic);

                index++;
            }
        }

        #endregion
    }
}