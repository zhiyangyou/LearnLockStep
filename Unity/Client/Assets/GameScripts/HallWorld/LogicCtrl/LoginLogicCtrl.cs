/*--------------------------------------------------------------------------------------
* Title: 业务逻辑脚本自动生成工具
* Author: 铸梦xy
* Date:2025/5/21 10:58:10
* Description:业务逻辑层,主要负责游戏的业务逻辑处理
* Modify:
* 注意:以下文件为自动生成，强制再次生成将会覆盖
----------------------------------------------------------------------------------------*/

using System;
using Fantasy;
using Fantasy.Network;

namespace ZMGC.Hall {
    public class LoginLogicCtrl : ILogicBehaviour {
        private const string kStr_AuthenticationAddr = "127.0.0.1:20001";

        public void RegisterAccount(string account, string password, Action<int> onResult) {
            if (string.IsNullOrEmpty(account)
                || string.IsNullOrEmpty(password)
               ) {
                ToastManager.ShowToast("账号或是密码为空");
                onResult?.Invoke(ErrorCode.Code_InvalidInput);
                return;
            }
            NetworkManager.Instance.Connect(kStr_AuthenticationAddr, NetworkProtocolType.TCP, async () => {
                    Send_RegisterAccount register = new Send_RegisterAccount() {
                        pass_word = password,
                        user_name = account,
                    };
                    var resp = await NetworkManager.Instance.SendCallMessage<Rcv_RegisterAccount>(register);
                    NetworkManager.Instance.Disconnect();
                    onResult?.Invoke((int)resp.ErrorCode);
                },
                () => { onResult?.Invoke(ErrorCode.Code_NetConnectFailed); }, null);
        }

        public void OnCreate() { }

        public void OnDestroy() { }
    }
}