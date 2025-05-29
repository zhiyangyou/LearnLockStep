using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using ZMGC.Hall;

public class Handler_TeamStateChanged : Message<Msg_TeamStateChanged> {
    protected override async FTask Run(Session session, Msg_TeamStateChanged message) {
        HallWorld.GetExitsLogicCtrl<TeamLogicCtrl>().OnTeamStateChanged(message);
        await FTask.CompletedTask;
    }
}