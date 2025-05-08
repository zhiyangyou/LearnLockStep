using FixMath;
using UnityEngine;

/// <summary>
/// 击退buff
/// </summary>
public class Buff_Repel : BuffComposite {
    #region 属性和字段

    #endregion

    #region life-cycle

    public override void BuffDelay() { }

    public override void BuffStart() { }

    public override void BuffTrigger() {
        var paramsList = buff.BuffConfigSo.paramsList;
        if (paramsList != null && paramsList.Count > 0) {
            FixInt repelValue = paramsList[0].Value;

            var logicAxix_X = buff.releaser.LogicAxis_X;
            var offsetPos = (FixIntVector3.right * logicAxix_X) * repelValue; // 击退后产生的偏移值

            FixIntVector3 startPos = buff.attachTarget.LogicPos;
            FixIntVector3 endPos = buff.attachTarget.LogicPos + offsetPos;

            MoveToAction moveToAction = new MoveToAction(
                buff.attachTarget,
                startPos,
                endPos,
                buff.BuffConfigSo.DurationMS,
                null,
                null,
                MoveType.Target);
            LogicActionController.Instance.RunAction(moveToAction);
        }
        else {
            Debug.LogError("Buff_Repel的参数没有配置");
        }
    }

    public override void BuffEnd() { }

    #endregion

    #region public

    public Buff_Repel(Buff buff) : base(buff) { }

    #endregion

    #region private

    #endregion
}