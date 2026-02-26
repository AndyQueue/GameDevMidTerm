using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private float startingPosition;
    private float length;
    public GameObject camera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distanceMoved = camera.transform.position.x * (1 - 0.5f);
        transform.position = new Vector3(startingPosition + distanceMoved, transform.position.y, transform.position.z);

        if (camera.transform.position.x > startingPosition + length)
        {
            startingPosition += length;
        }
        else if (camera.transform.position.x < startingPosition - length)
        {
            startingPosition -= length;
        }
    }
}
