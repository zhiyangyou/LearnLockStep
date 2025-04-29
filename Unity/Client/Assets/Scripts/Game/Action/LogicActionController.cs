using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZM.ZMAsset;

/// <summary>
/// 控制不同角色的行动
/// </summary>
public class LogicActionController : Singleton<LogicActionController> {
    #region 属性和字段

    private List<ActionBehaviour> _listActions = new();

    #endregion

    #region public

    public void RunAction(ActionBehaviour actionBehaviour) { }

    public void OnLogicFrameUpdate() { }

    public void RemoveAction(ActionBehaviour actionBehaviour) { }

    public void OnDestory() { }

    #endregion
}