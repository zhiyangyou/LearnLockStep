/*--------------------------------------------------------------------------------------
* Title: 网络消息层脚本自动生成工具
* Author: 铸梦xy
* Date:2025/5/28 12:59:18
* Description:网络消息层,主要负责游戏网络消息的收发
* Modify:
* 注意:以下文件为自动生成，强制再次生成将会覆盖
----------------------------------------------------------------------------------------*/

using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;

namespace ZMGC.Hall {
    public class Handler_StartDungeonBattle : Message<Msg_StartDungeonBattle> {
        protected override async FTask Run(Session session, Msg_StartDungeonBattle message) {
            HallWorld.GetExitsLogicCtrl<DungeonsSelectLogicCtrl>().OnStartDungeon(message);
            await FTask.CompletedTask;
        }
    }

    public class Handler_LoadDungeonProgress : Message<Msg_LoadDungeonProgress> {
        protected override async FTask Run(Session session, Msg_LoadDungeonProgress message) {
            HallWorld.GetExitsLogicCtrl<DungeonsSelectLogicCtrl>().OnLoadDungeonProgress(message);
            await FTask.CompletedTask;
        }
    }

    public class Handler_EnterDungeon : Message<Msg_EnterDungeon> {
        protected override async FTask Run(Session session, Msg_EnterDungeon message) {
            HallWorld.GetExitsLogicCtrl<DungeonsSelectLogicCtrl>().OnEnterDungeon(message);
            await FTask.CompletedTask;
        }
    }

    public class DungeonsSelectMsgMgr : IMsgBehaviour {
        public void OnCreate() { }

        public void OnDestroy() { }


        public void SendMessage_EnterDungeon(int teamID, DungeonType dungeonType) {
            Msg_EnterDungeon msg = new Msg_EnterDungeon();
            msg.team_id = teamID;
            msg.dungeonCfgID = (int)dungeonType;
            NetworkManager.Instance.Send(msg);
        }

        public void SendMessage_LoadDungeonProgess(int teamID, long account_id, float progress) {
            Msg_LoadDungeonProgress msg = new();
            msg.team_id = teamID;
            msg.account_id = account_id;
            msg.progress = progress;
            NetworkManager.Instance.Send(msg);
        }
    }
}