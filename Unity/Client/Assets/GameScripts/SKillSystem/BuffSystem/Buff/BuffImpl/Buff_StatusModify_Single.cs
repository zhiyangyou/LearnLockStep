using System;
using FixMath;
using UnityEditor.PackageManager;

public class Buff_StatusModify_Single : BuffComposite {
    #region 属性字段

    private FixInt _buffConfigValue = 0;

    #endregion


    #region public

    public Buff_StatusModify_Single(Buff buff) : base(buff) { }

    public override void BuffDelay() { }

    public override void BuffStart() {
        var paramList = buff.BuffConfigSo.paramsList;
        if (paramList is { Count: > 0 }) {
            _buffConfigValue = paramList[0].Value;
        }
    }

    public override void BuffTrigger() {
        ModifyStatus(true);
    }

    public override void BuffEnd() {
        ModifyStatus(false);
    }

    #endregion

    #region private

    private void ModifyStatus(bool modify) {
        switch (buff.BuffConfigSo.buffType) {
            case BuffType.AllowMove:
                buff.attachTarget.IsForceAllowMove = modify;
                break;
            case BuffType.NotAllowModifyDir:
                buff.attachTarget.IsForceNotAllowModifyDir = modify;
                break;
        }
    }

    #endregion
}