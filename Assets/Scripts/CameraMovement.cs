
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    void Start()
    {
        // this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0.0F);
    }

    [SerializeField] public Vector3 rotateFrom = new Vector3(0.0F, 0.0F, -45.0F);
    [SerializeField] public Vector3 rotateTo = new Vector3(0.0F, 0.0F, 45.0F);
    [SerializeField] protected float m_frequency = 0.25F;

    protected virtual void Update()
    {
        Quaternion from = Quaternion.Euler(this.rotateFrom);
        Quaternion to = Quaternion.Euler(this.rotateTo);

        float lerp = 0.5F * (1.0F + Mathf.Sin(Mathf.PI * Time.realtimeSinceStartup * this.m_frequency));
        this.transform.localRotation = Quaternion.Lerp(from, to, lerp);
    }

}