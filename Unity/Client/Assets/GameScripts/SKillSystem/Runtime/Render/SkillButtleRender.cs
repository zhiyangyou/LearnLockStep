using UnityEngine;

public class SkillButtleRender : RenderObject {
    #region 属性字段

    private SkillConfig_Bullet _bulletConfig;
    
    #endregion

    #region public

    public void SetRenderData(LogicObject logicObj, SkillConfig_Bullet bulletConfig) {
        
        SetLogicObject(logicObj);
        _bulletConfig = bulletConfig;
    }

    public override void UpdateDir() {

        transform.rotation = Quaternion.Euler(LogicObject.LogicAngle.ToVector3());
    }

    public override void UpdatePosition() {
        base.UpdatePosition();
    }


    public override void OnRelease() {
        base.OnRelease();
        GameObject.Destroy(this.gameObject);
    }

    #endregion

    #region private

    #endregion
}