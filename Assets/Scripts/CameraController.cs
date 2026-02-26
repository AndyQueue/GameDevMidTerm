using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTarget;
    public float followOffsetX;

    [Header("Walls")]
    public BoxCollider2D leftWall;
    public BoxCollider2D rightWall;
    private Camera mainCam;

    void Start()
    {
        mainCam = GetComponent<Camera>();
    }

    //we use late update since our player uses update, runs after update for smooth following of player
    void LateUpdate()
    {
        FollowCameraWithBoundaries();
    }

    void FollowCameraWithBoundaries()
    {
        //orthographic height gets half the height of x, we multiply by the camera's aspect ratio 
        //(width over height) and thus gives us half the width of the camera's view
        float halfWidth = mainCam.orthographicSize * mainCam.aspect;
        
        
        float minX = leftWall.bounds.max.x + halfWidth; //max x for left wall to get right edge
        //add halfWidth to get stopping point on the left since we change the camera's center position
        float maxX = rightWall.bounds.min.x - halfWidth; //min x for right wall to get left edge
        //subtract halfWidth to get stopping point on the right since we change the camera's center position
        
        float targetX = playerTarget.position.x + followOffsetX;
        float clampedX = Mathf.Clamp(targetX, minX, maxX);
        //clamps camera's center position between our min and max x from the wall's boundaries, if target smaller
        //than min we use min's value same logic for max
        transform.position = new Vector3(clampedX, 0, -10);
    }

}
