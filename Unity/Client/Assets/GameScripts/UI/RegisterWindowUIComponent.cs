/*---------------------------------
 *Title:UI自动化组件查找代码生成工具
 *Author:铸梦
 *Date:2025/5/21 10:02:33
 *Description:变量需要以[Text]括号加组件类型的格式进行声明，然后右键窗口物体—— 一键生成UI组件查找脚本即可
 *注意:以下文件是自动生成的，任何手动修改都会被下次生成覆盖,若手动修改后,尽量避免自动生成
---------------------------------*/
using UnityEngine.UI;
using UnityEngine;

namespace ZMUIFrameWork
{
	public class RegisterWindowUIComponent
	{
		public   Button  RegisterButton;

		public   InputField  PasswordInputField;

		public   InputField  AccountInputField;

		public   Button  CloseButton;

		public  void InitComponent(WindowBase target)
		{
		     //组件查找
		     RegisterButton =target.transform.Find("UIContent/[Button]Register").GetComponent<Button>();
		     PasswordInputField =target.transform.Find("UIContent/[InputField]Password").GetComponent<InputField>();
		     AccountInputField =target.transform.Find("UIContent/[InputField]Account").GetComponent<InputField>();
		     CloseButton =target.transform.Find("UIContent/[Button]Close").GetComponent<Button>();
	
	
		     //组件事件绑定
		     RegisterWindow mWindow=(RegisterWindow)target;
		     target.AddButtonClickListener(RegisterButton,mWindow.OnRegisterButtonClick);
		     target.AddInputFieldListener(PasswordInputField,mWindow.OnPasswordInputChange,mWindow.OnPasswordInputEnd);
		     target.AddInputFieldListener(AccountInputField,mWindow.OnAccountInputChange,mWindow.OnAccountInputEnd);
		     target.AddButtonClickListener(CloseButton,mWindow.OnCloseButtonClick);
		}
	}
}
