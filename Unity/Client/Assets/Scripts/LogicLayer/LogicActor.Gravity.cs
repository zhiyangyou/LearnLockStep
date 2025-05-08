using System.Collections;
using System.Collections.Generic;
using FixMath;
using UnityEngine;

public partial class LogicActor {
    private static readonly FixIntVector3 default_Y_Velocity = new FixIntVector3(0, 2, 0);
    protected FixInt gravity = 9.8f;
    public FixIntVector3 velicity = default_Y_Velocity;
    public bool isAddForce = false;

    /// <summary>
    /// 逻辑帧: 重力 , 竖直上抛公式 : v = v0  - gt (v:速度  v0:初速度  g:重力   t:时间 )
    /// </summary>
    public void OnLogicFrameUpdate_Gravity() {
        if (isAddForce) {
            velicity.y = velicity.y - gravity * LogicFrameConfig.LogicFrameInterval;

            var originPos = LogicPos;
            var newPosY = FixIntMath.Clamp(velicity.y, 0, FixInt.MaxValue); // 防止陷入到地底下去
            FixIntVector3 newPos = new FixIntVector3(originPos.x, newPosY, originPos.z);
            // 表示落地了

            if (newPos.y <= 0) {
                isAddForce = false;
                velicity = default_Y_Velocity;
            }

            LogicPos = newPos;
        }
    }
}