using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 粒子效果播放器
/// </summary>
public class ParticleAgent
{
    private ParticleSystem[] _particleArr;
    private double _lastRuntime = 0f;

#if UNITY_EDITOR
    public void Init(Transform tr)
    {
        _particleArr = tr.GetComponentsInChildren<ParticleSystem>();
        EditorApplication.update += OnUpdate;
    }

    public void OnDestory()
    {
        EditorApplication.update -= OnUpdate;
    }

#endif
    public void OnUpdate()
    {
        if (_lastRuntime == 0)
        {
            _lastRuntime = EditorApplication.timeSinceStartup;
        }
        // 当前运行时间
        double curRuntime = EditorApplication.timeSinceStartup - _lastRuntime;

        if (_particleArr != null)
        {
            foreach (var p in _particleArr)
            {
                if (p != null)
                {
                    p.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

                    p.useAutoRandomSeed = false;

                    p.Simulate((float)curRuntime);
                }
            }
        }
    }
}