using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class IntroController : MonoBehaviour
{
    public TextMeshProUGUI storyText;
    public GameObject controlsPanel;
    public Button startButton;
    public AudioSource ambientSound; // Referencia al AudioSource

    void Start()
    {
        StartCoroutine(PlayIntroSequence());
    }

    IEnumerator PlayIntroSequence()
    {
        StartCoroutine(FadeAudio(ambientSound, 0f, 0.5f, 3f)); 

        yield return StartCoroutine(FadeText(storyText, 0f, 1f, 3f));
        
        yield return new WaitForSeconds(5f);
        
        controlsPanel.SetActive(true);
        yield return StartCoroutine(FadePanel(controlsPanel.GetComponent<Image>(), 0f, 1f, 1f));
        
        yield return new WaitForSeconds(3f);
        
        startButton.gameObject.SetActive(true);
        startButton.onClick.AddListener(StartGame);
    }

    IEnumerator FadeText(TextMeshProUGUI text, float startAlpha, float endAlpha, float duration)
    {
        Color color = text.color;
        color.a = startAlpha;
        text.color = color;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            text.color = color;
            yield return null;
        }
        color.a = endAlpha;
        text.color = color;
    }

    IEnumerator FadePanel(Image panel, float startAlpha, float endAlpha, float duration)
    {
        Color color = panel.color;
        color.a = startAlpha;
        panel.color = color;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            panel.color = color;
            yield return null;
        }
        color.a = endAlpha;
        panel.color = color;
    }

    IEnumerator FadeAudio(AudioSource audio, float startVolume, float endVolume, float duration)
    {
        audio.volume = startVolume;
        audio.Play();
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            audio.volume = Mathf.Lerp(startVolume, endVolume, elapsed / duration);
            yield return null;
        }
        audio.volume = endVolume;
    }

    void StartGame()
    {
        SceneManager.LoadScene("MainScene"); 
    }
}
