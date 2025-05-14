using FixMath;

/// <summary>
/// 实现伤害计算公式
/// </summary>
public class DamageCalcuCenter {
    /// <summary>
    /// 最大免伤率
    /// </summary>
    private static readonly FixInt s_MaxDamageReductionRate = 0.75;

    /// <summary>
    /// 思维属性转换率
    /// </summary>
    public static readonly FixInt s_BaseAttrRate = 0.004;

    /// <summary>
    /// 物理攻击力
    /// 武器基础物理攻击力 x (1+力量 * s_BaseAttrRate)
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static FixInt GetADAttack(LogicActor target) {
        return target.AD * (1 + target.STR * s_BaseAttrRate);
    }

    /// <summary>
    /// 魔法攻击力
    /// 武器基础魔法攻击力 x (1+智力 *  s_BaseAttrRate)
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static FixInt GetAPAttack(LogicActor target) {
        return target.AP * (1 + target.INT * s_BaseAttrRate);
    }


    /// <summary>
    /// 获取物理免伤率
    /// 减伤百分比 = 自身防御 / (攻击方等级 * 200 + 自身防御)
    /// 封顶75%
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static FixInt GetADReduction(LogicActor attacker, LogicActor attackTarget) {
        FixInt damageReduction = attackTarget.ADDef / (attacker.Level * 200 + attackTarget.ADDef); // 0~1
        return damageReduction > s_MaxDamageReductionRate ? s_MaxDamageReductionRate : damageReduction;
    }

    public static FixInt GetAPReduction(LogicActor attacker, LogicActor attackTarget) {
        FixInt damageReduction = attackTarget.APDef / (attacker.Level * 200 + attackTarget.APDef); // 0~1
        return damageReduction > s_MaxDamageReductionRate ? s_MaxDamageReductionRate : damageReduction;
    }
}