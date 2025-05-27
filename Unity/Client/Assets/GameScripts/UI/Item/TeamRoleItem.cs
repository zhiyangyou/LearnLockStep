using Fantasy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZM.ZMAsset;

public class TeamRoleItem : MonoBehaviour {
    public Image headImage;
    public Text LevelText;
    public Text nickNameText;

    public void SetItemData(RoleData roleData) {
        headImage.sprite = ZMAsset.LoadSprite(AssetsPathConfig.Game_Texture_Path + $"HeadIcon/{roleData.role_id}");
        LevelText.text = "Lv." + roleData.level;
        nickNameText.text = roleData.role_name;
    }

    public void Release() {
        ZMAsset.Release(gameObject);
    }
}