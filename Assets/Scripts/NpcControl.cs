using UnityEngine;

public class NpcControl : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    Transform tr;
    Rigidbody2D rb;

    void Start()
    {
        tr = transform;
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        var fwd = tr.up * moveSpeed;
        rb.velocity = fwd;
    }

    void OnCollisionStay2D(Collision2D c)
    {
        var dir = tr.localEulerAngles.z;
        var newDir = dir + 180 + Random.Range(-30,30);
        rb.MoveRotation(newDir);
    }
}
