/*--------------------------------------------------------------------------------------
* Title: 网络消息层脚本自动生成工具
* Author: 铸梦xy
* Date:2025/5/28 12:42:47
* Description:网络消息层,主要负责游戏网络消息的收发
* Modify:
* 注意:以下文件为自动生成，强制再次生成将会覆盖
----------------------------------------------------------------------------------------*/

using System.Collections.Generic;
using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using UnityEngine;
using ZMGC.Battle;

namespace ZMGC.Hall {
    public class DungeonsSelectLogicCtrl : ILogicBehaviour {
        #region 属性和字段

        #endregion

        public void OnCreate() { }

        public void OnDestroy() { }

        #region public

        public void OnEnterDungeon(Msg_EnterDungeon msg) {
            if (msg.teamMembers == null || msg.teamMembers.Count == 0) {
                ToastManager.ShowToast("进入地下城失败, 队伍不存在");
            }

            HallWorld.EnterBattleWorld(msg.teamMembers);
        }

        public void OnLoadDungeonProgress(Msg_LoadDungeonProgress msg) {
            UIEventControl.DispensEvent(UIEventEnum.DungeonProgress, msg);
        }

        public void OnStartDungeon(Msg_StartDungeonBattle msg) {
            Debug.LogError("开始地下城");
            UIModule.Instance.DestroyAllWindow( new List<string>() {
                nameof(DungeonLoadingWindow)
            });
            WorldManager.CreateWorld<BattleWorld>(() => { BattleWorld.GetExitsDataMgr<HeroDataMgr>().CacheBattleRoleList(msg.battle_role_datas, msg.team_leader); });
            UIModule.Instance.PopUpWindow<BattleWindow>();
            UIModule.Instance.HideWindow<DungeonLoadingWindow>();
        }

        #endregion
    }
}