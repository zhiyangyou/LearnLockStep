/*---------------------------------
 *Title:UI自动化组件查找代码生成工具
 *Author:铸梦
 *Date:2025/5/21 9:31:25
 *Description:变量需要以[Text]括号加组件类型的格式进行声明，然后右键窗口物体—— 一键生成UI组件查找脚本即可
 *注意:以下文件是自动生成的，任何手动修改都会被下次生成覆盖,若手动修改后,尽量避免自动生成
---------------------------------*/
using UnityEngine.UI;
using UnityEngine;

namespace ZMUIFrameWork
{
	public class LoginWindowUIComponent
	{
		public   Button  CloseButton;

		public   Transform  CenterButtonRootTransform;

		public   Button  ServerSelectButton;

		public   InputField  AccountInputField;

		public   InputField  PasswordInputField;

		public   Button  EnterGameButton;

		public   Text  VersionText;

		public   Button  PublishButton;

		public   Button  ClickEnterButton;

		public   Button  RegisterAccountButton;

		public  void InitComponent(WindowBase target)
		{
		     //组件查找
		     CloseButton =target.transform.Find("UIContent/[Button]Close").GetComponent<Button>();
		     CenterButtonRootTransform =target.transform.Find("UIContent/[Transform]CenterButtonRoot").transform;
		     ServerSelectButton =target.transform.Find("UIContent/[Transform]CenterButtonRoot/[Button]ServerSelect").GetComponent<Button>();
		     AccountInputField =target.transform.Find("UIContent/[Transform]CenterButtonRoot/[InputField]Account").GetComponent<InputField>();
		     PasswordInputField =target.transform.Find("UIContent/[Transform]CenterButtonRoot/[InputField]Password").GetComponent<InputField>();
		     EnterGameButton =target.transform.Find("UIContent/[Transform]CenterButtonRoot/[Button]EnterGame").GetComponent<Button>();
		     VersionText =target.transform.Find("UIContent/Rightup/Version/[Text]Version").GetComponent<Text>();
		     PublishButton =target.transform.Find("UIContent/[Button]Publish").GetComponent<Button>();
		     ClickEnterButton =target.transform.Find("UIContent/[Button]ClickEnter").GetComponent<Button>();
		     RegisterAccountButton =target.transform.Find("UIContent/[Button]RegisterAccount").GetComponent<Button>();
	
	
		     //组件事件绑定
		     LoginWindow mWindow=(LoginWindow)target;
		     target.AddButtonClickListener(CloseButton,mWindow.OnCloseButtonClick);
		     target.AddButtonClickListener(ServerSelectButton,mWindow.OnServerSelectButtonClick);
		     target.AddInputFieldListener(AccountInputField,mWindow.OnAccountInputChange,mWindow.OnAccountInputEnd);
		     target.AddInputFieldListener(PasswordInputField,mWindow.OnPasswordInputChange,mWindow.OnPasswordInputEnd);
		     target.AddButtonClickListener(EnterGameButton,mWindow.OnEnterGameButtonClick);
		     target.AddButtonClickListener(PublishButton,mWindow.OnPublishButtonClick);
		     target.AddButtonClickListener(ClickEnterButton,mWindow.OnClickEnterButtonClick);
		     target.AddButtonClickListener(RegisterAccountButton,mWindow.OnRegisterAccountButtonClick);
		}
	}
}
