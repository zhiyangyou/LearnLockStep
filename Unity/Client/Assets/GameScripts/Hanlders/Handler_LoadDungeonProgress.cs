using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using ZMGC.Hall;

public class Handler_LoadDungeonProgress : Message<Msg_LoadDungeonProgress> {
    protected override async FTask Run(Session session, Msg_LoadDungeonProgress message) {
        HallWorld.GetExitsLogicCtrl<DungeonsSelectLogicCtrl>().OnLoadDungeonProgress(message);
        await FTask.CompletedTask;
    }
}