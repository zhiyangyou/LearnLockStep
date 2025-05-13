using FixMath;
using UnityEngine;

/// <summary>
/// 抓取buff
/// </summary>
public class Buff_Grab : BuffComposite {
    #region 属性和字段

    #endregion

    #region life-cycle

    public override void BuffDelay() { }

    public override void BuffStart() { }


    public override void BuffTrigger() {
        // 根据抓取的数据, 让当前目标飞向指定的目标点    

        // 目标点
        LogicObject attachTarget = buff.attachTarget;
        LogicObject releaser = buff.releaser;

        // 抓取怪物到角色所在位置
        attachTarget.LogicPos = releaser.LogicPos; // RenderObject中会有一个插值动画

        // 把怪物抓取到指定目标点
        FixIntVector3 grabPos = new FixIntVector3(buff.BuffConfigSo.targetGrabData.GrabMovePos) * releaser.LogicAxis_X;
        grabPos.y = FixIntMath.Abs(grabPos.y);
        FixIntVector3 targetGrabPos = releaser.LogicPos + grabPos;

        var moveAction = new MoveToAction(
            attachTarget,
            attachTarget.LogicPos,
            targetGrabPos, buff.BuffConfigSo.targetGrabData.GrabMoveTimeMS, null, null,
            MoveType.Target);
        LogicActionController.Instance.RunAction(moveAction);
    }

    public override void BuffEnd() { }

    #endregion

    #region public

    public Buff_Grab(Buff buff) : base(buff) { }

    #endregion

    #region private

    #endregion
}