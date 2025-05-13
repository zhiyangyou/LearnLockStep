using System;
using FixMath;
using UnityEngine;

/// <summary>
/// 处理buff的绘制, 比如: 人物身上回复绿光buff
/// </summary>
public class BuffRender : RenderObject {
    #region 属性字段

    private HeroRender _heroRender;
    private BuffConfigSO _buffConfig;
    private FixIntVector3 _buffInputPos;

    private LogicActor _buffReleaser;
    private LogicActor _buffAttachTarget;

    #endregion

    #region public

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="buffReleaser"></param>
    /// <param name="buffConfigSo"></param>
    /// <param name="targetPos"></param>
    public void InitBuffRender(LogicActor buffReleaser, LogicActor buffAttachTarget, BuffConfigSO buffConfigSo, FixIntVector3 targetPos) {
        base.SetLogicObject(buffReleaser);
        _buffInputPos = targetPos;
        _buffConfig = buffConfigSo;
        _heroRender = buffReleaser.RenderObject as HeroRender;
        _buffReleaser = buffReleaser;
        _buffAttachTarget = buffAttachTarget;
        // 音效
        if (buffConfigSo.audioClip != null) {
            AudioController.GetInstance().PlaySoundByAudioClip(buffConfigSo.audioClip, false, AudioPriorityConfig.Buff_AudioClip);
        }

        // 特效位置
        var attachPosType = buffConfigSo.effectConfig.effectAttachPosType;
        if (attachPosType == BuffEffectAttachPosType.Hand) {
            transform.SetParent(_heroRender.GetTransParent(TransParentType.LeftHand));
            transform.localScale = Vector3.one;
            transform.localPosition = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }
        else {
            switch (buffConfigSo.posType) {
                case BuffPosType.HitTargetPos:
                    transform.position = buffAttachTarget.LogicPos.ToVector3();
                    break;
                case BuffPosType.UIInputPos:
                    transform.position = _buffInputPos.ToVector3();
                    break;
                case BuffPosType.FollowTarget:
                    transform.position = _buffReleaser.LogicPos.ToVector3();
                    break;
                case BuffPosType.None:
                default:
                    Debug.LogError("尚未实现的buff的PosType");
                    break;
            }
        }

        // 重置粒子状态播放状态
        PlayParticle();
    }


    protected override void Update() {
        base.Update();
        if (_buffConfig != null
            && _buffConfig.posType == BuffPosType.FollowTarget) {
            transform.position = _buffAttachTarget.RenderObject.transform.position;
        }
    }

    public override void OnRelease() {
        base.OnRelease();
        _heroRender = null;
        _buffConfig = null;
        _buffReleaser = null;
        _buffAttachTarget = null;
        DestroyImmediate(gameObject);
    }

    #endregion

    #region private

    /// <summary>
    /// 重置粒子播放状态
    /// </summary>
    private void PlayParticle() {
        var ps = transform.GetComponents<ParticleSystem>();
        foreach (var p in ps) {
            p.Play();
        }
    }

    #endregion
}