using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using ZMGC.Hall;

public class Hanlder_OtherPlayerStateSync : Message<Msg_OtherPlayerStateSync> {
    protected override async FTask Run(Session session, Msg_OtherPlayerStateSync message) {
        HallWorld.GetExitsLogicCtrl<HallRoleLogicCtrl>().SyncOtherRoleState(message);
    }
}