using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using ZMGC.Hall;

public class Handler_EnterDungeon : Message<Msg_EnterDungeon> {
    protected override async FTask Run(Session session, Msg_EnterDungeon message) {
        HallWorld.GetExitsLogicCtrl<DungeonsSelectLogicCtrl>().OnEnterDungeon(message);
        await FTask.CompletedTask;
    }
}