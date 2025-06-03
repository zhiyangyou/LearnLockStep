using System;
using UnityEngine;
using ZM.ZMAsset;

/// <summary>
/// 渲染对象基础:位置, 旋转...
/// RenderObject会持有LogicObject ,
/// 同样的: LogicObject也会持有RenderObject ,二者会互相持有
/// </summary>
public class RenderObject : MonoBehaviour {
    #region 属性和字段

    public LogicObject LogicObject { get; private set; }

    /// <summary>
    /// 位置插值速度?
    /// </summary>
    protected float _smoothPosSpeed => 20f;

    protected Vector2 _renderDir = Vector2.zero;
    private bool _isUpdatePosAndDir = true;

    /// <summary>
    /// 是本地玩家吗?
    /// </summary>
    private bool _isLocalPlayer = false;

    /// <summary>
    /// 预测的位置
    /// </summary>
    private Vector3 _preTargetPos;

    /// <summary>
    /// 预测的次数
    /// </summary>
    private int _curPreMoveCount = 0;

    #endregion

    #region life-cycle

    protected virtual void Update() {
        if (LogicObject != null) {
            UpdatePosAndDir();
        }
    }

    #endregion

    #region public

    public void SetLogicObject(LogicObject logicObject, bool isUpdatePosAndDir = true) {
        this.LogicObject = logicObject;
        this._isUpdatePosAndDir = isUpdatePosAndDir;
        // 初始化位置
        transform.position = logicObject.LogicPos.ToVector3();
        if (!isUpdatePosAndDir) {
            transform.localPosition = Vector3.zero;
        }
        UpdatePosAndDir();
    }

    public virtual void OnCreate() { }

    public virtual void OnRelease() { }

    public virtual void PlayAnim(AnimationClip animationClip) { }
    public virtual void PlayAnim(string animClipName) { }

    public virtual string GetCurAnimName() {
        return null;
    }

    /// <summary>
    /// 造成伤害
    /// </summary>
    /// <param name="damageValue"></param>
    /// <param name="damageSource"></param>
    public virtual void Damage(int damageValue, DamageSource damageSource) {
        var goDamageText = ZMAsset.Instantiate($"{AssetsPathConfig.Game_Prefabs}DamageItem/DamageText.prefab", null);
        goDamageText.name = $"damage text {damageValue}";
        var textItem = goDamageText.GetComponent<DamageTextItem>();
        textItem.ShowDamageText(damageValue, this);
    }

    public virtual void OnHit(GameObject goEffect, int survialTimeMS, LogicObject sourceObj) {
        if (goEffect != null) {
            var createGoEffect = GameObject.Instantiate(goEffect);
            createGoEffect.transform.position = sourceObj.RenderObject.transform.position;
            createGoEffect.transform.localScale = sourceObj.LogicAxis_X > 0 ? Vector3.one : new Vector3(-1, 1, 1);
            GameObject.Destroy(createGoEffect, survialTimeMS * 0.001f);
        }
    }

    public virtual void OnDeath() { }

    public virtual Transform GetTransParent(TransParentType transParentType) {
        return null;
    }

    /// <summary>
    /// 通用逻辑:更新方向
    /// </summary>
    public virtual void UpdateDir() {
        _renderDir.y = LogicObject.LogicAxis_X >= 0 ? 0 : 180;
        transform.localEulerAngles = _renderDir;
    }

    /// <summary>
    /// 通用逻辑:更新位置
    /// </summary>
    public virtual void UpdatePosition() {
        // 本地预测和回滚
        if (_isLocalPlayer) {
            if (_isUpdatePosAndDir) {
                if (LogicObject.hasNewLogicPos) {
                    _preTargetPos = LogicObject.LogicPos.ToVector3();
                    LogicObject.hasNewLogicPos = false;
                    _curPreMoveCount = 0; // 真正的逻辑位置从网络到达, 
                }
            }
            else {
                // 进行预测
                if (_curPreMoveCount >= LogicFrameConfig.MaxPreMoveCount) {
                    return;
                }
                // 计算预测位置的增量
                Vector3 deltaPos = LogicObject.LogicDir.ToVector3() * LogicObject.LogicMoveSpeed.RawFloat * Time.deltaTime;
                _preTargetPos += deltaPos;
                _curPreMoveCount++;
            }
            transform.position = Vector3.Lerp(transform.position, _preTargetPos, Time.deltaTime * _smoothPosSpeed);
        }
        else {
            transform.position = Vector3.Lerp(transform.position, LogicObject.LogicPos.ToVector3(), Time.deltaTime * _smoothPosSpeed);
        }
    }

    /// <summary>
    /// 展示技能立绘
    /// </summary>
    public virtual void ShowSkillPortrait(GameObject goPortrait) {
        if (goPortrait != null) {
            var go = GameObject.Instantiate(goPortrait);
            GameObject.Destroy(go, 3f);
        }
    }

    #endregion

    #region private

    private void UpdatePosAndDir() {
        if (!_isUpdatePosAndDir) {
            return;
        }
        UpdateDir();
        UpdatePosition();
    }

    #endregion
}