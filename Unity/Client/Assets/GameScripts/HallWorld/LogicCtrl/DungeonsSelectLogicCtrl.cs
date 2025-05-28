/*--------------------------------------------------------------------------------------
* Title: 网络消息层脚本自动生成工具
* Author: 铸梦xy
* Date:2025/5/28 12:42:47
* Description:网络消息层,主要负责游戏网络消息的收发
* Modify:
* 注意:以下文件为自动生成，强制再次生成将会覆盖
----------------------------------------------------------------------------------------*/

using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;

namespace ZMGC.Hall {
    public class DungeonsSelectLogicCtrl : ILogicBehaviour {
        #region 属性和字段

        #endregion

        public void OnCreate() { }

        public void OnDestroy() { }

        #region public

        public void OnEnterDungeon(Msg_EnterDungeon msg) { }

        public void OnLoadDungeonProgress(Msg_LoadDungeonProgress msg) { }
        public void OnStartDungeon(Msg_StartDungeonBattle msg) { }

        #endregion
    }
}