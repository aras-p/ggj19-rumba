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
        Game,
        End
    }

    public float timePerGame = 60.0f;
    public int score = 0;
    [NonSerialized]
    public State state = State.Game;

    public SpriteRenderer levelSprite;
    public AudioClip endSound;

    [NonSerialized]
    public float timeRatioLeft;

    void Update()
    {
        timeRatioLeft = 1.0f - Time.timeSinceLevelLoad / timePerGame;
        if (timeRatioLeft < 0 && state != State.End)
        {
            state = State.End;
            AudioSource.PlayClipAtPoint(endSound, Camera.main.transform.position);
        }

        if (state == State.End && Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene("SceneBegin");
    }
}
