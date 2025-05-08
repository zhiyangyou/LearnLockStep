using UnityEngine;

public class MonsterRender : RenderObject {
    #region 属性字段

    private Animation _anim;

    private string _curAnimName = null;

    #endregion

    #region life-cycle

    private void Start() {
        _anim = GetComponent<Animation>();
        if (_anim == null) {
            Debug.LogError("MonsterRenderer上没有挂Animation组件");
        }
    }

    protected override void Update() {
        base.Update();
    }

    private void OnDestroy() { }

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

    #endregion
}