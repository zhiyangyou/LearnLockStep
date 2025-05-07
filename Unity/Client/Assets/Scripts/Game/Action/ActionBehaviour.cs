using System;

public abstract class ActionBehaviour {
    #region 属性和字段

    public bool actionFinish { get; set; }

    /// <summary>
    /// 行动完成回调
    /// </summary>
    protected Action _onMoveFinish = null;

    /// <summary>
    /// 行动更新回调
    /// </summary>
    protected Action _onUpdateAction = null;

    #endregion

    #region public

    public abstract void OnLogicFrameUpdate();

    /// <summary>
    /// 行动完成
    /// </summary>
    public abstract void OnActionFinish();

    #endregion
}