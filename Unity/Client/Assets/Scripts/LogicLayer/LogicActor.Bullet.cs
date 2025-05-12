using System.Collections.Generic;

public partial class LogicActor {
    #region 属性和字段

    private List<SkillBulletLogic> _listBullets = new();

    #endregion

    #region public

    public void Bullet_Add(SkillBulletLogic bulletLogic) {
        _listBullets.Add(bulletLogic);
    }

    public void Bullet_Remove(SkillBulletLogic bulletLogic) {
        _listBullets.Remove(bulletLogic);
    }

    #endregion

    #region private

    private void OnLogicFrameUpdate_Bullet() {
        for (int i = _listBullets.Count - 1; i >= 0; i-- ) {
            if (_listBullets[i].BulletIsUnValid) {
                Bullet_Remove(_listBullets[i]);
            }
        }
        foreach (var bulletLogic in _listBullets) {
            bulletLogic.OnLogicFrameUpdate();
        }
    }

    #endregion
}