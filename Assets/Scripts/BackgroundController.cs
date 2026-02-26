using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private float startingPosition;
    private float length;
    public GameObject cam;

    void Start()
    {
        startingPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float distanceMoved = cam.transform.position.x * (1 - 0.5f);
        transform.position = new Vector3(startingPosition + distanceMoved, transform.position.y, transform.position.z);

        if (cam.transform.position.x > startingPosition + length)
        {
            startingPosition += length;
        }
        else if (cam.transform.position.x < startingPosition - length)
        {
            startingPosition -= length;
        }
    }
}
