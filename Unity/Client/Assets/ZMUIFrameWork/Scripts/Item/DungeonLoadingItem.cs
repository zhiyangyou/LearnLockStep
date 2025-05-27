using Fantasy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZM.AssetFrameWork;
public class DungeonLoadingItem : MonoBehaviour
{
    public Image headImage;
    public Image headFrameImage;
    public Image sliderImage;
    public Text nickNameText;
    public Text levelText;
    private C2G_Role mRoleData;
    public void SetItemData(C2G_Role roleData, float progress)
    {
        UIEventControl.AddEvent(UIEventEnum.DungeonProgress, OnDungeonProgress);
        mRoleData= roleData;
        levelText.text = "Lv."+ roleData.level;
        nickNameText.text = roleData.roleName;
        headImage.sprite = ZMAsset.LoadSprite(AssetPathConfig.GAME_TEXTURES_PATH+ $"HeadIcon/{roleData.roleid}");
        sliderImage.fillAmount = progress;
    }
    public void OnDungeonProgress(object data)
    {
        if (data != null)
        {
            //�������װ��Ͳ���������ṩ�˷�ʽ��Ϊ�������ⳡ�����ṩ�����ԣ�����ʹ��
            LoadDungeonMessage loadData = (LoadDungeonMessage)data;
            if (mRoleData.accountId ==loadData.playerid)
            {
                sliderImage.fillAmount = loadData.progress;
            }
        }
    }
    public void OnRelease()
    {
        UIEventControl.RemoveEvent(UIEventEnum.DungeonProgress, OnDungeonProgress);
        ZMAsset.Release(gameObject,true);
    }
}
