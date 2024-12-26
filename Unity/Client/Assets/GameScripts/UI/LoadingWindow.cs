/*---------------------------------
 *Title:UI表现层脚本自动化生成工具
 *Author:ZM 铸梦
 *Date:2024/12/25 20:00:48
 *Description:UI 表现层，该层只负责界面的交互、表现相关的更新，不允许编写任何业务逻辑代码
 *注意:以下文件是自动生成的，再次生成不会覆盖原有的代码，会在原有的代码上进行新增，可放心使用
---------------------------------*/

using UnityEngine.UI;
using UnityEngine;
using ZMUIFrameWork;

public class LoadingWindow : WindowBase
{
    public LoadingWindowUIComponent uiCompt = new LoadingWindowUIComponent();

    #region 声明周期函数

    //调用机制与Mono Awake一致
    public override void OnAwake()
    {
        uiCompt.InitComponent(this);
        base.OnAwake();
        mDisableAnim = true;
        UIEventControl.AddEvent(UIEventEnum.SceneLoadingProgress, OnEvent_OnSceneLoadingProgress); 
    }

    

    private void OnEvent_OnSceneLoadingProgress(object data)
    {
        uiCompt.SliderImage.fillAmount = (float)data; 
    }

    //物体显示时执行
    public override void OnShow()
    {
        base.OnShow();
    }

    //物体隐藏时执行
    public override void OnHide()
    {
        base.OnHide();
    }

    //物体销毁时执行
    public override void OnDestroy()
    {
        base.OnDestroy();
        UIEventControl.RemoveEvent(UIEventEnum.SceneLoadingProgress, OnEvent_OnSceneLoadingProgress); 
    }

    #endregion

    #region API Function

    #endregion

    #region UI组件事件

    public void OnCloseButtonClick()
    {
        HideWindow();
    }

    #endregion
}