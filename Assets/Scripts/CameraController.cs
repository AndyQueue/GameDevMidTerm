using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraTarget;
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
        if (cameraTarget != null)
        {
            FollowCameraWithBoundaries();
        }
    }

    void FollowCameraWithBoundaries()
    {
        //takes the camera's position and view into account so it doesnt stop while after wall and void is visible
        float cameraSize = mainCam.orthographicSize * mainCam.aspect;
        //logic to calculate this value from google gemini
        //prompt: how do I make the edge of my camera's view stop at an x value rather than the center so 
        //that I cannot see the void beyond that x value 
        //gemini suggested using the orthographic size and taking the aspect ratio into account 
        
        float minX = leftWall.bounds.max.x + cameraSize; //max x for left wall to get right edge
        //add cameraSize to get stopping point on the left
        float maxX = rightWall.bounds.min.x - cameraSize; //min x for right wall to get left edge
        //subtract cameraSize to get stopping point on the right
        
        float targetX = cameraTarget.position.x + followOffsetX;
        float clampedX = Mathf.Clamp(targetX, minX, maxX);
        //clamps camera's position between our min and max x from the wall's boundaries, if target smaller
        //than min we use min's value same logic for max
        transform.position = new Vector3(clampedX, 0, -10);
    }

}
