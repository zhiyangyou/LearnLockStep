public abstract class BuffComposite {
    protected Buff _buff;

    private BuffComposite() { }

    protected BuffComposite(Buff buff) {
        _buff = buff;
    }

    /// <summary>
    /// buff延迟
    /// </summary>
    public abstract void BuffDelay();

    /// <summary>
    /// buff开始逻辑
    /// </summary>
    public abstract void BuffStart();

    /// <summary>
    /// buff 触发逻辑
    /// </summary>
    public abstract void BuffTrigger();

    /// <summary>
    /// buff结束
    /// </summary>
    public abstract void BuffEnd();
}