#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

/// <summary>
/// 动画播放代理
/// </summary>
public class AnimationAgent
{
    private Animation _animation;

    private double _lastRuntime = 0f;

#if UNITY_EDITOR
    public void Init(Transform tr)
    {
        _animation = tr.GetComponentInChildren<Animation>();
        EditorApplication.update += OnUpdate;
    }

    public void OnDestory()
    {
        EditorApplication.update -= OnUpdate;
    }


    public void OnUpdate()
    {
        if (_animation == null || _animation.clip == null) return;
        if (_lastRuntime == 0)
        {
            _lastRuntime = EditorApplication.timeSinceStartup;
        }
        // 当前运行时间
        double curRuntime = EditorApplication.timeSinceStartup - _lastRuntime;

        // 动画播放进度
        // float curAniNormalizationValue = (float)curRuntime / _animation.clip.length;

        // 采样动画
        _animation.clip.SampleAnimation(_animation.gameObject, (float)curRuntime);
    }
#endif
}