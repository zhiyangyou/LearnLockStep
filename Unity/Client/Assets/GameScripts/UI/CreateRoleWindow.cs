/*---------------------------------
 *Title:UI表现层脚本自动化生成工具
 *Author:ZM 铸梦
 *Date:2025/5/22 11:40:38
 *Description:UI 表现层，该层只负责界面的交互、表现相关的更新，不允许编写任何业务逻辑代码
 *注意:以下文件是自动生成的，再次生成不会覆盖原有的代码，会在原有的代码上进行新增，可放心使用
---------------------------------*/

using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using ZMGC.Hall;
using ZMUIFrameWork;

public class CreateRoleWindow : WindowBase {
    public CreateRoleWindowUIComponent uiCompt = new CreateRoleWindowUIComponent();
    private List<GameObject> _listRoleSelectItem = new();
    private UserDataMgr _userDataMgr;

    #region 声明周期函数

    //调用机制与Mono Awake一致
    public override void OnAwake() {
        uiCompt.InitComponent(this);
        base.OnAwake();

        _userDataMgr = HallWorld.GetExitsDataMgr<UserDataMgr>();
        _userDataMgr.CurSelectRoleID = _userDataMgr.RoleIDs.First();
        
        uiCompt.ItemRoleSelectGameObject.SetActive(false);
        uiCompt.CurSelectRoleIDText.text = $"ID: {_userDataMgr.CurSelectRoleID}";
        foreach (var id in _userDataMgr.RoleIDs) {
            int roleID = id;
            var go = GameObject.Instantiate(uiCompt.ItemRoleSelectGameObject, uiCompt.ContentTransform);
            go.GetComponent<Button>().onClick.AddListener(() => { OnClick_RoleItem(roleID); });
            go.GetComponentInChildren<Text>().text =$"ID: {roleID}";
            go.SetActive(true);
            _listRoleSelectItem.Add(go);
        }
    }

    //物体显示时执行
    public override void OnShow() {
        base.OnShow();
    }

    //物体隐藏时执行
    public override void OnHide() {
        base.OnHide();
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
        _userDataMgr.CurSelectRoleID = roleID;
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
        HallWorld.EnterBattleWorld();
        UIModule.Instance.HideWindow<CreateRoleWindow>();
    }

    public void OnCreateRoleButtonClick() { }
    public void OnDeleteButtonClick() { }
    public void OnAllowRightButtonClick() { }
    public void OnAllowLeftButtonClick() { }

    #endregion
}