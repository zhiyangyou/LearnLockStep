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
using UnityEngine;

namespace ZMGC.Hall {
    public class LoginLogicCtrl : ILogicBehaviour {
        /// <summary>
        /// 鉴权服务地址
        /// </summary>
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

        public void GetToken(string account, string password, Action<(int result, Rcv_GetLoginToken loginToken)> onResult) {
            if (string.IsNullOrEmpty(account)
                || string.IsNullOrEmpty(password)
               ) {
                ToastManager.ShowToast("GetToken: 账号或是密码为空");
                onResult?.Invoke((-1, null));
                return;
            }

            NetworkManager.Instance.Connect(kStr_AuthenticationAddr, NetworkProtocolType.TCP, async () => {
                    var resp = await NetworkManager.Instance.SendCallMessage<Rcv_GetLoginToken>(new Send_GetLoginToken() {
                        account_name = account,
                        pass_word = password,
                    });
                    if (resp.ErrorCode != 0) {
                        Debug.LogError($"获取token 失败 code:{resp.ErrorCode}");
                        resp = null;
                    }
                    else {
                        Debuger.LogGreen($"获取token 成功 {resp.login_address} {resp.token}");
                    }
                    NetworkManager.Instance.Disconnect();
                    onResult?.Invoke((0, resp));
                },
                () => { onResult?.Invoke((-1, null)); }, null);
        }


        public void LoginWithToken(Rcv_GetLoginToken tokenInfo, Action<(int, Rcv_LoginGate)> onResult) {
            if (tokenInfo == null
                || string.IsNullOrEmpty(tokenInfo.token)
                || string.IsNullOrEmpty(tokenInfo.login_address)
               ) {
                ToastManager.ShowToast("LoginWithToken 输入参数不合法");
                onResult?.Invoke((-1, null));
                return;
            }

            NetworkManager.Instance.Connect(tokenInfo.login_address, NetworkProtocolType.TCP, async () => {
                    Send_LoginGate loginGate = new() {
                        token = tokenInfo.token,
                        account_id = tokenInfo.account_id,
                        scene_config_id = tokenInfo.scene_config_id,
                    };
                    var resp = await NetworkManager.Instance.SendCallMessage<Rcv_LoginGate>(loginGate);
                    NetworkManager.Instance.Disconnect();
                    onResult?.Invoke(((int)resp.ErrorCode, resp));
                },
                () => { onResult?.Invoke((ErrorCode.Code_NetConnectFailed, null)); }, null);
        }


        public void OnCreate() { }

        public void OnDestroy() { }
    }
}