using System.Linq;
using FixMath;

/// <summary>
/// 属性修改buff
/// </summary>
public class Buff_ModifyAttribute : BuffComposite {
    #region 属性字段

    private BuffCollider _buffCollider = null;

    /// <summary>
    /// buff伤害值
    /// </summary>
    private FixInt _buffDamageValue = 0;

    #endregion


    #region public

    public Buff_ModifyAttribute(Buff buff) : base(buff) { }

    public override void BuffDelay() { }

    public override void BuffStart() {
        var paramList = buff.BuffConfigSo.paramsList;
        if (paramList != null && paramList.Count > 0) {
            _buffDamageValue = paramList[0].Value;
        }

        // 生成碰撞体
        if (buff.BuffConfigSo.targetConfig.isOpen) {
            _buffCollider = new(buff);
            _buffCollider.CreateOrUpdateCollider();
        }
    }

    public override void BuffTrigger() {
        // throw new System.NotImplementedException();
        // 检测碰撞体
        if (buff.BuffConfigSo.targetConfig.isOpen) {
            var buffHitTargets = _buffCollider.CaculateColliderTargetObjs();
            var buffDamageConfig = buff.BuffConfigSo.targetConfig.damageConfig;
            for (var i = 0; i < buffHitTargets.Count; i++) {
                var hitTarget = buffHitTargets[i];
                if (hitTarget.ObjectState != LogicObjectState.Death) {
                    hitTarget.BuffDamage(_buffDamageValue, buffDamageConfig);
                    hitTarget.OnHit(buff.BuffConfigSo.goBuffHitEffect, 1000, hitTarget, buff.releaser.LogicAxis_X);

                    //  处理造成伤害后的buff附加

                    if (buffDamageConfig.HasAddBuffs) {
                        int[] buffIDs = buffDamageConfig.addBuffs;
                        for (int k = 0; k < buffIDs.Length; k++) {
                            var buffID = buffIDs[k];
                            BuffSystem.Instance.AttachBuff(buffID, buff.releaser, hitTarget, buff.skill, null);
                        }
                    }
                }
            }
            buffHitTargets.Clear();
        }
    }

    public override void BuffEnd() {
        // 销毁碰撞体
        // 清理列表
        _buffCollider?.OnRelease();
        _buffCollider = null;
    }

    #endregion

    #region private

    #endregion
}