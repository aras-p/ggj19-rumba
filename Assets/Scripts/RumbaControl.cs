using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RumbaControl : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float rotateSpeed = 10.0f;
    public Color arrowInactiveColor = new Color(1,1,1,0.25f);
    public Color arrowActiveColor = new Color(1, 0.5f, 0.5f, 1.0f);
    public AudioClip[] pickupSounds;
    public AudioClip[] bumpSounds;
    Transform tr;
    Rigidbody2D rb;
    SpriteRenderer arrowLeft;
    SpriteRenderer arrowRight;
    float directionChoice;

    void Start()
    {
        tr = transform;
        rb = GetComponent<Rigidbody2D>();
        arrowLeft = tr.Find("left").GetComponent<SpriteRenderer>();
        arrowRight = tr.Find("right").GetComponent<SpriteRenderer>();
        UpdateArrowColors();
    }
    
    void Update()
    {
        if (Game.instance.state != Game.State.Game)
            return;
        var fwd = tr.up * moveSpeed;
        rb.velocity = fwd;

        var hor = Input.GetAxis("Horizontal");
        directionChoice = -hor;

        rb.angularVelocity = directionChoice * rotateSpeed;

        UpdateArrowColors();
        
        RoomClearing.instance.DrawTrace(tr.localPosition);
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (Game.instance.state != Game.State.Game)
            return;        
        var pickup = c.gameObject.GetComponent<Pickupable>();
        if (pickup != null)
        {
            AudioSource.PlayClipAtPoint(pickupSounds[Random.Range(0, pickupSounds.Length)], Camera.main.transform.position);
            Destroy(c.gameObject);
            Game.instance.score += pickup.score;
            return;
        }
        AudioSource.PlayClipAtPoint(bumpSounds[Random.Range(0, bumpSounds.Length)], Camera.main.transform.position, 0.7f);
    }

    void OnCollisionStay2D(Collision2D c)
    {
        if (Game.instance.state != Game.State.Game)
            return;

        var pickup = c.gameObject.GetComponent<Pickupable>();
        if (pickup != null)
            return;
        
        var dir = tr.localEulerAngles.z;
        var newDir = dir + directionChoice * rotateSpeed * Time.deltaTime * 15.0f;
        rb.MoveRotation(newDir);
        UpdateArrowColors();
    }

    void UpdateArrowColors()
    {
        arrowLeft.color = directionChoice > 0.1f ? arrowActiveColor : arrowInactiveColor;
        arrowRight.color = directionChoice < -0.1f ? arrowActiveColor : arrowInactiveColor;
    }
}
