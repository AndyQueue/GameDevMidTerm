// this script is mainly from a Youtube tutorial, edited to fit our game. https://youtu.be/d002CljR-KU?si=gM51dETCUJaY7Swd
using Unity.VisualScripting;
using UnityEngine;

public class GuardPatrol : MonoBehaviour
{
    [Header ("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header ("Guard")]
    [SerializeField] private Transform guard;
    [Header ("Movment Parameters")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;
    [Header("Idle Behvaior")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("Guard Animator")]
    [SerializeField] private Animator anim;

    private void Awake()
    {
        anim = guard.GetComponent<Animator>();
        initScale = guard.localScale;
    }

    private void OnDisable()
    {
        if (anim!=null)
            anim.SetBool("IsMoving", false);
    }

    private void Update()
    {
        // animation for patrolling
        if (movingLeft)
        {
            if (guard.position.x >= leftEdge.position.x)
            {
                MoveInDirection(-1);
            }
            else
            {
                DirectionChange();
            }
        }
        else
        {
            if (guard.position.x <= rightEdge.position.x)
            {
               MoveInDirection(1);
            }
            else
            {
                DirectionChange();
            }
        }
    }

    //moving and animating guard in the right direction
    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;
        anim.SetBool("IsMoving", true);
        guard.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction, initScale.y, initScale.z);
        guard.position = new Vector3(guard.position.x + Time.deltaTime * _direction * speed, guard.position.y, guard.position.z);
    }

    // change direction guard is moving
    private void DirectionChange()
    {
        anim.SetBool("IsMoving", false);
        idleTimer += Time.deltaTime;
        if (idleTimer > idleDuration)
        {
            movingLeft = !movingLeft;
        }
    }

    public void StopGuard()
    {
        anim.SetBool("IsMoving", false);
        guard.position = guard.position;
    }

}
