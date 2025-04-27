using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class World
{
    public  void AddLogicCtrl(ILogicBehaviour behaviour)
    {
        mLogicBehaviourDic.TryAdd(behaviour.GetType().Name, behaviour);
        behaviour.OnCreate();
    }

    public  void AddDataMgr(IDataBehaviour behaviour)
    {
        mDataBehaviourDic.TryAdd(behaviour.GetType().Name, behaviour);
        behaviour.OnCreate();
    }
    public  void AddMsgMgr(IMsgBehaviour behaviour)
    {
        mMsgBehaviourDic.TryAdd(behaviour.GetType().Name, behaviour);
        behaviour.OnCreate();
    }
}
