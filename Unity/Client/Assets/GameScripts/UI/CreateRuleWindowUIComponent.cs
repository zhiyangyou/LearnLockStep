/*---------------------------------
 *Title:UI自动化组件查找代码生成工具
 *Author:铸梦
 *Date:2024/12/24 21:33:12
 *Description:变量需要以[Text]括号加组件类型的格式进行声明，然后右键窗口物体—— 一键生成UI组件查找脚本即可
 *注意:以下文件是自动生成的，任何手动修改都会被下次生成覆盖,若手动修改后,尽量避免自动生成
---------------------------------*/
using UnityEngine.UI;
using UnityEngine;

namespace ZMUIFrameWork
{
	public class CreateRuleWindowUIComponent
	{
		public   InputField  InputNameInputField;

		public   Button  EnterBtnButton;

		public   Button  Role1Button;

		public   Button  Role2Button;

		public  void InitComponent(WindowBase target)
		{
		     //组件查找
		     InputNameInputField =target.transform.Find("UIContent/RightContent/[InputField]InputName").GetComponent<InputField>();
		     EnterBtnButton =target.transform.Find("UIContent/RightContent/[Button]EnterBtn").GetComponent<Button>();
		     Role1Button =target.transform.Find("UIContent/LeftContent/Layout/[Button]Role1").GetComponent<Button>();
		     Role2Button =target.transform.Find("UIContent/LeftContent/Layout/[Button]Role2").GetComponent<Button>();
	
	
		     //组件事件绑定
		     CreateRuleWindow mWindow=(CreateRuleWindow)target;
		     target.AddInputFieldListener(InputNameInputField,mWindow.OnInputNameInputChange,mWindow.OnInputNameInputEnd);
		     target.AddButtonClickListener(EnterBtnButton,mWindow.OnEnterBtnButtonClick);
		     target.AddButtonClickListener(Role1Button,mWindow.OnRole1ButtonClick);
		     target.AddButtonClickListener(Role2Button,mWindow.OnRole2ButtonClick);
		}
	}
}
