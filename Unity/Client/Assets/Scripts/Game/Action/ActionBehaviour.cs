using System;

public abstract class ActionBehaviour {
    #region 属性和字段

    public bool actionFinish { get; protected set; }

    /// <summary>
    /// 行动完成回调
    /// </summary>
    protected Action _callBackMoveFinish = null;

    /// <summary>
    /// 行动更新回调
    /// </summary>
    protected Action _callBackUpdateAction = null;

    #endregion

    #region public

    public abstract void OnLogicFrameUpdate();

    #endregion
}