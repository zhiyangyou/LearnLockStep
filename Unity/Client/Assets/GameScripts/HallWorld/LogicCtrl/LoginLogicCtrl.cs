/*--------------------------------------------------------------------------------------
* Title: 业务逻辑脚本自动生成工具
* Author: 铸梦xy
* Date:2025/5/21 10:58:10
* Description:业务逻辑层,主要负责游戏的业务逻辑处理
* Modify:
* 注意:以下文件为自动生成，强制再次生成将会覆盖
----------------------------------------------------------------------------------------*/

using Fantasy;
using Fantasy.Network;

namespace ZMGC.Hall {
    public class LoginLogicCtrl : ILogicBehaviour {
        private const string kStr_AuthenticationAddr = "127.0.0.1:20001";

        public void RegisterAccount(string account, string password) {
            if (string.IsNullOrEmpty(account)
                || string.IsNullOrEmpty(password)
               ) {
                ToastManager.ShowToast("账号或是密码为空");
                return;
            }
            NetworkManager.Instance.Connect(kStr_AuthenticationAddr, NetworkProtocolType.TCP, async () => {
                    Send_RegisterAccount register = new Send_RegisterAccount() {
                        pass_word = password,
                        user_name = account,
                    };
                    var resp = await NetworkManager.Instance.SendCallMessage<Rcv_RegisterAccount>(register);
                    if (resp.ErrorCode == 0) {
                        ToastManager.ShowToast("注册成功");
                    }
                    else {
                        ToastManager.ShowToast($"注册失败{resp.ErrorCode}");
                        Debuger.LogGreen($"注册失败{resp.ErrorCode}");
                    }
                    NetworkManager.Instance.Disconnect();
                },
                () => {
                    const string msg = "鉴权服务器连接失败, 检查网络后重试";
                    ToastManager.ShowToast(msg);
                    Debuger.LogError(msg);
                },
                () => {
                    const string strDisconnectMsg = "鉴权服务器断联";
                    ToastManager.ShowToast(strDisconnectMsg);
                    Debuger.LogError(strDisconnectMsg);
                });
        }

        public void OnCreate() { }

        public void OnDestroy() { }
    }
}