using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using ZMGC.Battle;

namespace GameScripts.Hanlders {
    public class Handler_FrameOpEvent : Message<Msg_S2C_FrameOpEvent> {
        
        
        
        protected override async FTask Run(Session session, Msg_S2C_FrameOpEvent message) {
            BattleWorld.GetExitsLogicCtrl<BattleLogicCtrl>().OnLogicFrameUpdateByServer(message);
            await FTask.CompletedTask;
        }
    }
}