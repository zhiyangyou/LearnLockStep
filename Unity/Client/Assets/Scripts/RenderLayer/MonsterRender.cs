using UnityEngine;
using ZM.ZMAsset;

public class MonsterRender : RenderObject {
    #region 属性字段

    private Animation _anim;

    private string _curAnimName = null;

    private int _monsterID = 0;

    #endregion

    #region life-cycle

    public override void OnCreate() {
        base.OnCreate();
        _anim = GetComponent<Animation>();
        if (_anim == null) {
            Debug.LogError("MonsterRenderer上没有挂Animation组件");
        }
        _monsterID = ((MonsterLogic)LogicObject).MonsterID;
    }

    public override void PlayAnim(string animClipName) {
        base.PlayAnim(animClipName);
        if (_anim == null) {
            return;
        }
        // 怪物死亡只能播放死亡动画
        if (LogicObject.ObjectState == LogicObjectState.Death
            && animClipName != AnimaNames.Anim_Dead
           ) {
            Debug.LogError("怪物死亡只能播放死亡动画");
            return;
        }
        else {
            _curAnimName = animClipName;
            _anim.Play(animClipName);
        }
    }

    public override string GetCurAnimName() {
        return _curAnimName;
    }

    public override void OnHit(GameObject goEffect, int survialTimeMS, LogicObject sourceObj) {
        base.OnHit(goEffect, survialTimeMS, sourceObj);
        AudioClip audioClip = null;
        
        // 通过配置表来处理...
        if (_monsterID == 20001) {
            // 哥布林
            audioClip = ZMAsset.LoadAudio($"{AssetsPathConfig.Game_Audio_Path}Gebulin/GoblinAttackC.wav");
        }
        else if (_monsterID == 20005) {
            // 蜘蛛
            audioClip = ZMAsset.LoadAudio($"{AssetsPathConfig.Game_Audio_Path}zhizu/NorthrendGhoulWound1.wav");
        }
        if (audioClip != null) {
            AudioController.GetInstance().PlaySoundByAudioClip(audioClip, false, AudioPriorityConfig.Monster_BeHit_Audio);
        }
    }

    #endregion
}