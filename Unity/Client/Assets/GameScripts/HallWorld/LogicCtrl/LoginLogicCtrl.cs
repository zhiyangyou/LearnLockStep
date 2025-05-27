/*--------------------------------------------------------------------------------------
* Title: 业务逻辑脚本自动生成工具
* Author: 铸梦xy
* Date:2025/5/21 10:58:10
* Description:业务逻辑层,主要负责游戏的业务逻辑处理
* Modify:
* 注意:以下文件为自动生成，强制再次生成将会覆盖
----------------------------------------------------------------------------------------*/

using System;
using System.Threading.Tasks;
using Fantasy;
using Fantasy.Network;
using UnityEngine;

namespace ZMGC.Hall {
    public class LoginLogicCtrl : ILogicBehaviour {
        /// <summary>
        /// 鉴权服务地址
        /// </summary>
        // private const string kStr_AuthenticationAddr = "127.0.0.1:20001";
        private const string kStr_AuthenticationAddr = "192.168.1.69:20001";

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

                    NetworkManager.Instance.Disconnect();
                    if (resp.ErrorCode != 0) {
                        Debug.LogError($"获取token 失败 code:{resp.ErrorCode}");
                        onResult?.Invoke(((int)resp.ErrorCode, resp));
                        resp = null;
                    }
                    else {
                        Debuger.LogGreen($"获取token 成功 {resp.login_address} {resp.token}");
                        onResult?.Invoke((0, resp));
                    }
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
                    // NetworkManager.Instance.Disconnect(); // 登录成功之后, session 会一直保持在gate服务器
                    onResult?.Invoke(((int)resp.ErrorCode, resp));
                },
                () => { onResult?.Invoke((ErrorCode.Code_NetConnectFailed, null)); }, null);
        }


        public async Task EnterHallWithSelectRole() {
            var userDataMgr = HallWorld.GetExitsDataMgr<UserDataMgr>();
            var curSelectIndex = userDataMgr.CurSelectRoleIndex;
            if (curSelectIndex < 0 && curSelectIndex >= userDataMgr.RoleDatas.Count) {
                ToastManager.ShowToast("尚未选择角色");
                return;
            }
            var roleData = userDataMgr.RoleDatas[curSelectIndex];
            if (roleData == null) {
                ToastManager.ShowToast("选择的index的数据是null");
                return;
            }
            
            Send_SelectRole request = new Send_SelectRole();
            request.account_id = userDataMgr.account_id;
            request.role_uid = roleData.uid;
            UIModule.Instance.PopUpWindow<ReConnectWindow>();
            var response = await NetworkManager.Instance.SendCallMessage<Rcv_SelectRole>(request);
            var code = response.ErrorCode;
            if (code == 0) {
                ToastManager.ShowToast("成功! 进入游戏中...");
                HallWorld.EnterHallWorld();
            }
            else {
                ToastManager.ShowToast($"进入游戏失败 {code}");
            }
            UIModule.Instance.HideWindow<ReConnectWindow>();
        }

        public void OnCreate() { }

        public void OnDestroy() { }
    }
}