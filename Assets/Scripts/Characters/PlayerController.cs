using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : BaseEntity
{
    public float speed;
    public InputAction move;
    public Transform camPos;
    public bool useGravity = true;
    
    void FixedUpdate()
    {
        Movement();
        mh.playerPos = transform.position;
    }

    private void Movement()
    {
        Vector2 dir = move.ReadValue<Vector2>();
        float forward = dir.y;
        float right = dir.x;

        if (!(forward == 0f && right == 0f))
        {
            var transform2 = transform;
            Vector3 f = transform2.forward * forward;
            Vector3 r = transform2.right * right;

            rb.velocity = (f + r).normalized * (Time.fixedDeltaTime * speed);
        }
        else
        {
                rb.velocity = Vector3.zero;
        }
        
        var transform1 = transform;
        transform1.rotation = Quaternion.FromToRotation(transform1.up, transform1.position) * transform.rotation;                                
        
        if (!IsOnTheGround() && useGravity)
        {
            Vector3 g = transform.position * (-gravity * Time.fixedDeltaTime * 0.01f);
            rb.velocity += g;
        }
    }

    void EnableGravity()
    {
        useGravity = true;
    }
    
    private void OnEnable()
    {
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }
}
