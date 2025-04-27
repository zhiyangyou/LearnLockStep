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

    #endregion

    #region life-cycle

    protected virtual void Update() {
        UpdatePosition();
        UpdateDir();
    }

    #endregion

    #region public

    public void SetLogicObject(LogicObject logicObject) {
        this.LogicObject = logicObject;

        // 初始化位置
        transform.position = logicObject.LogicPos.ToVector3();
    }

    public virtual void OnCreate() { }

    public virtual void OnRelease() { }

    public virtual void PlayAnim(AnimationClip animationClip) { }


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

    #endregion

    #region private

    /// <summary>
    /// 通用逻辑:更新方向
    /// </summary>
    private void UpdateDir() {
        _renderDir.x = LogicObject.LogicAxis_X >= 0 ? 0 : -20;
        _renderDir.y = LogicObject.LogicAxis_X >= 0 ? 0 : 180;
        transform.localEulerAngles = _renderDir;
    }

    /// <summary>
    /// 通用逻辑:更新位置
    /// </summary>
    private void UpdatePosition() {
        // 对逻辑位置做插值动画, 使其渲染对象移动看起来比较流畅 
        // TODO 不理解... 2025年4月23日17:07:36
        // 上述代码可能导致延迟累积。因为Lerp的起点是上一渲染帧的位置，而非最新的逻辑位置，在高延迟或高速移动场景中，物体可能始终“追赶”逻辑位置。
        // 
        transform.position = Vector3.Lerp(transform.position, LogicObject.LogicPos.ToVector3(), Time.deltaTime * _smoothPosSpeed);
    }

    #endregion
}