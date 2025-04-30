using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillItem : MonoBehaviour {
    #region 属性和字段

    [SerializeField] private TextMeshProUGUI _txtCD;
    [SerializeField] private Image _imgIcon;
    [SerializeField] private Image _imgMaskImage;
    [SerializeField] private SKillItem_JoyStick _skillItemJoyStick;

    private Skill _skillData;
    private LogicActor _skillCreater;

    #endregion

    #region life-cycle

    private void Update() { }

    #endregion

    #region public

    /// <summary>
    /// 设置技能数据
    /// </summary>
    /// <param name="skillConfig"></param>
    /// <param name="skillCreater"></param>
    public void SetItemSkillData(Skill skillData, LogicActor skillCreater) {
        this._skillCreater = skillCreater;
        this._skillData = skillData;
        _skillItemJoyStick.InitSkillData(GetSkillGuideType(skillData.SkillCfgConfig.SkillType), skillData.SkillID, skillData.SkillCfgConfig.skillGuideRange);
        _skillItemJoyStick.OnReleaseSkill += OnTriggerSkill;
        _skillItemJoyStick.OnSkillGuide += OnSkillGuide;

        // 初始化UI
        _imgIcon.sprite = skillData.SkillCfgConfig.skillIcon;
        _imgMaskImage.gameObject.SetActive(false);
        _txtCD.gameObject.SetActive(false);
    }


    public SKillGuideType GetSkillGuideType(SkillType skillType) {
        SKillGuideType ret = SKillGuideType.Click;
        switch (skillType) {
            case SkillType.None:
            case SkillType.Chat:
            case SkillType.Ballistic:
                ret = SKillGuideType.Click;
                break;
            case SkillType.StockPile:
                ret = SKillGuideType.LongPress;
                break;
            case SkillType.PosGuide:
                ret = SKillGuideType.Position;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(skillType), skillType, null);
        }
        return ret;
    }


    public void OnDestroy() {
        _skillItemJoyStick.OnReleaseSkill -= OnTriggerSkill;
        _skillItemJoyStick.OnSkillGuide -= OnSkillGuide;
    }

    #endregion

    #region private

    /// <summary>
    /// 
    /// </summary>
    /// <param name="skillguide">技能引导类型</param>
    /// <param name="iscancel">是否取消</param>
    /// <param name="skillpos">引导位置</param>
    /// <param name="skillid">技能id</param>
    /// <param name="skilldirdis">技能半径距离</param>
    private void OnSkillGuide(SKillGuideType skillguide, bool iscancel, Vector3 skillpos, int skillid, float skilldirdis) { }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="skillguide"></param>
    /// <param name="skillpos"></param>
    /// <param name="skillid"></param>
    private void OnTriggerSkill(SKillGuideType skillguide, Vector3 skillpos, int skillid) {
        switch (skillguide) {
            case SKillGuideType.Click: {
                _skillCreater.ReleaseSkill(skillid);
            }
                break;
            case SKillGuideType.LongPress: {
                Debug.LogError("TODO 蓄力技能");
            }
                break;
            case SKillGuideType.Position:
                Debug.LogError("TODO 引导到位置进行释放");
                break;
            case SKillGuideType.Dirction:
                Debug.LogError("TODO 引导方向");
                break;
            default:
            case SKillGuideType.None:
                throw new ArgumentOutOfRangeException(nameof(skillguide), skillguide, null);
        }
    }

    #endregion
}