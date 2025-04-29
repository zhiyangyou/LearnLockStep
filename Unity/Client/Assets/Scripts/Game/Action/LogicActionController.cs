using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZM.ZMAsset;

/// <summary>
/// 控制不同角色的行动
/// </summary>
public class LogicActionController : Singleton<LogicActionController> {
    #region 属性和字段

    private List<ActionBehaviour> _listDoingActions = new();

    #endregion

    #region public

    public void RunAction(ActionBehaviour actionBehaviour) {
        if (actionBehaviour == null) {
            Debug.LogError("RunAction argument is null ");
            return;
        }
        actionBehaviour.actionFinish = false;
        _listDoingActions.Add(actionBehaviour);
    }

    /// <summary>
    /// 逻辑帧更新
    /// </summary>
    public void OnLogicFrameUpdate() {
        // 移除已经完成的
        for (int i = _listDoingActions.Count - 1; i >= 0; i--) {
            var actionBehaviour = _listDoingActions[i];
            if (actionBehaviour.actionFinish) {
                RemoveAction(actionBehaviour);
            }
        }

        // 更新逻辑帧
        foreach (var actionBehaviour in _listDoingActions) {
            actionBehaviour.OnLogicFrameUpdate();
        }
    }

    public void RemoveAction(ActionBehaviour actionBehaviour) {
        _listDoingActions.Remove(actionBehaviour);
    }

    public void OnDestory() {
        _listDoingActions.Clear();
    }

    #endregion
}