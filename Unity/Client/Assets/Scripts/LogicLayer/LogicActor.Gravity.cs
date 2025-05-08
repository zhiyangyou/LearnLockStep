using System.Collections;
using System.Collections.Generic;
using FixMath;
using UnityEngine;

public partial class LogicActor {
    private static readonly FixIntVector3 default_Y_Velocity = new FixIntVector3(0, 2, 0);
    protected FixInt gravity = 9.8f;
    public FixIntVector3 velicity;
    public bool isAddForce = false;

    /// <summary>
    /// 上升时间
    /// </summary>
    private FixInt _risingTimeMS = 0;

    /// <summary>
    /// 逻辑帧: 重力 , 竖直上抛公式 : v = v0  - gt (v:速度  v0:初速度  g:重力   t:时间 )
    /// </summary>
    public void OnLogicFrameUpdate_Gravity() {
        if (isAddForce) {
            // Debug.LogError($"velicity.y - gravity * _risingTime: {velicity.y - gravity * _risingTime}");
            velicity.y -= (gravity * LogicFrameConfig.LogicFrameInterval * _risingTimeMS);
            var newPosY = FixIntMath.Clamp(LogicPos.y + velicity.y, 0, FixInt.MaxValue); // 防止陷入到地底下去
            // Debug.LogError($"newPOsY {newPosY}");
            FixIntVector3 newPos = new FixIntVector3(LogicPos.x, newPosY, LogicPos.z);
            // 表示落地了

            if (newPos.y <= 0) {
                isAddForce = false;
            }
    
            LogicPos = newPos;
        }
    }

    /// <summary>
    /// 添加一个上升力
    /// </summary>
    public void AddRisingForce(FixInt risingUpValue, FixInt risingTimeS) {
        velicity.y = risingUpValue;
        Debug.LogError($"velicity.y:{velicity.y}");
        _risingTimeMS = risingTimeS * 1f/1000f;
        isAddForce = true;
    }
}