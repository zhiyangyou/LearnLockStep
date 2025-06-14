/*---------------------------------
 *Title:UI表现层脚本自动化生成工具
 *Author:ZM 铸梦
 *Date:2024/7/24 22:36:15
 *Description:UI 表现层，该层只负责界面的交互、表现相关的更新，不允许编写任何业务逻辑代码
 *注意:以下文件是自动生成的，再次生成不会覆盖原有的代码，会在原有的代码上进行新增，可放心使用
---------------------------------*/

using UnityEngine.UI;
using UnityEngine;
using ZMUIFrameWork;
using ZMGC.Hall;
using ZM.ZMAsset;
using System.Collections.Generic;

public class TeamWindow : WindowBase {
    public TeamWindowUIComponent dataCompt = new TeamWindowUIComponent();
    private TeamLogicCtrl _teamLogicCtrl;
    private TeamDataMgr _teamDataMgr;
    private UserDataMgr _userDataMgr;

    private List<TeamRoleItem> mTeamItemList = new List<TeamRoleItem>();

    #region 声明周期函数

    //调用机制与Mono Awake一致
    public override void OnAwake() {
        dataCompt.InitComponent(this);
        base.OnAwake();
        _teamDataMgr = HallWorld.GetExitsDataMgr<TeamDataMgr>();
        _userDataMgr = HallWorld.GetExitsDataMgr<UserDataMgr>();
        _teamLogicCtrl = HallWorld.GetExitsLogicCtrl<TeamLogicCtrl>();
    }

    //物体显示时执行
    public override void OnShow() {
        base.OnShow();
        UIEventControl.AddEvent(UIEventEnum.RefreshTeamList, OnRefreshTeamList);
    }

    //物体隐藏时执行
    public override void OnHide() {
        base.OnHide();
        UIEventControl.RemoveEvent(UIEventEnum.RefreshTeamList, OnRefreshTeamList);
        ReleaseItemList();
    }

    //物体销毁时执行
    public override void OnDestroy() {
        base.OnDestroy();
    }

    #endregion

    #region API Function

    public void OnRefreshTeamList(object data) {
        //释放历史Item对象
        ReleaseItemList();
        //重新从对象池中进行创建
        foreach (var item in _teamDataMgr.RoleDatas)
        {
            GameObject itemObj = ZMAsset.Instantiate(AssetsPathConfig.Hall_Prefabs_Item + "TeamRoleItem.prefab", dataCompt.TeamItemParentTransform);
            itemObj.transform.ReSetParent(dataCompt.TeamItemParentTransform);
            TeamRoleItem itemScript = itemObj.GetComponent<TeamRoleItem>();
            itemScript.SetItemData(item);
            mTeamItemList.Add(itemScript);
        }
    }

    public void ReleaseItemList() {
        foreach (var item in mTeamItemList) {
            item.Release();
        }
        mTeamItemList.Clear();
    }

    private void HideTeamButton() {
        dataCompt.CreateTeamButton.SetVisible(false);
        dataCompt.JoinTeamButton.SetVisible(false);
        dataCompt.TeamIDInputField.text = "Teamid : " + HallWorld.GetExitsDataMgr<TeamDataMgr>().TeamID;
        dataCompt.TeamIDInputField.enabled = false;
    }

    #endregion

    #region UI组件事件

    public void OnCloseButtonClick() {
        HideWindow();
    }

    public void OnTeamToggleChange(bool state, Toggle toggle) {
        dataCompt.LeftTransform.gameObject.SetActive(state);
    }

    public async void OnCreateTeamButtonClick() {
        dataCompt.CreateTeamButton.interactable = false;
        bool isSuccess = await _teamLogicCtrl.CreateTeam(UserDataMgr.TeamFightMapType, _userDataMgr.account_id);
        //房主关闭加入和创建按钮
        if (isSuccess) {
            HideTeamButton();
        }
        dataCompt.CreateTeamButton.interactable = true;
    }

    public async void OnJoinTeamButtonClick() {
        dataCompt.JoinTeamButton.interactable = false;
        bool isSuccess = await _teamLogicCtrl.JoinTeam(dataCompt.TeamIDInputField.text);
        if (isSuccess)
        {
            HideTeamButton();
        }
        dataCompt.TeamIDInputField.text = "";
        dataCompt.JoinTeamButton.interactable = true;
    }

    public void OnTeamIDInputChange(string text) { }
    public void OnTeamIDInputEnd(string text) { }

    #endregion

    public void OnTeamIDInputCaretInputChange(string arg0) { }

    public void OnTeamIDInputCaretInputEnd(string arg0) { }
}