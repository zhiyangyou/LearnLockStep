using FixMath;
using UnityEngine;
using ZM.ZMAsset;

public class MonsterRender : RenderObject {
    #region 属性字段

    private Animation _anim;

    private string _curAnimName = null;

    private int _monsterID = 0;

    private MonsterCfg _monsterCfg;

    private MonsterLogic _monsterLogic;

    #endregion

    #region life-cycle

    public override void OnCreate() {
        base.OnCreate();
        _anim = GetComponent<Animation>();
        if (_anim == null) {
            Debug.LogError("MonsterRenderer上没有挂Animation组件");
        }
        _monsterID = ((MonsterLogic)LogicObject).MonsterID;
        _monsterCfg = ConfigCenter.Instance.GetMonsterCfgById(_monsterID);
        _monsterLogic = this.LogicObject as MonsterLogic;
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
            if (_anim.GetClip(animClipName)) {
                _anim.Play(animClipName);
            }
            else {
                Debug.LogWarning($"{this.gameObject.name} Animation中没有{animClipName}动画");
            }
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
        else if (_monsterID == 30001) {
            // 蜘蛛
            audioClip = ZMAsset.LoadAudio($"{AssetsPathConfig.Game_Audio_Path}zhizu/NorthrendGhoulWound1.wav");
        }
        if (audioClip != null) {
            AudioController.GetInstance().PlaySoundByAudioClip(audioClip, false, AudioPriorityConfig.Monster_BeHit_Audio);
        }
    }

    public override void Damage(int damageValue, DamageSource damageSource) {
        base.Damage(damageValue, damageSource);
        UIModule.Instance.GetWindow<BattleWindow>()
            .ShowMonsterDamage(_monsterCfg, gameObject.GetInstanceID(), _monsterLogic.HP, new FixInt(damageValue));
    }

    public override void OnDeath() {
        base.OnDeath();
        PlayAnim(AnimaNames.Anim_Dead);
        LogicTimerManager.Instance.DelayCallOnce(1.5f, () => {
            ZMAsset.Release(gameObject);
        });
    }

    #endregion
}