/*---------------------------------
 *Title:UI表现层脚本自动化生成工具
 *Author:ZM 铸梦
 *Date:2025/4/24 16:51:16
 *Description:UI 表现层，该层只负责界面的交互、表现相关的更新，不允许编写任何业务逻辑代码
 *注意:以下文件是自动生成的，再次生成不会覆盖原有的代码，会在原有的代码上进行新增，可放心使用
---------------------------------*/

using UnityEngine.UI;
using UnityEngine;
using ZMGC.Battle;
using ZMUIFrameWork;

public class BattleWindow : WindowBase {
    #region 属性和字段

    private HeroLogic _heroLogic = null;

    #endregion

    public BattleWindowDataComponent uiCompt = null;

    #region 声明周期函数

    //调用机制与Mono Awake一致
    public override void OnAwake() {
        uiCompt = transform.GetComponent<BattleWindowDataComponent>();
        uiCompt.InitComponent(this);
        base.OnAwake();
    }

    //物体显示时执行
    public override void OnShow() {
        base.OnShow();
        _heroLogic = BattleWorld.GetExitsLogicCtrl<HeroLogicCtrl>().HeroLogic;
    }

    //物体隐藏时执行
    public override void OnHide() {
        base.OnHide();
    }

    //物体销毁时执行
    public override void OnDestroy() {
        base.OnDestroy();
    }

    #endregion

    #region API Function

    #endregion

    #region UI组件事件

    public void OnNormalAttackButtonClick() {
        Debug.LogError($"按钮点击");
        _heroLogic.ReleaseNormalAttack(); //  普通攻击技能
    }

    #endregion
}