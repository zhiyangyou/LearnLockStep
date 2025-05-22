using System;
using System.Threading.Tasks;
using Fantasy;
using UnityEngine;
using UnityEngine.UI;
using ZM.ZMAsset;

public class ItemSelectRole : MonoBehaviour {
    #region 属性字段

    [SerializeField] private GameObject goSelectImage;
    [SerializeField] private Transform trModelPos;
    [SerializeField] private Text txtLevel;
    [SerializeField] private Text txtRoleName;
    [SerializeField] private Text txtRoleTypeName;
    [SerializeField] private GameObject goInfo;

    private AssetsRequest _goModelRequest = null;

    #endregion

    #region public

    public async void SetData(RoleData roleData) {
        goSelectImage.SetActive(false);
        if (roleData == null) {
            goInfo.SetActive(false);
            return;
        }
        goInfo.SetActive(true);
        txtLevel.text = $"Lv.{roleData.level}";
        txtRoleName.text = roleData.role_name;
        var roleID = roleData.role_id;
        txtRoleTypeName.text = ConfigCenter.Instance.GetHeroCfgById(roleID).name;

        _goModelRequest = await ZMAsset.InstantiateAsync($"{AssetsPathConfig.Hall_Prefabs}Role/Role_{roleID}.prefab", this.trModelPos);
        _goModelRequest.obj.transform.ReSetParent(this.trModelPos.transform);
        // _goModelRequest.obj.layer = LayerMask.NameToLayer("UI");
    }

    public void SetIsSelect(bool isSelect) {
        goSelectImage.SetActive(isSelect);
    }

    public void OnRelease() {
        if (_goModelRequest != null) {
            _goModelRequest.Release();
        }
    }
    

    #endregion

    public void SetModelVisiable(bool visiable) {
        if (_goModelRequest != null) {
            _goModelRequest.obj.SetActive(visiable);
        }
    }
}