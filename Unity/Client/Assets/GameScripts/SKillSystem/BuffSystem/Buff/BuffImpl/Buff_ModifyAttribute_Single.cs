using System;
using System.Linq;
using FixMath;

/// <summary>
/// 属性修改buff
/// </summary>
public class Buff_ModifyAttribute_Single : BuffComposite {
    #region 属性字段

    private FixInt _buffConfigValue = 0;

    #endregion


    #region public

    public Buff_ModifyAttribute_Single(Buff buff) : base(buff) { }

    public override void BuffDelay() { }

    public override void BuffStart() {
        var paramList = buff.BuffConfigSo.paramsList;
        if (paramList is { Count: > 0 }) {
            _buffConfigValue = paramList[0].Value;
        }
    }

    public override void BuffTrigger() {
        ModifyAttribute(_buffConfigValue);
    }

    public override void BuffEnd() {
        ModifyAttribute(-_buffConfigValue);
    }

    #endregion

    #region private

    private void ModifyAttribute(FixInt value) {
        switch (buff.BuffConfigSo.buffType) {
            case BuffType.MoveSpeed_Modify_Single:
                buff.attachTarget.LogicMoveSpeed += value;
                break;
            case BuffType.AllowMove:
                break;
        }
    }

    #endregion
}