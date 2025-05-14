using System;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class CanvasCamera : MonoBehaviour {
    private void Awake() {
        GetComponent<Canvas>().worldCamera = UIModule.Instance.Camera;
    }
}