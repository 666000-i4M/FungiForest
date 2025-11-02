using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public Button playButton;
    public Button optionsButton;
    public Button quitButton;
    public AudioSource buttonClickSound; 
    public float silenceDuration = 2f;

    void Start()
    {
        playButton.onClick.AddListener(PlayGame);
        optionsButton.onClick.AddListener(OpenOptions);
        quitButton.onClick.AddListener(QuitGame);
    }

    void PlayGame()
    {
        buttonClickSound.Play();
        Invoke("LoadIntroScene", buttonClickSound.clip.length - silenceDuration);
    }

    void LoadIntroScene()
    {
        SceneManager.LoadScene("IntroScene");
    }

    void OpenOptions()
    {
        buttonClickSound.Play();
       Invoke("LoadAboutGame", buttonClickSound.clip.length - silenceDuration);
    }

    void LoadAboutGame()
    {
        SceneManager.LoadScene("AboutGame"); 
    }

    void QuitGame()
    {
        buttonClickSound.Play();
        Application.Quit();
    }
}
