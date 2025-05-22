/*---------------------------------
 *Title:UI自动化组件查找代码生成工具
 *Author:铸梦
 *Date:2025/5/22 17:46:23
 *Description:变量需要以[Text]括号加组件类型的格式进行声明，然后右键窗口物体—— 一键生成UI组件查找脚本即可
 *注意:以下文件是自动生成的，任何手动修改都会被下次生成覆盖,若手动修改后,尽量避免自动生成
---------------------------------*/
using UnityEngine.UI;
using UnityEngine;

namespace ZMUIFrameWork
{
	public class SelectRoleWindowUIComponent
	{
		public   Transform  RoleListTransform;

		public   GameObject  SelectRoleItemGameObject;

		public   Button  CloseButton;

		public   Button  EnterGameButton;

		public   Button  CreateRoleButton;

		public   Button  DeleteButton;

		public   Button  AllowRightButton;

		public   Button  AllowLeftButton;

		public  void InitComponent(WindowBase target)
		{
		     //组件查找
		     RoleListTransform =target.transform.Find("UIContent/RoleRoot/[Transform]RoleList").transform;
		     SelectRoleItemGameObject =target.transform.Find("UIContent/RoleRoot/[Transform]RoleList/[GameObject]SelectRoleItem").gameObject;
		     CloseButton =target.transform.Find("UIContent/[Button]Close").GetComponent<Button>();
		     EnterGameButton =target.transform.Find("UIContent/[Button]EnterGame").GetComponent<Button>();
		     CreateRoleButton =target.transform.Find("UIContent/[Button]CreateRole").GetComponent<Button>();
		     DeleteButton =target.transform.Find("UIContent/[Button]Delete").GetComponent<Button>();
		     AllowRightButton =target.transform.Find("UIContent/[Button]AllowRight").GetComponent<Button>();
		     AllowLeftButton =target.transform.Find("UIContent/[Button]AllowLeft").GetComponent<Button>();
	
	
		     //组件事件绑定
		     SelectRoleWindow mWindow=(SelectRoleWindow)target;
		     target.AddButtonClickListener(CloseButton,mWindow.OnCloseButtonClick);
		     target.AddButtonClickListener(EnterGameButton,mWindow.OnEnterGameButtonClick);
		     target.AddButtonClickListener(CreateRoleButton,mWindow.OnCreateRoleButtonClick);
		     target.AddButtonClickListener(DeleteButton,mWindow.OnDeleteButtonClick);
		     target.AddButtonClickListener(AllowRightButton,mWindow.OnAllowRightButtonClick);
		     target.AddButtonClickListener(AllowLeftButton,mWindow.OnAllowLeftButtonClick);
		}
	}
}
