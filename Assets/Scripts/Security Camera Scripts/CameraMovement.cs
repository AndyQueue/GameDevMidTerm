
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [SerializeField] public Vector3 rotateFrom = new Vector3(0.0F, 0.0F, -45.0F);
    [SerializeField] public Vector3 rotateTo = new Vector3(0.0F, 0.0F, 45.0F);
    [SerializeField] protected float m_frequency = 0.25F;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private float timer;

    void Update()
    {
        Quaternion from = Quaternion.Euler(this.rotateFrom);
        Quaternion to = Quaternion.Euler(this.rotateTo);
        timer += Time.deltaTime;

        float lerp = 0.5F * (1.0F + Mathf.Sin(Mathf.PI * timer * this.m_frequency));
        this.transform.localRotation = Quaternion.Slerp(from, to, lerp);
    }

}