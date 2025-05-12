using System;
using System.Collections;
using System.Collections.Generic;
using FixMath;
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
    private HeroRender _heroRender;

    private bool _isCD = false;
    private long _enterCDLogicFrame = 0;
    private float _cdTimeS = 0f;

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
        _skillCreater = skillCreater;
        _skillData = skillData;
        _heroRender = skillCreater.RenderObject as HeroRender;
        _skillItemJoyStick.InitSkillData(GetSkillGuideType(skillData.SkillCfgConfig.SkillType), skillData.SkillID, skillData.SkillCfgConfig.skillGuideRange);
        _skillItemJoyStick.OnReleaseSkill += OnTriggerSkill;
        _skillItemJoyStick.OnSkillGuide += OnSkillGuide;
        _skillItemJoyStick.OnSkillCancel += OnSkillCancle;

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
        _skillItemJoyStick.OnSkillCancel -= OnSkillCancle;
    }

    #endregion

    #region private

    private void OnSkillCancle(int skillid) {
        _heroRender.OnGuideRelease();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="skillguide">技能引导类型</param>
    /// <param name="isPress">是否取消</param>
    /// <param name="skillPos">引导位置</param>
    /// <param name="skillId">技能id</param>
    /// <param name="skilldirdis">技能半径距离</param>
    private void OnSkillGuide(SKillGuideType skillguide, bool isPress, Vector3 skillPos, int skillId, float skilldirdis) {
        // Debug.LogError($"Skill Guide ... {skillguide}");
        if (skillguide == SKillGuideType.LongPress) {
            // Debug.LogError($"OnSkillGuide long press 触发{skillid} {Time.frameCount} ");
            _skillCreater.ReleaseSkill(skillId, OnReleaseSkillResult);
        }
        else if (skillguide == SKillGuideType.Position) {
            _heroRender.UpdateSkillGuide(SKillGuideType.Position, skillId, isPress, skillPos, skilldirdis);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="skillguide"></param>
    /// <param name="skillPos"></param>
    /// <param name="skillId"></param>
    private void OnTriggerSkill(SKillGuideType skillguide, Vector3 skillPos, int skillId) {
        switch (skillguide) {
            case SKillGuideType.Click: {
                // Debug.LogError($"触发 click 技能:{skillid}");
                _skillCreater.ReleaseSkill(skillId, OnReleaseSkillResult);
            }
                break;
            case SKillGuideType.LongPress: {
                // Debug.LogError($"触发 蓄力技能:{skillid} {Time.frameCount} ");
                _skillCreater.TriggerStockPileSkill(skillId);
            }
                break;
            case SKillGuideType.Position: {
                skillPos.y = 0; // 确保引导的位置在地面上
                _skillCreater.ReleaseSkill(skillId, null, _skillCreater.LogicPos + new FixIntVector3(skillPos));
                // Debug.LogError("TODO 引导位置技能 释放");
                _heroRender.OnGuideRelease();
            }
                break;
            case SKillGuideType.Dirction:
                Debug.LogError("TODO 引导方向");
                break;
            default:
            case SKillGuideType.None:
                throw new ArgumentOutOfRangeException(nameof(skillguide), skillguide, null);
        }
    }


    private void OnReleaseSkillResult(bool isSuccess) {
        if (isSuccess) {
            EnterSkillCD();
        }
    }

    private void EnterSkillCD() {
        _enterCDLogicFrame = LogicFrameConfig.LogicFrameID;

        float _alreadyCDTimeS = (LogicFrameConfig.LogicFrameID - _enterCDLogicFrame) * LogicFrameConfig.LogicFrameInterval;
        float leftCDTimeS = _skillData.SkillCfgConfig.CDTimeS - _alreadyCDTimeS;

        _txtCD.gameObject.SetActive(true);
        _imgMaskImage.gameObject.SetActive(true);
        _cdTimeS = _skillData.SkillCfgConfig.skillCDTimeMS / 1000f;

        _txtCD.text = $"{leftCDTimeS:F1}";

        float updateTime = 0.01f; // 每隔0.1秒更新UI
        LogicTimer timerCD = null;
        timerCD = LogicTimerManager.Instance.DelayCall(LogicFrameConfig.LogicFrameInterval, () => {
            // Debug.LogError($"timer finish");
            float _alreadyCDTimeS = (LogicFrameConfig.LogicFrameID - _enterCDLogicFrame) * LogicFrameConfig.LogicFrameInterval;
            float leftCDTimeS = _skillData.SkillCfgConfig.CDTimeS - _alreadyCDTimeS;
            _txtCD.text = $"{leftCDTimeS:F1}";
            // Debug.LogError($"_alreadyCDTimeS:{_alreadyCDTimeS}");
            var isComplete = _alreadyCDTimeS >= _skillData.SkillCfgConfig.CDTimeS;
            var completeProgress = Mathf.Clamp01(_alreadyCDTimeS / _skillData.SkillCfgConfig.CDTimeS);
            _txtCD.gameObject.SetActive(!isComplete);
            _imgMaskImage.gameObject.SetActive(!isComplete);
            if (!isComplete) {
                _imgMaskImage.fillAmount = 1f - completeProgress;
            }
            if (isComplete) {
                // Debug.LogError("移除timer");
                timerCD.Complete();
            }
        }, -1);
        // Debug.LogError($"timer启动:{timerCD!=null}");
    }

    #endregion
}