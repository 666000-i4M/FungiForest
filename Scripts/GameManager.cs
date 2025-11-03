using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool hasMap = false;
    public List<string> missions = new();
    public GameObject mapCanvas;
    public GameObject playerMarker;
    public GameObject axulMarker;
    public Transform axulHouse; 

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }

    public void GiveMap()
    {
        hasMap = true;
        ShowPressMText(); // UI temporal
    }

    public void AddMission(string mission)
    {
        missions.Add(mission);
    }

    void ShowPressMText()
    {
        // Crea UI temporal: Text "Oprime M para ver" fade out en 5s
        Debug.Log("Mapa recibido! Presiona M");
    }
}
