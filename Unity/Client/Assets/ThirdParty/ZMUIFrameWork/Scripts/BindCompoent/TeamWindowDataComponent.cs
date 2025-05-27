/*---------------------------------
 *Title:UI自动化组件生成代码生成工具
 *Author:铸梦
 *Date:2024/7/24 22:36:10
 *Description:变量需要以[Text]括号加组件类型的格式进行声明，然后右键窗口物体—— 一键生成UI数据组件脚本即可
 *注意:以下文件是自动生成的，任何手动修改都会被下次生成覆盖,若手动修改后,尽量避免自动生成
---------------------------------*/
using UnityEngine.UI;
using UnityEngine;

namespace ZMUIFrameWork
{
	public class TeamWindowDataComponent:MonoBehaviour
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
