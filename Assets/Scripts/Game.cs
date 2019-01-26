using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static Game instance;
    void Awake()
    {
        instance = this;
    }

    public enum State
    {
        Begin,
        Game,
        End
    }

    public float timePerGame = 60.0f;
    public int score = 0;
    public State state = State.Game;

    public SpriteRenderer levelSprite;
    
    internal float timeRatioLeft;

    void Update()
    {
        timeRatioLeft = 1.0f - Time.timeSinceLevelLoad / timePerGame;
        if (timeRatioLeft < 0)
            state = State.End;

        if (state == State.End && Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
