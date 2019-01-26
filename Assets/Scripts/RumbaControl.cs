using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RumbaControl : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float rotateSpeed = 10.0f;
    public Color arrowInactiveColor = new Color(1,1,1,0.25f);
    public Color arrowActiveColor = new Color(1, 0.5f, 0.5f, 1.0f);
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
    	//var dt = Time.deltaTime;
        var fwd = tr.up * moveSpeed;
        rb.velocity = fwd;

        var hor = Input.GetAxis("Horizontal");
        if (hor < 0)
            directionChoice = 1;
        else if (hor > 0)
            directionChoice = -1;
        if (Math.Abs(directionChoice) < 0.05f)
            directionChoice = 0;

        rb.angularVelocity = directionChoice * rotateSpeed;

        UpdateArrowColors();
    }

    void OnCollisionStay2D(Collision2D c)
    {
        var dir = tr.localEulerAngles.z;
        var newDir = dir + Random.Range(-5,5) + directionChoice * 90;
        rb.MoveRotation(newDir);
        directionChoice *= 0.5f;
        UpdateArrowColors();
    }

    void UpdateArrowColors()
    {
        arrowLeft.color = directionChoice > 0 ? arrowActiveColor : arrowInactiveColor;
        arrowRight.color = directionChoice < 0 ? arrowActiveColor : arrowInactiveColor;
    }
}
