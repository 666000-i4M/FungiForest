using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionManager : MonoBehaviour
{
    public GameObject missionPanel; // Panel del bloc de notas
    public TextMeshProUGUI missionText; // Texto para mostrar misiones
    private bool[] missionsCompleted = new bool[1]; // Una misión por ahora
    private bool isMissionPanelOpen = false;

    void Start()
    {
        if (missionPanel != null) missionPanel.SetActive(false);
        UpdateMissionText();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            isMissionPanelOpen = !isMissionPanelOpen;
            missionPanel.SetActive(isMissionPanelOpen);
        }
    }

    public void CompleteMission(int missionIndex)
    {
        if (missionIndex < missionsCompleted.Length)
        {
            missionsCompleted[missionIndex] = true;
            UpdateMissionText();
        }
    }

    private void UpdateMissionText()
    {
        if (missionText != null)
        {
            missionText.text = missionsCompleted[0] ? 
                "Misión 1: Hablar con el NPC - Completada" : 
                "Misión 1: Hablar con el NPC - Incompleta";
        }
    }
}
