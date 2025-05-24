using System;
using UnityEngine;


[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour {
    #region 属性和字段

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

    #endregion


    #region public

    public void Init(Transform trFollowTarget, Vector2 minPos, Vector2 maxPos, float smoothSpeed, float cameraInitY) {
        this.trFollowTarget = trFollowTarget;
        this.minPos = minPos;
        this.maxPos = maxPos;
        this.SmoothSpeed = smoothSpeed;
        var newPos = this.transform.position;
        newPos.y = cameraInitY;
        newPos.x = trFollowTarget.position.x;
        this.transform.position = newPos;
    }

    #endregion


    #region private

    private void LateUpdate() {
        if (trFollowTarget != null) {
            Vector3 targetPos = new Vector3(trFollowTarget.position.x, transform.position.y, transform.position.z);

            float dis = Vector2.Distance(new Vector2(trFollowTarget.position.x, 0), new Vector2(transform.position.x, 0));

            // bool isFollowTarget = dis > (followDistance - Mathf.Abs(trFollowTarget.position.x- transform.position.x));


            bool isFollowTarget = dis > followDistance;
            targetPos.x = Mathf.Clamp(targetPos.x, minPos.x, maxPos.x);
            // Vector3.SmoothDamp(,);
            // transform.position =  targetPos;
            if (isFollowTarget) {
                // 这个写法有问题的
                // Debug.LogError("do follow");
                transform.position = Vector3.Lerp(transform.position, targetPos, SmoothSpeed * Time.deltaTime);
            }
        }
    }

    #endregion
}