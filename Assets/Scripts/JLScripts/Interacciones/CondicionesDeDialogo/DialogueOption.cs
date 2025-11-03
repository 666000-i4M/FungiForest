using UnityEngine;

[System.Serializable]
public class DialogueOption
{
    public string text;          // Texto del botón
    public string response;      // Texto que mostrará el NPC
    public bool requiresItem;    // ¿Requiere ítem?
    public string requiredItem;  // Nombre del ítem
    public bool requiresNPC;     // ¿Requiere haber hablado con alguien?
    public string requiredNPC;   // Nombre del NPC requerido
    public bool grantsItem;      // ¿Otorga un ítem?
    public string grantedItem;   // Ítem que otorga

    public bool IsUnlocked()
    {
        bool itemOK = !requiresItem || GameState.Instance.HasItem(requiredItem);
        bool npcOK = !requiresNPC || GameState.Instance.HasTalkedTo(requiredNPC);
        return itemOK && npcOK;
    }
}
