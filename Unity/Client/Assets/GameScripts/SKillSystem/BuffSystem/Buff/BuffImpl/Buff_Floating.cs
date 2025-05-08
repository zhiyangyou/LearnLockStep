using FixMath;
using UnityEngine;

/// <summary>
/// 浮空buff
/// </summary>
public class Buff_Floating : BuffComposite {
    public Buff_Floating(Buff buff) : base(buff) { }

    public override void BuffDelay() { }

    public override void BuffStart() { }

    public override void BuffTrigger() {
        var paramList = buff.BuffConfigSo.paramsList;
        if (paramList != null && paramList.Count > 0) {
            FixInt floatingValue = paramList[0].Value;
            buff.attachTarget.AddRisingForce(floatingValue, buff.BuffConfigSo.DurationMS);
        }
        else {
            Debug.LogError("Buff_Floating buff的参数是空");
        }
    }

    public override void BuffEnd() { }
}