using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AboutMenuController : MonoBehaviour
{
    public Button backButton;
    public AudioSource buttonClickSound; 
    public float silenceDuration = 2f;

    void Start()
    {
        backButton.onClick.AddListener(BackToMenu);
    }

    void BackToMenu()
    {
        buttonClickSound.Play();
        Invoke("LoadMainMenu", buttonClickSound.clip.length - silenceDuration);
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); 
    }
}
