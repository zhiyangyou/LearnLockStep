/*---------------------------------
 *Title:UI表现层脚本自动化生成工具
 *Author:ZM 铸梦
 *Date:2025/5/21 9:29:16
 *Description:UI 表现层，该层只负责界面的交互、表现相关的更新，不允许编写任何业务逻辑代码
 *注意:以下文件是自动生成的，再次生成不会覆盖原有的代码，会在原有的代码上进行新增，可放心使用
---------------------------------*/

using Fantasy;
using UnityEngine.UI;
using UnityEngine;
using ZMGC.Hall;
using ZMUIFrameWork;

public class LoginWindow : WindowBase {
    private const string PerfKey_Account = "PerfKey_Account";
    private const string PerfKey_Password = "PerfKey_Password";
    public LoginWindowUIComponent uiCompt = new LoginWindowUIComponent();

    private LoginLogicCtrl _logicCtrl = null;

    private UserDataMgr _userData = null;

    #region 声明周期函数

    //调用机制与Mono Awake一致
    public override void OnAwake() {
        uiCompt.InitComponent(this);
        _logicCtrl = HallWorld.GetExitsLogicCtrl<LoginLogicCtrl>();
        _userData = HallWorld.GetExitsDataMgr<UserDataMgr>();
        base.OnAwake();
    }

    //物体显示时执行
    public override void OnShow() {
        base.OnShow();
        UIEventControl.AddEvent(UIEventEnum.LoginSuccess, OnLoginSuccess);
        uiCompt.AccountInputField.text = PersistentDataTool.LoadStr(PerfKey_Account);
        uiCompt.PasswordInputField.text = PersistentDataTool.LoadStr(PerfKey_Password);
    }


    //物体隐藏时执行
    public override void OnHide() {
        base.OnHide();
        UIEventControl.RemoveEvent(UIEventEnum.LoginSuccess, OnLoginSuccess);

        PersistentDataTool.SaveStr(PerfKey_Account, uiCompt.AccountInputField.text);
        PersistentDataTool.SaveStr(PerfKey_Password, uiCompt.PasswordInputField.text);
    }

    //物体销毁时执行
    public override void OnDestroy() {
        base.OnDestroy();
    }

    #endregion

    #region 事件回调

    private void OnLoginSuccess(object data) {
        HideWindow();
        UIModule.Instance.PopUpWindow<SelectRoleWindow>();
    }

    #endregion

    #region API Function

    #endregion

    #region UI组件事件

    public void OnCloseButtonClick() {
        HideWindow();
    }

    public void OnServerSelectButtonClick() { }
    public void OnAccountInputChange(string text) { }
    public void OnAccountInputEnd(string text) { }
    public void OnPasswordInputChange(string text) { }
    public void OnPasswordInputEnd(string text) { }

    public void OnEnterGameButtonClick() {
        PopUpWindow<ReConnectWindow>();
        _logicCtrl.GetToken(uiCompt.AccountInputField.text, uiCompt.PasswordInputField.text,
            tuple => {
                UIModule.Instance.HideWindow<ReConnectWindow>();
                var code = tuple.Item1;
                if (code == 0) {
                    ContinueLogin(tuple.Item2);
                }
                else {
                    ToastManager.ShowToast("获取token失败");
                }
            });
    }

    public void OnPublishButtonClick() { }
    public void OnClickEnterButtonClick() { }

    public void OnRegisterAccountButtonClick() {
        PopUpWindow<RegisterWindow>();
    }

    #endregion

    #region private

    private void ContinueLogin(Rcv_GetLoginToken tokenInfo) {
        _logicCtrl.LoginWithToken(tokenInfo, tp => {
            var code = tp.Item1;
            var loginData = tp.Item2;

            var toastMsg = code == 0 ? "登录成功" : $"登录失败:{code}";
            ToastManager.ShowToast(toastMsg);
            if (code == 0) {
                _userData.InitByLoginData(loginData);
                LoginSuccess();
            }
        });
    }

    private void LoginSuccess() {
        UIEventControl.DispensEvent(UIEventEnum.LoginSuccess);
    }

    #endregion
}