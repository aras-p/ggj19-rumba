using UnityEngine;

public class SpriteSwapper : MonoBehaviour
{
    public Sprite[] sprites;
    public int frameRate = 2;
    SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        var idx = (int)(Time.timeSinceLevelLoad * frameRate);
        sr.sprite = sprites[idx % sprites.Length];
    }
}
