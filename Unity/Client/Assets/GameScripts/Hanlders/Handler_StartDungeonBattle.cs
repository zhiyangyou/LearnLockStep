using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using ZMGC.Hall;

public class Handler_StartDungeonBattle : Message<Msg_StartDungeonBattle> {
    protected override async FTask Run(Session session, Msg_StartDungeonBattle message) {
        HallWorld.GetExitsLogicCtrl<DungeonsSelectLogicCtrl>().OnStartDungeon(message);
        await FTask.CompletedTask;
    }
}