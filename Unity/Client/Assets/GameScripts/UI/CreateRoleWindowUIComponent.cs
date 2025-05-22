/*---------------------------------
 *Title:UI自动化组件查找代码生成工具
 *Author:铸梦
 *Date:2025/5/22 14:27:15
 *Description:变量需要以[Text]括号加组件类型的格式进行声明，然后右键窗口物体—— 一键生成UI组件查找脚本即可
 *注意:以下文件是自动生成的，任何手动修改都会被下次生成覆盖,若手动修改后,尽量避免自动生成
---------------------------------*/
using UnityEngine.UI;
using UnityEngine;

namespace ZMUIFrameWork
{
	public class CreateRoleWindowUIComponent
	{
		public   Transform  RolePortraitRootTransform;

		public   Text   CurSelectRoleIDText;

		public   Button  EnterGameButton;

		public   InputField  NameInputField;

		public   Button  CloseButton;

		public   Transform  ContentTransform;

		public   GameObject   ItemRoleSelectGameObject;

		public  void InitComponent(WindowBase target)
		{
		     //组件查找
		     RolePortraitRootTransform =target.transform.Find("UIContent/[Transform]RolePortraitRoot").transform;
		      CurSelectRoleIDText =target.transform.Find("UIContent/RightConter/[Text] CurSelectRoleID").GetComponent<Text>();
		     EnterGameButton =target.transform.Find("UIContent/RightConter/[Button]EnterGame").GetComponent<Button>();
		     NameInputField =target.transform.Find("UIContent/RightConter/[InputField]Name").GetComponent<InputField>();
		     CloseButton =target.transform.Find("UIContent/HroizontalUp/[Button]Close").GetComponent<Button>();
		     ContentTransform =target.transform.Find("UIContent/LeftVertical/Scroll View/Viewport/[Transform]Content").transform;
		      ItemRoleSelectGameObject =target.transform.Find("UIContent/LeftVertical/Scroll View/Viewport/[Transform]Content/[GameObject] ItemRoleSelect").gameObject;
	
	
		     //组件事件绑定
		     CreateRoleWindow mWindow=(CreateRoleWindow)target;
		     target.AddButtonClickListener(EnterGameButton,mWindow.OnEnterGameButtonClick);
		     target.AddInputFieldListener(NameInputField,mWindow.OnNameInputChange,mWindow.OnNameInputEnd);
		     target.AddButtonClickListener(CloseButton,mWindow.OnCloseButtonClick);
		}
	}
}
