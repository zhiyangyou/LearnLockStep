using System.Collections.Generic;
using FixMath;
using UnityEngine;

public partial class Skill {
    #region 属性和字段

    private LogicRandom _logicRandom = null;

    /// <summary>
    /// 当前所有子弹累计时间
    /// </summary>
    private List<int> _listCurCreateBulletAccTimeMS = new();

    #endregion

    #region public

    #endregion

    #region private

    private void OnBulletInit() {
        _listCurCreateBulletAccTimeMS.Clear();
        var listBulletConfig = _skillConfig.bulletList;
        if (listBulletConfig != null && listBulletConfig.Count > 0) {
            for (int i = 0; i < listBulletConfig.Count; i++) {
                _listCurCreateBulletAccTimeMS.Add(0);
            }
        }
        _logicRandom = new LogicRandom(10);
    }

    private void OnLogicFrameUpdate_Buttle() {
        var listBulletConfig = _skillConfig.bulletList;
        if (listBulletConfig != null && listBulletConfig.Count > 0) {
            for (int i = 0; i < listBulletConfig.Count; i++) {
                var bulletConfig = listBulletConfig[i];
                _listCurCreateBulletAccTimeMS[i] += LogicFrameConfig.LogicFrameIntervalMS;
                if (bulletConfig.triggerFrame == _curLogicFrame) {
                    // 创建子弹
                    CreateButtle(bulletConfig);
                }
                if (bulletConfig.isLoopCreate) {
                    if (bulletConfig.loopIntervalMS <= 0) {
                        Debug.LogError($"错误的loopIntervalMS :{bulletConfig.loopIntervalMS}");
                        continue;
                    }
                    while (_listCurCreateBulletAccTimeMS[i] >= bulletConfig.loopIntervalMS) {
                        CreateButtle(bulletConfig);
                        _listCurCreateBulletAccTimeMS[i] -= bulletConfig.loopIntervalMS;
                    }
                }
            }
        }
    }

    private void CreateButtle(SkillConfig_Bullet bulletConfig) {
        // TODO 对象池? 资源框架的资源池管理
        var goBullet = GameObject.Instantiate(bulletConfig.goBulletPrefab);

        SkillButtleRender bulletRender = goBullet.GetComponent<SkillButtleRender>();
        if (bulletRender == null) {
            bulletRender = goBullet.AddComponent<SkillButtleRender>();
        }

        FixIntVector3 rangePos = FixIntVector3.zero;
        if (bulletConfig.isLoopCreate) {
            var minV3 = bulletConfig.minRandomRangeVec3;
            var maxV3 = bulletConfig.maxRandomRangeVec3;
            rangePos.x = _logicRandom.Range(minV3.x, maxV3.x);
            rangePos.y = _logicRandom.Range(minV3.y, maxV3.y);
            rangePos.z = _logicRandom.Range(minV3.z, maxV3.z);
        }
        SkillBulletLogic skillBulletLogic = new SkillBulletLogic(this, _skillCreater, bulletRender, bulletConfig, rangePos);

        bulletRender.SetRenderData(skillBulletLogic, bulletConfig);

        _skillCreater.Bullet_Add(skillBulletLogic);
    }

    private void OnBulletRelease() {
        _listCurCreateBulletAccTimeMS.Clear();
        _logicRandom = null;
    }

    #endregion
}