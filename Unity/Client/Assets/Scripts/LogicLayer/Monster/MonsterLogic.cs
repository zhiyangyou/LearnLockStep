using System.Collections;
using System.Collections.Generic;
using FixIntPhysics;
using FixMath;
using ServerShareToClient;
using UnityEngine;
using ZMGC.Battle;

public class MonsterLogic : LogicActor {
    #region 属性字段

    public int MonsterID { get; private set; }

    private FixInt _attackRange = 0.5f; // 攻击范围
    private FixInt _chaseDistance = 4f; // 追踪范围 
    private LogicActor _chaseTarget = null; // 追踪目标
    private FixInt _moveSpeed = 1f;

    #endregion

    #region public

    public override FixInt LogicMoveSpeed {
        get { return _moveSpeed; }
        set { _moveSpeed = value; }
    }

    public MonsterLogic(int monsterID,
        RenderObject renderObject,
        FixIntBoxCollider fixIntBoxCollider,
        FixIntVector3 logicPos
    ) {
        MonsterID = monsterID;
        RenderObject = renderObject;
        FixIntBoxCollider = fixIntBoxCollider;
        LogicPos = logicPos;
        ObjectType = LogicObjectType.Monster;
    }


    public override void OnCreate() {
        base.OnCreate();
        InitAttribute();
        _chaseTarget = BattleWorld.GetExitsLogicCtrl<HeroLogicCtrl>().HeroLogic;
    }

    public override void OnHit(GameObject goEffect, int survialTimeMS, LogicObject sourceObj, FixInt logicAxisX) {
        base.OnHit(goEffect, survialTimeMS, sourceObj, logicAxisX);
        LogicAxis_X = -logicAxisX;
    }

    public override void OnLogicFrameUpdate() {
        base.OnLogicFrameUpdate();
        UpdateAIMove();
    }


    public override void State_Floating(bool isUpFloating) {
        base.State_Floating(isUpFloating);
        string animClipName = isUpFloating ? AnimaNames.Anim_Float_up : AnimaNames.Anim_Float_down;
        PlayAnim(animClipName);
        ActionState = LogicObjectActionState.Float;
    }

    public override void State_TriggerGrounding() {
        base.State_TriggerGrounding();
        if (ObjectState != LogicObjectState.Death) {
            PlayAnim(AnimaNames.Anim_Getup);
            // 延迟动画的时间之后, 将状态改成Idle
            // TODO 这个固定延迟不是很准确, 应该更加Getup的动画时间决定
            LogicTimerManager.Instance.DelayCallOnce(0.7f, () => {
                PlayAnim(AnimaNames.Anim_Idle);
                ActionState = LogicObjectActionState.Idle;
            });
        }
        else {
            PlayAnim(AnimaNames.Anim_Dead);
            // ActionState = LogicObjectActionState.Idle; // TODO 还没验证
        }
    }

    #endregion

    #region private

    private void UpdateAIMove() {
        if (ObjectState == LogicObjectState.Death) {
            return;
        }

        // 追踪目标位置
        FixIntVector3 targetPos = _chaseTarget.LogicPos;
        // 方向向量
        FixIntVector3 dirToPlayer = (_chaseTarget.LogicPos - LogicPos).normalized;
        //距离
        FixInt distance = FixIntVector3.Distance(LogicPos, _chaseTarget.LogicPos);

        if (distance <= _attackRange) {
            // 执行攻击
            if (ActionState == LogicObjectActionState.Idle) {
                PlayAnim(AnimaNames.Anim_Gongji_01);
                // TODO 使用怪物的技能系统进行技能释放
            }
        }
        else if (distance > atkRange && distance <= _chaseDistance) {
            // 执行移动
            if (ActionState == LogicObjectActionState.Idle
                || ActionState == LogicObjectActionState.Move
               ) {
                LogicPos += (dirToPlayer * LogicMoveSpeed * GameConstConfig.LogicFrameInterval);
                LogicAxis_X = dirToPlayer.x;
                PlayAnim(AnimaNames.Anim_Walk);
            }
        }
        else {
            // 什么都干
            PlayAnim(AnimaNames.Anim_Idle);
        }
    }

    private void InitAttribute() {
        MonsterCfg cfg = ConfigCenter.Instance.GetMonsterCfgById(MonsterID);
        if (cfg == null) {
            Debug.LogError($"配置是空 MonsterID:{MonsterID}");
            return;
        }
        this.hp = cfg.hp;
        this.mp = cfg.mp;
        this.ap = cfg.ap;
        this.ad = cfg.ad;
        this.adDef = cfg.adDef;
        this.apDef = cfg.apDef;
        this.pct = cfg.pct;
        this.mct = cfg.mct;
        this.adPctRate = cfg.adPctRate;
        this.apMctRate = cfg.apMctRate;
        this.str = cfg.str;
        this.sta = cfg.sta;
        this.Int = cfg.Int;
        this.spi = cfg.spi;
        this.agl = cfg.agl;

        this.atkRange = cfg.atkRange;
        this.searchDisRange = cfg.searchDisRange;

        this.LogicMoveSpeed = 1f; // TODO 读配置表
    }

    #endregion
}