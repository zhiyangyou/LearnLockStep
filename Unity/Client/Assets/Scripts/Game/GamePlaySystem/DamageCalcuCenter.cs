using System;
using System.Diagnostics;
using FixMath;
using Debug = UnityEngine.Debug;

/// <summary>
/// 实现伤害计算公式
/// </summary>
public class DamageCalcuCenter {
    #region 属性字段

    /// <summary>
    /// 最大免伤率
    /// </summary>
    private static readonly FixInt s_MaxDamageReductionRate = 0.75;

    /// <summary>
    /// 思维属性转换率
    /// </summary>
    public static readonly FixInt s_BaseAttrRate = 0.004;

    public static readonly FixInt s_LevelRate = new FixInt(200);

    #endregion

    #region public

    /// <summary>
    /// 物理攻击力
    /// 武器基础物理攻击力 x (1+力量 * s_BaseAttrRate)
    /// </summary>
    /// <param name="attacker"></param>
    /// <returns></returns>
    public static FixInt GetADAttack(LogicActor attacker) {
        return attacker.AD * (FixInt.One + attacker.STR * s_BaseAttrRate);
    }

    /// <summary>
    /// 魔法攻击力
    /// 武器基础魔法攻击力 x (1+智力 *  s_BaseAttrRate)
    /// </summary>
    /// <param name="attacker"></param>
    /// <returns></returns>
    public static FixInt GetAPAttack(LogicActor attacker) {
        return attacker.AP * (FixInt.One + attacker.INT * s_BaseAttrRate);
    }


    /// <summary>
    /// 获取物理免伤率
    /// 减伤百分比 = 自身防御 / (攻击方等级 * 200 + 自身防御)
    /// 封顶75%
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static FixInt GetADReduction(LogicActor attacker, LogicActor attackTarget) {
        FixInt damageReduction = attackTarget.ADDef / (SafeLevel(attacker.Level) * s_LevelRate + attackTarget.ADDef); // 0~1
        return damageReduction > s_MaxDamageReductionRate ? s_MaxDamageReductionRate : damageReduction;
    }

    public static FixInt GetAPReduction(LogicActor attacker, LogicActor attackTarget) {
        FixInt damageReduction = attackTarget.APDef / (SafeLevel(attacker.Level) * s_LevelRate + attackTarget.APDef); // 0~1
        return damageReduction > s_MaxDamageReductionRate ? s_MaxDamageReductionRate : damageReduction;
    }

    /// <summary>
    /// 获取物理伤害暴击倍率
    /// 结果 = 暴击伤害 * (100% + 50%)
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static FixInt GetADPCTDamage(FixInt totalDamage, LogicActor attacker) {
        return totalDamage * (FixInt.One + attacker.PCT);
    }

    /// <summary>
    /// 获取魔法伤害暴击倍率
    /// 结果 = 暴击伤害 * (100% + 50%)
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static FixInt GetAPMCTDamage(FixInt totalDamage, LogicActor attacker) {
        return totalDamage * (1 + attacker.MCT);
    }


    public static FixInt CalculateDamage(SkillConfig_Damage configDamage, LogicActor attacker, LogicActor attackTarget) {
        FixInt finalDamagae;
        switch (configDamage.damageType) {
            case DamageType.None:
                finalDamagae = FixInt.Zero;
                break;
            case DamageType.AD:
                finalDamagae = (FixInt.One - GetADReduction(attacker, attackTarget)) * GetADAttack(attacker);
                break;
            case DamageType.AP:
                finalDamagae = (FixInt.One - GetAPReduction(attacker, attackTarget)) * GetAPAttack(attacker);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        // Debug.LogError($"finalDamagae:{finalDamagae} new FixInt(configDamage.damageRate) / new FixInt(100f):{new FixInt(configDamage.damageRate) / new FixInt(100f)} ");
        return finalDamagae * (new FixInt(configDamage.damageRate) / new FixInt(100f));
    }


    public static FixInt CalculateDamage(BuffConfigSO buffConfigSo, LogicActor attacker, LogicActor attackTarget) {
        FixInt finalDamagae;
        var type = buffConfigSo.targetConfig.isOpen
            ? buffConfigSo.targetConfig.damageConfig.damageType
            : buffConfigSo.damageType;
        switch (type) {
            case DamageType.None:
                finalDamagae = FixInt.Zero;
                break;
            case DamageType.AD:
                finalDamagae = (FixInt.One - GetADReduction(attacker, attackTarget)) * GetADAttack(attacker);
                break;
            case DamageType.AP:
                finalDamagae = (FixInt.One - GetAPReduction(attacker, attackTarget)) * GetAPAttack(attacker);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        if (buffConfigSo.targetConfig.isOpen) {
            return finalDamagae * (new FixInt(buffConfigSo.targetConfig.damageConfig.damageRate) / new FixInt(100f));
        }
        else {
            return finalDamagae * (new FixInt(buffConfigSo.damageRate) / new FixInt(100f));
        }
    }

    #endregion

    #region private

    private static FixInt SafeLevel(FixInt level) {
        return level <= FixInt.Zero ? FixInt.One : level;
    }

    #endregion
}