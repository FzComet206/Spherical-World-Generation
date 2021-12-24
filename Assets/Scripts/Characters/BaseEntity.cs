using UnityEngine;

public class BaseEntity : MonoBehaviour
{
    public Rigidbody rb;
    public new CapsuleCollider collider; 
    public bool isActive;
    public int gravity = 10;
    public float groundedDistance = 0.0001f;

    public MeshHelper mh;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        collider = GetComponent<CapsuleCollider>();
        mh = FindObjectOfType<MeshHelper>();
    }

    public bool IsOnTheGround()
    {
        var bounds = collider.bounds;
        return Physics.Raycast(bounds.center, Vector3.down,
            bounds.extents.y + groundedDistance);
    }
    
    public void Move(Vector3 dir)
    {
        rb.velocity = dir;
        rb.transform.rotation = Quaternion.Euler(-transform.position);
    }
}
