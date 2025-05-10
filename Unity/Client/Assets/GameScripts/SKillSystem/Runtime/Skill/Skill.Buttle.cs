using UnityEngine;

public partial class Skill {
    #region 属性和字段

    private int _curCreateButtleAccTimeMS = 0; // 创建子弹累加时间

    #endregion

    #region public

    #endregion

    #region private

    private void OnLogicFrameUpdate_Buttle() {
        var listBulletConfig = _skillConfig.bulletList;
        if (listBulletConfig != null && listBulletConfig.Count > 0) {
            foreach (var bulletConfig in listBulletConfig) {
                _curCreateButtleAccTimeMS += LogicFrameConfig.LogicFrameIntervalMS;
                if (bulletConfig.triggerFrame == _curLogicFrame) {
                    // 创建子弹
                    CreateButtle(bulletConfig);
                }
                if (bulletConfig.isLoopCreate) {
                    while (_curCreateButtleAccTimeMS >= bulletConfig.loopIntervalMS) {
                        CreateButtle(bulletConfig);
                        _curCreateButtleAccTimeMS -= bulletConfig.loopIntervalMS;
                    }
                }
            }
        }
    }

    private void CreateButtle(SkillConfig_Bullet bulletConfig) {
        
        // TODO 对象池? 资源框架的资源池管理
        var goBullet = GameObject.Instantiate(bulletConfig.goBulletPrefab);
        
        
    }

    #endregion
}