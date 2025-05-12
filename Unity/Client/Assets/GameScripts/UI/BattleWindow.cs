/*---------------------------------
 *Title:UI表现层脚本自动化生成工具
 *Author:ZM 铸梦
 *Date:2025/4/24 16:51:16
 *Description:UI 表现层，该层只负责界面的交互、表现相关的更新，不允许编写任何业务逻辑代码
 *注意:以下文件是自动生成的，再次生成不会覆盖原有的代码，会在原有的代码上进行新增，可放心使用
---------------------------------*/

using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using ZM.ZMAsset;
using ZMGC.Battle;
using ZMUIFrameWork;

public class BattleWindow : WindowBase {
    #region 属性和字段

    private HeroLogic _heroLogicActor = null;

    private List<Transform> _listSkillItemTr = new();
    private List<SkillItem> _listSkillItemCompt = new();

    #endregion

    public BattleWindowDataComponent uiCompt = null;

    #region 声明周期函数

    //调用机制与Mono Awake一致
    public override void OnAwake() {
        uiCompt = transform.GetComponent<BattleWindowDataComponent>();
        uiCompt.InitComponent(this);

        foreach (var childObj in uiCompt.SkillRootTransform) {
            Transform trChild = childObj as Transform;
            _listSkillItemTr.Add(trChild);
        }
        base.OnAwake();
    }

    //物体显示时执行
    public override void OnShow() {
        base.OnShow();
        _heroLogicActor = BattleWorld.GetExitsLogicCtrl<HeroLogicCtrl>().HeroLogic;

        // 遍历角色技能数组, 生成对应的技能按钮
        var skillIDs = BattleWorld.GetExitsDataMgr<HeroDataMgr>().GetHeroSkillIDs(HeroIDConfig.HeroID_神枪手);
        for (int i = 0; i < skillIDs.Length; i++) {
            var skillID = skillIDs[i];
            if (i >= _listSkillItemTr.Count) {
                Debug.LogError("技能数组越界, BattleWindow上没有准备那么多skill位置");
                break;
            }
            var skill = _heroLogicActor.GetSkill(skillID);
            if (skill == null) {
                Debug.LogError($"错误的skill ID:{skillID} 技能系统中没有初始化这个实例");
                continue;
            }
            var goSkillItem = ZMAsset.Instantiate($"{AssetsPathConfig.Game_Prefabs}/Item/SkillItem.prefab", _listSkillItemTr[i]);
            goSkillItem.transform.localScale = Vector3.one;
            goSkillItem.transform.localPosition = Vector3.zero;
            goSkillItem.transform.rotation = Quaternion.identity;
            var skillItemCompt = goSkillItem.GetComponent<SkillItem>();
            _listSkillItemCompt.Add(skillItemCompt);
            skillItemCompt.SetItemSkillData(_heroLogicActor.GetSkill(skillID), _heroLogicActor);
        }
        // 获取角色id数组
    }

    //物体隐藏时执行
    public override void OnHide() {
        base.OnHide();
    }

    //物体销毁时执行
    public override void OnDestroy() {
        base.OnDestroy();
    }

    #endregion

    #region API Function

    #endregion

    #region UI组件事件

    public void OnNormalAttackButtonClick() {
        _heroLogicActor.ReleaseNormalAttack(); //  普通攻击技能
    }

    #endregion
}