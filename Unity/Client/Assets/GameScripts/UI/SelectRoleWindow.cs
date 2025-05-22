/*---------------------------------
 *Title:UI表现层脚本自动化生成工具
 *Author:ZM 铸梦
 *Date:2025/5/22 11:35:53
 *Description:UI 表现层，该层只负责界面的交互、表现相关的更新，不允许编写任何业务逻辑代码
 *注意:以下文件是自动生成的，再次生成不会覆盖原有的代码，会在原有的代码上进行新增，可放心使用
---------------------------------*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fantasy;
using Fantasy.Helper;
using UnityEngine.UI;
using UnityEngine;
using ZMGC.Hall;
using ZMUIFrameWork;

public class SelectRoleWindow : WindowBase {
    public SelectRoleWindowUIComponent uiCompt = new SelectRoleWindowUIComponent();
    private UserDataMgr _userDataMgr = null;
    private List<ItemSelectRole> _listSelectRoles = new();

    #region 声明周期函数

    //调用机制与Mono Awake一致
    public override void OnAwake() {
        this.FullScreenWindow = true;
        uiCompt.InitComponent(this);
        _userDataMgr = HallWorld.GetExitsDataMgr<UserDataMgr>();
        InitRoleItems();
        if (_userDataMgr.RoleDatas.Count > 0) {
            DoSelectItem(0); // 默认选着第一个
        }

        base.OnAwake();
    }

    //物体显示时执行
    public override void OnShow() {
        base.OnShow();
        SetModelsVisiable(true);
    }

    //物体隐藏时执行
    public override void OnHide() {
        base.OnHide();
    }

    //物体销毁时执行
    public override void OnDestroy() {
        DestoryAllRoleItems();
        base.OnDestroy();
    }

    public override void PseudoHidden(int value) {
        base.PseudoHidden(value);
        SetModelsVisiable(value != 0);
    }

    #endregion

    #region API Function

    #endregion

    #region UI组件事件

    public void OnCloseButtonClick() {
        Application.Quit(0);
    }

    public async void OnEnterGameButtonClick() {
        var curSelectIndex = _userDataMgr.CurSelectRoleIndex;
        if (curSelectIndex < 0 && curSelectIndex >= _userDataMgr.RoleDatas.Count) {
            ToastManager.ShowToast("尚未选择角色");
            return;
        }
        var roleData = _userDataMgr.RoleDatas[curSelectIndex];
        if (roleData == null) {
            ToastManager.ShowToast("选择的index的数据是null");
            return;
        }
        
        Send_SelectRole request = new Send_SelectRole();
        request.account_id = _userDataMgr.account_id;
        request.role_uid = roleData.uid;
        PopUpWindow<ReConnectWindow>();
        var response = await NetworkManager.Instance.SendCallMessage<Rcv_SelectRole>(request);
        var code = response.ErrorCode;
        if (code == 0) {
            ToastManager.ShowToast("成功! 进入游戏中...");
        }
        else {
            ToastManager.ShowToast($"进入游戏失败 {code}");
        }
        UIModule.Instance.HideWindow<ReConnectWindow>();
    }

    public void OnCreateRoleButtonClick() {
        PopUpWindow<CreateRoleWindow>();
    }

    public void OnDeleteButtonClick() { }
    public void OnAllowRightButtonClick() { }
    public void OnAllowLeftButtonClick() { }

    #endregion

    #region private

    private void SetModelsVisiable(bool visiable) {
        foreach (var item in _listSelectRoles) {
            item.SetModelVisiable(visiable);
        }
    }

    private void DestoryAllRoleItems() {
        foreach (var item in _listSelectRoles) {
            item.OnRelease();
        }
        _listSelectRoles.Clear();
    }

    private void InitRoleItems() {
        int initCount = Mathf.Max(8, _userDataMgr.RoleDatas.Count);
        var templateGo = uiCompt.SelectRoleItemGameObject;
        templateGo.SetActive(false);
        var roleDatas = _userDataMgr.RoleDatas;
        for (int i = 0; i < initCount; i++) {
            var newGo = GameObject.Instantiate(templateGo, templateGo.transform.parent);
            newGo.SetActive(true);
            ItemSelectRole itemSelectRole = newGo.GetComponent<ItemSelectRole>();
            _listSelectRoles.Add(itemSelectRole);

            RoleData? roleData = i < roleDatas.Count ? roleDatas[i] : null;
            itemSelectRole.SetData(roleData);
            int index = i;
            newGo.GetComponent<Button>().onClick.AddListener(() => { OnClick_RoleItem(index); });
        }
    }

    private void OnClick_RoleItem(int clickIndex) {
        DoSelectItem(clickIndex);
    }

    private void DoSelectItem(int clickIndex) {
        if (clickIndex >= 0 && clickIndex < _userDataMgr.RoleDatas.Count) {
            foreach (var item in _listSelectRoles) {
                item.SetIsSelect(false);
            }
            _listSelectRoles[clickIndex].SetIsSelect(true);
            _userDataMgr.CurSelectRoleIndex = clickIndex;
        }
    }

    #endregion
}