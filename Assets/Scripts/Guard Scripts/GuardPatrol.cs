using Unity.VisualScripting;
using UnityEngine;

public class GuardPatrol : MonoBehaviour
{
    [Header ("Patrol Points")]
    [SerializeField]private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header ("Guard")]
    [SerializeField] private Transform guard;
     [Header ("Movment Parameters")]
    [SerializeField] private float speed;

    private void Update()
    {
        MoveInDirection(1);
    }
    private void MoveInDirection(int _direction)
    {
        guard.position = new Vector3(guard.position.x + Time.deltaTime * _direction * speed, guard.position.y, guard.position.z);
    }
}
