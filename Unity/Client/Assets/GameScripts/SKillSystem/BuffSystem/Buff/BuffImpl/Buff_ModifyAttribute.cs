/// <summary>
/// 属性修改buff
/// </summary>
public class Buff_ModifyAttribute : BuffComposite {
    #region 属性字段

    private BuffCollider _buffCollider;

    #endregion


    #region public

    public Buff_ModifyAttribute(Buff buff) : base(buff) { }

    public override void BuffDelay() {
        // throw new System.NotImplementedException();
    }

    public override void BuffStart() {
        // throw new System.NotImplementedException();
        // 生成碰撞体
        _buffCollider = new(buff);
    }

    public override void BuffTrigger() {
        // throw new System.NotImplementedException();
        // 检测碰撞体
    }

    public override void BuffEnd() {
        // throw new System.NotImplementedException();
        // 销毁碰撞体
        // 清理列表
    }

    #endregion

    #region private

    #endregion
}