using System;
using System.Collections;
using System.Collections.Generic;
using FixMath;
using UnityEngine;
using ZM.ZMAsset;

public class LogicTimerManager : Singleton<LogicTimerManager> {
    #region 属性和字段

    private List<TimerBehaviour> _listTimers = new();

    #endregion

    #region public

    public void DelayCallOnce(FixInt delayTimeS, Action onTimerFinish) {
        DelayCall(delayTimeS, onTimerFinish, 1);
    }

    public void DelayCall(FixInt delayTimeS, Action onTimerFinish, int loopCount) {
        LogicTimer timer = new LogicTimer(delayTimeS, onTimerFinish, loopCount);
        _listTimers.Add(timer);
    }

    /// <summary>
    /// 逻辑帧更新
    /// </summary>
    public void OnLogicFrameUpdate() {
        // 移除已经完成的
        for (int i = _listTimers.Count - 1; i >= 0; i--) {
            var timer = _listTimers[i];
            if (timer.timerFinish) {
                RemoveTimer(timer);
            }
        }

        // 更新逻辑帧
        foreach (var actionBehaviour in _listTimers) {
            actionBehaviour.OnLogicFrameUpdate();
        }
    }

    public void RemoveTimer(TimerBehaviour actionBehaviour) {
        _listTimers.Remove(actionBehaviour);
    }

    public void OnDestory() {
        _listTimers.Clear();
    }

    #endregion
}