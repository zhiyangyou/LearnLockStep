using System;
using FixMath;
using UnityEngine;
using UnityEngine.UI;
using ZM.ZMAsset;

public class MonsterBloodItem : MonoBehaviour {
    #region 属性字段

    public Image imgHead;
    public Image imgMonsterType;
    public MultipleBloodBars bloodBars;
    public Text txtName;

    private MonsterCfg _monsterCfg;
    private int monsterID;

    public int InstanceID { get; private set; }

    #endregion

    #region public

    public void InitBloodData(MonsterCfg monsterCfg, FixInt curHP, int instanceID) {
        InstanceID = instanceID;
        bloodBars.InitBlood(curHP.RawInt);
        imgHead.sprite = ZMAsset.LoadSprite($"{AssetsPathConfig.Game_Texture_Path}HeadIcon/{monsterCfg.id}.png");
        imgMonsterType.sprite = ZMAsset.LoadPNGAtlasSprite($"{AssetsPathConfig.Game_Texture_Path}BttlePEV/p_UI_Battle_Pve.png", GetMonsterTypeName(monsterCfg));
        txtName.text = monsterCfg.name;
    }

    public void Damage(FixInt subHp) {
        // Debug.LogError($"sub hp {subHp} nowHp:{bloodBars.nowBlood}");
        if (!gameObject.activeSelf) {
            gameObject.SetActive(true);
        }
        bloodBars.ChangeBlood(subHp.RawFloat);
        if (bloodBars.nowBlood <= 0) {
            bloodBars.gameObject.SetActive(false);
        }
    }

    #endregion

    #region private

    private string GetMonsterTypeName(MonsterCfg config) {
        switch (config.type) {
            case MonsterType.Normal:
                return "UI_Battle_Pve_Tubiao_Putong";
            case MonsterType.Elite:
                return "UI_Battle_Pve_Tubiao_Jingying";
            case MonsterType.Boss:
                return "UI_Battle_Pve_Tubiao_Lingzhu";
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #endregion
}