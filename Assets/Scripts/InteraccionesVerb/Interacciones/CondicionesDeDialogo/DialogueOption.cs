[System.Serializable]
public class DialogueOption
{
    public string text;                 // Texto del botón
    //[TextArea(3,5)]
    public string[] introTexts;         // Varias líneas de respuesta (como introText)
    
    public bool requiresItem;    
    public string requiredItem;    
    public bool requiresNPC;     
    public string requiredNPC;   
    public bool grantsItems;               // indica si este botón da ítems
    public ItemData[] grantedItems;        // lista de ítems a otorgar


    public bool IsUnlocked()
    {
        bool itemOK = !requiresItem || GameState.Instance.HasItem(requiredItem);
        bool npcOK = !requiresNPC || GameState.Instance.HasTalkedTo(requiredNPC);
        return itemOK && npcOK;
    }
}
