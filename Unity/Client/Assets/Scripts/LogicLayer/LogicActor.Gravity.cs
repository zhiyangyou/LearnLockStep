using System.Collections;
using System.Collections.Generic;
using FixMath;
using UnityEngine;

public partial class LogicActor {
    protected FixInt gravity = 9.8f;
    public FixIntVector3 velicity;
    public bool isAddForce = false;
    private FixInt _V0; // 上升的初速度

    private float _StartFloatingTime = 0f;


    /// <summary>
    /// 浮空总时间
    /// </summary>
    private FixInt _risingTimeS = 0;

    /// <summary>
    /// 逻辑帧: 重力 , 竖直上抛公式 : v = v0  - gt (v:速度  v0:初速度  g:重力   t:时间 )
    /// </summary>
    public void OnLogicFrameUpdate_Gravity() {
        if (isAddForce) {
            // Debug.LogError($"velicity.y - gravity * _risingTime: {velicity.y - gravity * _risingTime}");
            float logicFrameInterval = LogicFrameConfig.LogicFrameInterval;
            FixInt gt = gravity * logicFrameInterval;

            //  就是 v0/g 求出上升时间
            FixInt risingUpTime = (_V0 / gt) * logicFrameInterval;
            // 时间倍率
            FixInt timeScale = (risingUpTime * 2) / _risingTimeS; // 时间缩放倍率

            velicity.y -= (gravity * logicFrameInterval * timeScale);

            // Debug.LogError($"gt:{gt} timeScale:{timeScale} velicity.y:{velicity.y} ");

            var newPosY = FixIntMath.Clamp(LogicPos.y + velicity.y * logicFrameInterval, 0, FixInt.MaxValue); // 防止陷入到地底下去
            FixIntVector3 newPos = new FixIntVector3(LogicPos.x, newPosY, LogicPos.z);  
            // Debug.LogError($"{Time.frameCount} 复制浮空位置数值");
            // 落地
            if (newPos.y <= 0) {
                isAddForce = false;
                State_TriggerGrounding();
                // var allFloatingTime = Time.realtimeSinceStartup - _StartFloatingTime;
                // Debug.LogError($"浮空总时间:{allFloatingTime} 误差:{allFloatingTime / _risingTimeS}");
            }
            else {
                // 上升阶段
                if (velicity.y >= 0) {
                    State_Floating(true);
                }
                // 下降阶段
                else {
                    State_Floating(false);
                }
            }

            LogicPos = newPos;
        }
    }

    /// <summary>
    /// 添加一个上升力
    /// </summary>
    public void AddRisingForce(FixInt risingUpVelocity, FixInt risingTimeS) {
        _V0 = risingUpVelocity;
        velicity.y = risingUpVelocity;
        _risingTimeS = risingTimeS;
        isAddForce = true;
        _StartFloatingTime = Time.realtimeSinceStartup;
    }
}