using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            SceneManager.LoadScene("SceneGame");
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
