using UnityEngine;

public class HumanControl : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float intervalMin = 0.3f;
    public float intervalMax = 2.0f;
    public GameObject[] droppings;
    public float dropInterval = 7.0f;
    Transform tr;
    Rigidbody2D rb;
    SpriteSwapper swapper;
    float timeToDecision;
    float timeToDrop;

    enum State
    {
        Moving,
        Idle,
        Count
    }

    State state = State.Idle;

    void Start()
    {
        swapper = GetComponentInChildren<SpriteSwapper>();
        tr = transform;
        rb = GetComponent<Rigidbody2D>();
        timeToDrop = Random.Range(dropInterval * 0.5f, dropInterval * 1.5f);
        NextDecision();
    }
    
    void Update()
    {
        var dt = Time.deltaTime;
        switch (state)
        {
        case State.Idle:
            rb.velocity = Vector2.zero;
            swapper.enabled = false;
            break;
        case State.Moving:
            var fwd = tr.up * moveSpeed;
            rb.velocity = fwd;
            swapper.enabled = true;
            if (timeToDrop <= 0 && droppings.Length > 0)
            {
                var dropIndex = Random.Range(0, droppings.Length);
                //var go =
                timeToDrop = Random.Range(dropInterval * 0.5f, dropInterval * 1.5f);
            }
            break;
        }

        timeToDrop -= dt;
        timeToDecision -= dt;
        if (timeToDecision <= 0)
            NextDecision();
    }

    void OnCollisionStay2D(Collision2D c)
    {
        NextDecision();
    }

    void NextDecision()
    {
        timeToDecision = Random.Range(intervalMin, intervalMax);
        var dir = tr.localEulerAngles.z;
        var newDir = dir + Random.Range(-30.0f, 30.0f);
        rb.MoveRotation(newDir);
        state = (State) Random.Range(0, (int)State.Count);
    }
}
