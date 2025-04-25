using FixIntPhysics;
using FixMath;

/// <summary>
/// RenderObject会持有LogicObject ,
/// 同样的: LogicObject也会持有RenderObject ,二者会互相持有
/// 同时具有的基础属性
/// </summary>
public abstract class LogicObject
{
    private FixIntVector3 logicPos; // 逻辑位置
    private FixIntVector3 logicDir; // 朝向
    private FixIntVector3 logiclogicAngle; // 旋转角度
    private FixInt logicMoveSpeed; // 移动速度
    private FixInt _logicAxis_X; // 轴向
    private bool isActive; // 是否激活

    public FixIntVector3 LogicPos
    {
        get { return logicPos; }
        protected set { logicPos = value; }
    }

    public FixIntVector3 LogicDir
    {
        get { return logicDir; }
        protected set { logicDir = value; }
    }

    public FixIntVector3 LogiclogicAngle
    {
        get { return logiclogicAngle; }
        protected set { logiclogicAngle = value; }
    }

    public FixInt LogicMoveSpeed
    {
        get { return logicMoveSpeed; }
        protected set { logicMoveSpeed = value; }
    }

    public FixInt LogicAxis_X
    {
        get { return _logicAxis_X; }
        protected set { _logicAxis_X = value; }
    }

    public bool LsActive
    {
        get { return isActive; }
        protected set { isActive = value; }
    }

    /// <summary>
    /// 渲染对象
    /// </summary>
    public RenderObject RenderObject { get; protected set; }

    /// <summary>
    /// 定点数碰撞体
    /// </summary>
    public FixIntBoxCollider FixIntBoxCollider { get; protected set; }

    /// <summary>
    /// 逻辑对象状态
    /// </summary>
    public LogicObjectState ObjectState { get; protected set; }

    /// <summary>
    /// 逻辑对象类型
    /// </summary>
    public LogicObjectType ObjectType { get; protected set; }


    /// <summary>
    /// 逻辑对象行动状态
    /// </summary>
    public LogicObjectActionState ActionState { get; protected set; }

    public virtual void OnCreate()
    {
    }

    public virtual void OnLogicFrameUpdate()
    {
    }

    public virtual void OnDestory()
    {
    }
}

public enum LogicObjectActionState
{
    /// <summary>
    /// 待机
    /// </summary>
    Idle,

    /// <summary>
    /// 移动
    /// </summary>
    Move,

    /// <summary>
    /// 技能释放中
    /// </summary>
    SkillReleaseSkilling,

    /// <summary>
    /// 浮空中
    /// </summary>
    Float,

    /// <summary>
    /// 受击中
    /// </summary>
    Hitting,

    /// <summary>
    /// 蓄力中
    /// </summary>
    StockPileing,
}

public enum LogicObjectType
{
    Hero,
    Monster,
    Effect,
}

public enum LogicObjectState
{
    Survial, // 存活中
    Death, // 死亡
}