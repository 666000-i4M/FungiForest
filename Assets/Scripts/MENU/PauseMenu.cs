using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [Header("=== Paneles ===")]
    public GameObject pausePanel;       // Panel con los botones
    public GameObject controlsPanel;    // Panel de controles

    [Header("=== Botones ===")]
    public Button resumeButton;
    public Button controlsButton;
    public Button exitButton;

    [Header("=== Input ===")]
    public PlayerInput playerInput;

    [Header("=== Configuración ===")]
    public string mainMenuSceneName = "MainMenu";

    private bool isPaused = false;
    private bool showingControls = false;

    void Start()
    {
        pausePanel.SetActive(false);
        controlsPanel.SetActive(false);
        SetupButtons();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (showingControls)
                HideControls();
            else
                TogglePause();
        }
    }

    void SetupButtons()
    {
        if (resumeButton) resumeButton.onClick.AddListener(Resume);
        if (controlsButton) controlsButton.onClick.AddListener(ShowControls);
        if (exitButton) exitButton.onClick.AddListener(ExitToMenu);
    }

    public void TogglePause()
    {
        if (isPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;

        // DESACTIVAR INPUT DEL JUGADOR
        if (playerInput != null)
            playerInput.DeactivateInput();

        pausePanel.SetActive(true);
    }

    public void Resume()
    {
        isPaused = false;

        // OCULTAR MENÚ
        pausePanel.SetActive(false);

        Time.timeScale = 1f;

        // REACTIVAR INPUT
        if (playerInput != null)
            playerInput.ActivateInput();
    }

    public void ShowControls()
    {
        showingControls = true;
        controlsPanel.SetActive(true);
    }

    public void HideControls()
    {
        showingControls = false;
        controlsPanel.SetActive(false);
    }

    public void ExitToMenu()
    {
        Time.timeScale = 1f;
        if (playerInput != null)
            playerInput.ActivateInput();
        SceneManager.LoadScene(mainMenuSceneName);
    }
}