using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraTarget;
    public float followOffsetX;

    void Update()
    {
        FollowCameraTargetHorizontally();
    }
    public void FollowCameraTargetHorizontally()
    {
        Vector3 targetPosition = transform.position;
        targetPosition.x = cameraTarget.position.x +followOffsetX;
        transform.position = targetPosition;
    }

}
