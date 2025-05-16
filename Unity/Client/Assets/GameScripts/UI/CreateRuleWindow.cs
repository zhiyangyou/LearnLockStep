/*---------------------------------
 *Title:UI表现层脚本自动化生成工具
 *Author:ZM 铸梦
 *Date:2024/12/24 21:33:31
 *Description:UI 表现层，该层只负责界面的交互、表现相关的更新，不允许编写任何业务逻辑代码
 *注意:以下文件是自动生成的，再次生成不会覆盖原有的代码，会在原有的代码上进行新增，可放心使用
---------------------------------*/

using System.Collections.Generic;
using System.Linq;
using GameScripts;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using ZMGC.Battle;
using ZMGC.Hall;
using ZMUIFrameWork;

public class CreateRuleWindow : WindowBase {
    #region 属性和字段

    private List<int> _listRoleIDs = new() {
        HeroIDConfig.HeroID_神枪手,
        HeroIDConfig.HeroID_鬼剑士,
    };

    private List<GameObject> _listRoleSelectItem = new();

    private int _curSelectRoleID = -1;

    #endregion

    public CreateRuleWindowDataComponent uiCompt;

    #region 声明周期函数

    //调用机制与Mono Awake一致
    public override void OnAwake() {
        base.OnAwake();
        uiCompt = gameObject.GetComponent<CreateRuleWindowDataComponent>();
        uiCompt.InitComponent(this);

        uiCompt.RoleTemplateGameObject.SetActive(false);

        _curSelectRoleID = _listRoleIDs.First();
        uiCompt.CurSelectRoleIDText.text = $"ID: {_curSelectRoleID}";
        foreach (var id in _listRoleIDs) {
            int roleID = id;
            var go = GameObject.Instantiate(uiCompt.RoleTemplateGameObject, uiCompt.RoleSelectListTransform);
            go.GetComponent<Button>().onClick.AddListener(() => { OnClick_RoleItem(roleID); });
            go.GetComponentInChildren<Text>().text = $"{roleID}";
            go.SetActive(true);
            _listRoleSelectItem.Add(go);
        }
    }

    //物体显示时执行
    public override void OnShow() {
        base.OnShow();
    }

    private void OnClick_RoleItem(int roleID) {
        _curSelectRoleID = roleID;
        uiCompt.CurSelectRoleIDText.text = $"ID: {_curSelectRoleID}";
    }

    //物体隐藏时执行
    public override void OnHide() {
        base.OnHide();
    }

    //物体销毁时执行
    public override void OnDestroy() {
        base.OnDestroy();

        foreach (var go in _listRoleSelectItem) {
            GameObject.Destroy(go);
        }
        _listRoleSelectItem.Clear();
    }

    #endregion

    #region API Function

    #endregion

    #region UI组件事件

    public void OnInputNameInputChange(string text) { }

    public void OnInputNameInputEnd(string text) {
        HallWorld.GetExitsDataMgr<UserDataMgr>().UserName = text;
    }

    public void OnEnterBtnButtonClick() {
        HallWorld.EnterBattleWorld();
        UIModule.Instance.HideWindow<CreateRuleWindow>();
    }


    public void OnRole1ButtonClick() { }

    public void OnRole2ButtonClick() { }

    #endregion
}