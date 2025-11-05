using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance;

    // Datos del jugador
    public HashSet<string> obtainedItems = new HashSet<string>();
    public HashSet<string> interactedNPCs = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool HasItem(string itemName) => obtainedItems.Contains(itemName);
    public bool HasTalkedTo(string npcName) => interactedNPCs.Contains(npcName);
}
