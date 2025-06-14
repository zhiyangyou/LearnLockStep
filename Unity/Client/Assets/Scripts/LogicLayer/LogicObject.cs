using FixIntPhysics;
using FixMath;

/// <summary>
/// RenderObject会持有LogicObject ,
/// 同样的: LogicObject也会持有RenderObject ,二者会互相持有
/// 同时具有的基础属性
/// </summary>
public abstract class LogicObject {
    private FixIntVector3 _logicPos; // 逻辑位置
    private FixIntVector3 _logicDir; // 朝向
    private FixIntVector3 _logicAngle; // 旋转角度
    private FixInt _logicMoveSpeed = (FixInt)3; // 移动速度
    private FixInt _logicAxis_X = FixInt.One; // 默认朝右
    private bool _isActive; // 是否激活
    private bool _isForceAllowMove; // 是否强制允许移动
    private bool _isForceNotAllowModifyDir; // 是否强制不允许修改位置
    public bool hasNewLogicPos = false;

    public FixIntVector3 LogicPos {
        get { return _logicPos; }
        set {
            _logicPos = value;
            hasNewLogicPos = true;
        }
    }

    public FixIntVector3 LogicDir {
        get { return _logicDir; }
        protected set { _logicDir = value; }
    }

    public FixIntVector3 LogicAngle {
        get { return _logicAngle; }
        protected set { _logicAngle = value; }
    }

    public virtual FixInt LogicMoveSpeed {
        get { return _logicMoveSpeed; }
        set { _logicMoveSpeed = value; }
    }

    public FixInt LogicAxis_X {
        get { return _logicAxis_X; }
        protected set { _logicAxis_X = value; }
    }

    public bool IsActive {
        get => _isActive;
        set => _isActive = value;
    }

    public bool IsForceAllowMove {
        get => _isForceAllowMove;
        set => _isForceAllowMove = value;
    }
    public bool IsForceNotAllowModifyDir {
        get => _isForceNotAllowModifyDir;
        set => _isForceNotAllowModifyDir = value;
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

    public virtual void OnCreate() { }

    public virtual void OnLogicFrameUpdate() { }

    public virtual void OnDestory() { }
}

public enum LogicObjectActionState {
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
    ReleasingSkill,

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

public enum LogicObjectType {
    Hero,
    Monster,
    Effect,
    None,
}

public enum LogicObjectState {
    Survial, // 存活中
    Death, // 死亡
}