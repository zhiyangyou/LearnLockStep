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

    #endregion

    #region life-cycle

    protected virtual void Update() {
        UpdatePosAndDir();
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
        var goDamageText = ZMAsset.Instantiate($"{AssetsPathConfig.Game_Prefabs}/DamageItem/DamageText.prefab", null);
        goDamageText.name = $"damage text {damageValue}";
        var textItem = goDamageText.GetComponent<DamageTextItem>();
        textItem.ShowDamageText(damageValue, this);
    }

    public virtual void OnHit(GameObject goEffect, int survialTimeMS, LogicActor sourceActor) {
        if (goEffect != null) {
            var createGoEffect = GameObject.Instantiate(goEffect);
            createGoEffect.transform.position = this.transform.position;
            // goHitEffect.transform.position = sourceActor.RenderObject.transform.position;
            createGoEffect.transform.localScale = sourceActor.LogicAxis_X > 0 ? Vector3.one : new Vector3(-1, 1, 1);
            GameObject.Destroy(createGoEffect, survialTimeMS * 0.001f);
        }
    }

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
        transform.position = Vector3.Lerp(transform.position, LogicObject.LogicPos.ToVector3(), Time.deltaTime * _smoothPosSpeed);
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