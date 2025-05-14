using System.Collections;
using System.Collections.Generic;
using FixMath;
using UnityEngine;

public partial class LogicActor {
    protected FixInt level; //等级
    protected string name; //名称
    protected FixInt id; //唯一id
    protected FixInt type; //类型

    #region 内部属性

    protected FixInt hp; // 血量
    protected FixInt mp; // 法力值
    protected FixInt ap; // 魔法攻击力
    protected FixInt ad; // 物理攻击力
    protected FixInt adDef; // 物理防御
    protected FixInt apDef; // 魔法防御
    protected FixInt pct; // 物理暴击
    protected FixInt mct; // 魔法暴击

    protected float adPctRate; // 物理暴击倍率
    protected float apMctRate; // 魔法暴击倍率

    protected FixInt str; // 力量
    protected FixInt sta; // 体力
    protected FixInt Int; // 智力
    protected FixInt spi; // 精神
    protected FixInt agl; // 敏捷

    protected FixInt atkRange; // 攻击距离，用于区别远程怪物和近战怪物的攻击距离
    protected FixInt searchDisRange; // 搜寻距离 用于出生后首次搜寻目标进行进行追击

    #endregion


    #region 战斗时增益属性(受到buff增加的值)

    protected FixInt add_ap; // 魔法攻击力
    protected FixInt add_ad; // 物理攻击力
    protected FixInt add_adDef; // 物理防御
    protected FixInt add_apDef; // 魔法防御
    protected FixInt add_pct; // 物理暴击
    protected FixInt add_mct; // 魔法暴击

    protected FixInt add_adPctRate; // 物理暴击倍率
    protected FixInt add_apMctRate; // 魔法暴击倍率

    protected FixInt add_str; // 力量
    protected FixInt add_sta; // 体力
    protected FixInt add_Int; // 智力
    protected FixInt add_spi; // 精神
    protected FixInt add_agl; // 敏捷

    #endregion


    #region 公开属性

    public FixInt Level => level; // 等级
    public FixInt HP => hp; // 血量
    public FixInt MP => mp; // 法力值
    public FixInt AP => ap + add_ap; // 魔法攻击力
    public FixInt AD => ad + add_ad; // 物理攻击力
    public FixInt ADDef => adDef + add_adDef; // 物理防御
    public FixInt APDef => apDef + add_apDef; // 魔法防御
    public FixInt PCT => pct + add_pct; // 物理暴击
    public FixInt MCT => mct + add_mct; // 魔法暴击
    public FixInt ADPctRate => adPctRate + add_adPctRate; // 物理暴击倍率
    public FixInt APMctRate => apMctRate + add_apMctRate; // 魔法暴击倍率
    public FixInt STR => str + add_sta; // 力量
    public FixInt STA => sta + add_sta; // 体力
    public FixInt INT => Int + add_Int; // 智力
    public FixInt SPI => spi + add_spi; // 精神
    public FixInt AGL => agl + add_agl; // 敏捷

    #endregion
}