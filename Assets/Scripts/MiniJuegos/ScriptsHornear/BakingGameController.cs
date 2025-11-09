using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BakingGameController : MonoBehaviour
{
    [Header("UI")]
    public GameObject bakingPanel;
    public TextMeshProUGUI temperatureText;
    public GameObject successItem;
    public GameObject retryPanel;

    [Header("Configuración")]
    public int initialTemperature = 100;
    public int perfectMin = 175;        // ← Asegúrate de que sea 175
    public int perfectMax = 185;        // ← Asegúrate de que sea 185
    public ItemData cakeItemData;

    private int currentTemperature;
    private bool gameActive = false;

    void Start()
    {
        currentTemperature = initialTemperature;
        UpdateTemperatureUI();
        HideAllUI();
    }

    void Update()
    {
        if (!gameActive) return; // ← Si gameActive es false, no hace nada

        if (Input.GetKeyDown(KeyCode.C))
        {
            currentTemperature -= 5;
            UpdateTemperatureUI();
            CheckTemperature();
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            currentTemperature += 5;
            UpdateTemperatureUI();
            CheckTemperature();
        }
    }

    void UpdateTemperatureUI()
    {
        if (temperatureText != null)
            temperatureText.text = "Temperatura: " + currentTemperature + "°C";
    }

    void CheckTemperature()
    {
        if (currentTemperature >= perfectMin && currentTemperature <= perfectMax)
        {
            WinGame();
        }
        else if (currentTemperature > perfectMax + 10 || currentTemperature < perfectMin - 10)
        {
            // Por ejemplo, si perfectMin = 175, pierdes si < 165
            // Si perfectMax = 185, pierdes si > 195
            LoseGame();
        }
    }

    void WinGame()
    {
        gameActive = false;
        successItem.SetActive(true);

        if (cakeItemData != null)
            InventoryManager.Instance.AddItem(cakeItemData);

        Debug.Log("🎉 ¡Pastel horneado con éxito! 🎉");
    }

    void LoseGame()
    {
        gameActive = false;
        retryPanel.SetActive(true);
        Debug.Log("🔥 Has perdido. Temperatura fuera de rango.");
    }

    public void StartGame()
    {
        gameActive = true;
        currentTemperature = initialTemperature;
        UpdateTemperatureUI();
        successItem.SetActive(false);
        retryPanel.SetActive(false);
        bakingPanel.SetActive(true);
    }

    public void Retry()
    {
        gameActive = true; // ← ¡Muy importante!
        StartGame();
    }

    public void ExitGame()
    {
        bakingPanel.SetActive(false);
    }

    void HideAllUI()
    {
        bakingPanel.SetActive(false);
        successItem.SetActive(false);
        retryPanel.SetActive(false);
    }
}