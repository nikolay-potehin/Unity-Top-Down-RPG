using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.HideAll();
    }

    public void OnPlayButton()
    {
        SceneManager.LoadScene("Main");
    }

    public void OnExitButton()
    {
        Application.Quit();
    }
}
