/*---------------------------------
 *Title:UI自动化组件查找代码生成工具
 *Author:铸梦
 *Date:2025/5/27 18:15:26
 *Description:变量需要以[Text]括号加组件类型的格式进行声明，然后右键窗口物体—— 一键生成UI组件查找脚本即可
 *注意:以下文件是自动生成的，任何手动修改都会被下次生成覆盖,若手动修改后,尽量避免自动生成
---------------------------------*/
using UnityEngine.UI;
using UnityEngine;

namespace ZMUIFrameWork
{
	public class TeamWindowUIComponent
	{
		public   Button  CloseButton;

		public   Toggle  TeamToggle;

		public   Transform  LeftTransform;

		public   Button  CreateTeamButton;

		public   Button  JoinTeamButton;

		public   Transform  TeamItemParentTransform;

		public   InputField  TeamIDInputField;

		public  void InitComponent(WindowBase target)
		{
		     //组件查找
		     CloseButton =target.transform.Find("UIContent/[Button]Close").GetComponent<Button>();
		     TeamToggle =target.transform.Find("UIContent/[Toggle]Team").GetComponent<Toggle>();
		     LeftTransform =target.transform.Find("UIContent/[Transform]Left").transform;
		     CreateTeamButton =target.transform.Find("UIContent/[Transform]Left/[Button]CreateTeam").GetComponent<Button>();
		     JoinTeamButton =target.transform.Find("UIContent/[Transform]Left/[Button]JoinTeam").GetComponent<Button>();
		     TeamItemParentTransform =target.transform.Find("UIContent/[Transform]Left/[Transform]TeamItemParent").transform;
		     TeamIDInputField =target.transform.Find("UIContent/[Transform]Left/[InputField]TeamID").GetComponent<InputField>();
	
	
		     //组件事件绑定
		     TeamWindow mWindow=(TeamWindow)target;
		     target.AddButtonClickListener(CloseButton,mWindow.OnCloseButtonClick);
		     target.AddToggleClickListener(TeamToggle,mWindow.OnTeamToggleChange);
		     target.AddButtonClickListener(CreateTeamButton,mWindow.OnCreateTeamButtonClick);
		     target.AddButtonClickListener(JoinTeamButton,mWindow.OnJoinTeamButtonClick);
		     target.AddInputFieldListener(TeamIDInputField,mWindow.OnTeamIDInputChange,mWindow.OnTeamIDInputEnd);
		}
	}
}
