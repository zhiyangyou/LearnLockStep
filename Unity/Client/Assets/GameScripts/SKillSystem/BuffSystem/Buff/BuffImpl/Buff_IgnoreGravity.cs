public class Buff_IgnoreGravity : BuffComposite {
    #region 属性和字段

    #endregion

    #region life-cycle

    public override void BuffDelay() { }

    public override void BuffStart() { }

    public override void BuffTrigger() {
        buff.attachTarget.IsIgnoreGravity = true;
    }

    public override void BuffEnd() {
        buff.attachTarget.IsIgnoreGravity = false;
    }

    #endregion

    #region public

    public Buff_IgnoreGravity(Buff buff) : base(buff) { }

    #endregion

    #region private

    #endregion
}