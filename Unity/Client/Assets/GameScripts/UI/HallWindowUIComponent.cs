/*---------------------------------
 *Title:UI自动化组件查找代码生成工具
 *Author:铸梦
 *Date:2025/5/23 9:54:11
 *Description:变量需要以[Text]括号加组件类型的格式进行声明，然后右键窗口物体—— 一键生成UI组件查找脚本即可
 *注意:以下文件是自动生成的，任何手动修改都会被下次生成覆盖,若手动修改后,尽量避免自动生成
---------------------------------*/
using UnityEngine.UI;
using UnityEngine;

namespace ZMUIFrameWork
{
	public class HallWindowUIComponent
	{
		public   Button  HeadButton;

		public   Button  DailyButton;

		public   Button  CultivateButton;

		public   Toggle  TaskToggle;

		public   Button  TransferButton;

		public   Button  ChallengeButton;

		public   JoystickUGUI  RoleJoystickUGUI;

		public  void InitComponent(WindowBase target)
		{
		     //组件查找
		     HeadButton =target.transform.Find("UIContent/topleft/MainHead/[Button]Head").GetComponent<Button>();
		     DailyButton =target.transform.Find("UIContent/bottomrightVertical/dailyroot/[Button]Daily").GetComponent<Button>();
		     CultivateButton =target.transform.Find("UIContent/bottomrightVertical/bestrongroot/[Button]Cultivate").GetComponent<Button>();
		     TaskToggle =target.transform.Find("UIContent/left/[Toggle]Task").GetComponent<Toggle>();
		     TransferButton =target.transform.Find("UIContent/[Button]Transfer").GetComponent<Button>();
		     ChallengeButton =target.transform.Find("UIContent/[Button]Challenge").GetComponent<Button>();
		     RoleJoystickUGUI =target.transform.Find("UIContent/[JoystickUGUI]Role").GetComponent<JoystickUGUI>();
	
	
		     //组件事件绑定
		     HallWindow mWindow=(HallWindow)target;
		     target.AddButtonClickListener(HeadButton,mWindow.OnHeadButtonClick);
		     target.AddButtonClickListener(DailyButton,mWindow.OnDailyButtonClick);
		     target.AddButtonClickListener(CultivateButton,mWindow.OnCultivateButtonClick);
		     target.AddToggleClickListener(TaskToggle,mWindow.OnTaskToggleChange);
		     target.AddButtonClickListener(TransferButton,mWindow.OnTransferButtonClick);
		     target.AddButtonClickListener(ChallengeButton,mWindow.OnChallengeButtonClick);
		}
	}
}
