using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game instance;
    void Awake()
    {
        instance = this;
    }
        
    public int score = 0;

    public SpriteRenderer levelSprite;
}
