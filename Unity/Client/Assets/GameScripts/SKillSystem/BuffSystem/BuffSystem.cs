using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZM.ZMAsset;


/// <summary>
/// 管理buff, 主要功能如下
/// 生命周期调用 :释放, 移除, 更新
/// 对象保存: 增删改查 
/// </summary>
public class BuffSystem : Singleton<BuffSystem> {
    #region 属性字段

    private List<Buff> _listBuff = new(); // 所有buff

    #endregion


    #region life-cycle

    public void OnCreate() { }

    public void OnLogicFrameUpdate() {
        // 必须是方向for循环, 因为在buff的Update中,可能出现RemoveBuff的调用, 导致for循环遍历越界
        for (int i = _listBuff.Count - 1; i >= 0; i--) {
            _listBuff[i].OnLogicFrameUpdate();
        }
    }

    public void OnDestory() {
        // 必须是方向for循环, 因为在buff的Update中,可能出现RemoveBuff的调用, 导致for循环遍历越界
        for (int i = _listBuff.Count - 1; i >= 0; i--) {
            _listBuff[i].OnDestory();
        }
        _listBuff.Clear();
    }

    #endregion

    #region public

    public void RemoveBuff(Buff buff) {
        if (buff == null) {
            Debug.LogError("RemoveBuff 失败,传入的buff 是null");
            return;
        }
        if (!_listBuff.Remove(buff)) {
            Debug.LogError("RemoveBuff 失败, 在_listBuff中不包含buff");
        }
    }

    /// <summary>
    /// 附加buff
    /// </summary>
    /// <param name="buffID"></param>
    /// <param name="releaser"></param>
    /// <param name="attachTarget"></param>
    /// <param name="skill"></param>
    /// <param name="paramObjs"></param>
    /// <returns></returns>
    public Buff AttachBuff(int buffID,
        LogicActor releaser,
        LogicActor attachTarget,
        Skill skill,
        object[] paramObjs
    ) {
        if (buffID <= 0) {
            Debug.LogError($"非法的buffID :{buffID}");
            return null;
        }
        var buff = new Buff(buffID, releaser, attachTarget, skill, paramObjs);
        buff.OnCreate();
        _listBuff.Add(buff);
        // Debug.LogError($"add buff {buffID}");
        return buff;
    }

    #endregion

    #region private

    #endregion
}