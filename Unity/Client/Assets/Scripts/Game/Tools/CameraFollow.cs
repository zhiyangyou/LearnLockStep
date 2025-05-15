using System;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using UnityEngine;
using UnityEngine.Serialization;


[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour {
    [NonSerialized] public Transform trFollowTarget;

    [SerializeField] Vector2 minPos;
    [SerializeField] Vector2 maxPos;

    [SerializeField] float SmoothSpeed = 6f;

    /// <summary>
    /// 偏移一定距离后才跟随
    /// </summary>
    [SerializeField] float followDistance = 0.1f;
    // [SerializeField] float bufferRatio = 0.3f; // 缓冲比例

    // private bool isFollowing = false;


    private void LateUpdate() {
        if (trFollowTarget != null) {
            Vector3 targetPos = new Vector3(trFollowTarget.position.x, transform.position.y, transform.position.z);

            float dis = Vector2.Distance(new Vector2(trFollowTarget.position.x, 0), new Vector2(transform.position.x, 0));

            // bool isFollowTarget = dis > (followDistance - Mathf.Abs(trFollowTarget.position.x- transform.position.x));


            bool isFollowTarget = dis > followDistance;
            targetPos.x = Mathf.Clamp(targetPos.x, minPos.x, maxPos.x);
            // Vector3.SmoothDamp(,);
            // transform.position =  targetPos;
            if (isFollowTarget) { // 这个写法有问题的
                // Debug.LogError("do follow");
                transform.position = Vector3.Lerp(transform.position, targetPos, SmoothSpeed * Time.deltaTime);
            }
        }
    }
}