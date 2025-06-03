using System.Collections;
using System.Collections.Generic;
using FixMath;
using UnityEngine;

public class HeroLogic : LogicActor {
    #region 属性和字段

    public int RoleID { get; private set; }


    public long AccountID { get; private set; }


    public bool IsSelfPlayer { get; private set; } = false;

    #endregion

    #region public

    public HeroLogic(int roleID, long accountID, RenderObject renderObject) {
        this.AccountID = accountID;
        RoleID = roleID;
        RenderObject = renderObject;
        this.ObjectType = LogicObjectType.Hero;
    }

    public override void OnCreate() {
        base.OnCreate();
        InitActorSkill(this.RoleID);
        InitAttribute();
    }

    public void SetIsLocalPlayer(bool v) {
        this.IsSelfPlayer = v;
        this.RenderObject.SetIsLocalPlayer(v);
    }

    #endregion

    #region private

    /// <summary>
    /// 初始化属性
    /// </summary>
    private void InitAttribute() {
        HeroDataCfg heroDataCfg = ConfigCenter.Instance.GetHeroCfgById(RoleID);
        if (heroDataCfg == null) {
            Debug.LogError($"配置是空 heroID:{RoleID}");
            return;
        }
        this.hp = heroDataCfg.hp;
        this.mp = heroDataCfg.mp;
        this.ap = heroDataCfg.ap;
        this.ad = heroDataCfg.ad;
        this.adDef = heroDataCfg.adDef;
        this.apDef = heroDataCfg.apDef;
        this.pct = heroDataCfg.pct;
        this.mct = heroDataCfg.mct;
        this.adPctRate = heroDataCfg.adPctRate;
        this.apMctRate = heroDataCfg.apMctRate;
        this.str = heroDataCfg.str;
        this.sta = heroDataCfg.sta;
        this.Int = heroDataCfg.Int;
        this.spi = heroDataCfg.spi;
        this.agl = heroDataCfg.agl;
    }

    #endregion
}