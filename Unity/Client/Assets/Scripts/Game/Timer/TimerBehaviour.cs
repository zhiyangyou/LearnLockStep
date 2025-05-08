using System;

public abstract class TimerBehaviour {
    #region 属性和字段

    public bool timerFinish { get; set; }

    /// <summary>
    /// 行动完成回调
    /// </summary>
    protected Action _onTimerFinish = null;

    /// <summary>
    /// 行动更新回调
    /// </summary>
    protected Action _onTimerUpdate = null;

    #endregion

    #region public

    public abstract void OnLogicFrameUpdate();

    /// <summary>
    /// 行动完成
    /// </summary>
    public abstract void OnTimerFinish();

    #endregion
}