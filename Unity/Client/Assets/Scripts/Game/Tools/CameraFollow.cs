using System;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using UnityEngine;


[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour {
    [NonSerialized] public Transform trFollowTarget;

    [SerializeField] Vector2 minPos;
    [SerializeField] Vector2 maxPos;

    [SerializeField] float smoothTime = 0.3f;

    /// <summary>
    /// 偏移一定距离后才跟随
    /// </summary>
    [SerializeField] float followDistance = 0.1f;

    private void LateUpdate() {
        if (trFollowTarget != null) {
            Vector3 targetPos = new Vector3(trFollowTarget.position.x, transform.position.y, transform.position.z);

            float dis = Vector2.Distance(targetPos, transform.position);

            bool isFollowTarget = dis > followDistance;

            targetPos.x = Mathf.Clamp(targetPos.x, minPos.x, maxPos.y);

            if (isFollowTarget) {
                transform.position = Vector3.Lerp(transform.position, targetPos, smoothTime * Time.deltaTime);
            }
        }
    }
}