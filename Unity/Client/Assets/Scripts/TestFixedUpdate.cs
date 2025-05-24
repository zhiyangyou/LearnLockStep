using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class TestFixedUpdate : MonoBehaviour {
    // Update is called once per frame
    void Update() {
        var sb = new StringBuilder();
        for (int i = 0; i < 100_1000; i++) {
            sb.AppendLine($"index {i}");
        }
    }

    private void FixedUpdate() {
        // 如果update逻辑很复杂,拖慢了帧率, 在同一帧之内, 会发生多次的fixedUpdate调用
        Debug.LogError($"fixedUpdate {Time.fixedDeltaTime}: {Time.frameCount}");
    }
}