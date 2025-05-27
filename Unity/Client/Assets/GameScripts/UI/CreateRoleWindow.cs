/*---------------------------------
 *Title:UI表现层脚本自动化生成工具
 *Author:ZM 铸梦
 *Date:2025/5/22 11:40:38
 *Description:UI 表现层，该层只负责界面的交互、表现相关的更新，不允许编写任何业务逻辑代码
 *注意:以下文件是自动生成的，再次生成不会覆盖原有的代码，会在原有的代码上进行新增，可放心使用
---------------------------------*/

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fantasy;
using UnityEngine.UI;
using UnityEngine;
using ZMGC.Hall;
using ZMUIFrameWork;

public class CreateRoleWindow : WindowBase {
    public CreateRoleWindowUIComponent uiCompt = new CreateRoleWindowUIComponent();
    private List<GameObject> _listRoleSelectItem = new();
    private UserDataMgr _userDataMgr;


    /// <summary>
    /// 创建角色页面的当前选择的角色ID
    /// </summary>
    public int _curSelectRoleID { get; set; }


    #region 声明周期函数

    //调用机制与Mono Awake一致
    public override void OnAwake() {
        this.FullScreenWindow = true;
        uiCompt.InitComponent(this);
        base.OnAwake();

        _userDataMgr = HallWorld.GetExitsDataMgr<UserDataMgr>();
        this._curSelectRoleID = _userDataMgr.RoleIDs.First(); 

        uiCompt.ItemRoleSelectGameObject.SetActive(false);
        uiCompt.CurSelectRoleIDText.text = $"ID: {this._curSelectRoleID}";
        foreach (var id in _userDataMgr.RoleIDs) {
            int roleID = id;
            var go = GameObject.Instantiate(uiCompt.ItemRoleSelectGameObject, uiCompt.ContentTransform);
            go.GetComponent<Button>().onClick.AddListener(() => { OnClick_RoleItem(roleID); });
            go.GetComponentInChildren<Text>().text = $"ID: {roleID}";
            go.SetActive(true);
            _listRoleSelectItem.Add(go);
        }
    }


    //物体销毁时执行
    public override void OnDestroy() {
        base.OnDestroy();

        base.OnDestroy();

        foreach (var go in _listRoleSelectItem) {
            GameObject.Destroy(go);
        }
        _listRoleSelectItem.Clear();
    }

    #endregion

    #region API Function

    #endregion

    #region private

    private void OnClick_RoleItem(int roleID) {
        this._curSelectRoleID = roleID; 
        uiCompt.CurSelectRoleIDText.text = $"ID: {roleID}";
    }

    #endregion

    #region UI组件事件

    public void OnNameInputEnd(string text) {
        HallWorld.GetExitsDataMgr<UserDataMgr>().UserName = text;
    }

    public void OnNameInputChange(string text) { }

    public void OnCloseButtonClick() {
        HideWindow();
    }

    public void OnEnterGameButtonClick() {
        NetSend_CreateRole();
    }

    #endregion


    #region 网络接口

    private async void NetSend_CreateRole() {
        Send_CreateRole sendCreateRole = new();
        sendCreateRole.account_id = _userDataMgr.account_id;
        sendCreateRole.role_id = _curSelectRoleID;
        sendCreateRole.role_name = uiCompt.NameInputField.text;
        if (sendCreateRole.account_id <= 0) {
            ToastManager.ShowToast($" sendCreateRole.account_id  非法:{sendCreateRole.account_id}");
            return;
        }
        if (sendCreateRole.role_id <= 0) {
            ToastManager.ShowToast($"roleID 非法:{sendCreateRole.role_id}");
            return;
        }
        if (string.IsNullOrEmpty(sendCreateRole.role_name)) {
            ToastManager.ShowToast($"角色名字不能是空");
            return;
        }
        PopUpWindow<ReConnectWindow>();
        Rcv_CreateRole resultCreateRole = await NetworkManager.Instance.SendCallMessage<Rcv_CreateRole>(sendCreateRole);

        var code = resultCreateRole.ErrorCode;
        if (code != 0) {
            ToastManager.ShowToast($"创建角色失败 code:{code}");
        }
        else {
            _userDataMgr.AddRoleData(resultCreateRole.role_data);
            ToastManager.ShowToast("创建角色成功");
            // TODO
            Debug.LogError("加载大厅, 并跳转  ");
        }
        UIModule.Instance.HideWindow<ReConnectWindow>();
    }

    #endregion
}