using FixMath;
using UnityEngine;

/// <summary>
/// 硬直buff
/// </summary>
public class Buff_Stiff : BuffComposite {
    #region 属性和字段

    #endregion

    #region life-cycle

    public override void BuffDelay() { }

    public override void BuffStart() { }

    public override void BuffTrigger() {
        
    }

    public override void BuffEnd() { }

    #endregion

    #region public

    public Buff_Stiff(Buff buff) : base(buff) { }

    #endregion

    #region private

    #endregion
}