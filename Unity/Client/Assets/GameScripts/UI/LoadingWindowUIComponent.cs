/*---------------------------------
 *Title:UI自动化组件查找代码生成工具
 *Author:铸梦
 *Date:2024/12/25 19:53:03
 *Description:变量需要以[Text]括号加组件类型的格式进行声明，然后右键窗口物体—— 一键生成UI组件查找脚本即可
 *注意:以下文件是自动生成的，任何手动修改都会被下次生成覆盖,若手动修改后,尽量避免自动生成
---------------------------------*/
using UnityEngine.UI;
using UnityEngine;

namespace ZMUIFrameWork
{
	public class LoadingWindowUIComponent
	{
		public   RawImage  BaseMapRawImage;

		public   Button  CloseButton;

		public   Image  SliderImage;

		public  void InitComponent(WindowBase target)
		{
		     //组件查找
		     BaseMapRawImage =target.transform.Find("UIContent/[RawImage]BaseMap").GetComponent<RawImage>();
		     CloseButton =target.transform.Find("UIContent/[Button]Close").GetComponent<Button>();
		     SliderImage =target.transform.Find("UIContent/SliderRoot/[Image]Slider").GetComponent<Image>();
	
	
		     //组件事件绑定
		     LoadingWindow mWindow=(LoadingWindow)target;
		     target.AddButtonClickListener(CloseButton,mWindow.OnCloseButtonClick);
		}
	}
}
