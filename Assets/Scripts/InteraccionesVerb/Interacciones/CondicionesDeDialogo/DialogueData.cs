using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueData", menuName = "Dialogue/Data")]
public class DialogueData : ScriptableObject
{
    public string npcName;

    [TextArea(3,5)]
    public string[] introTexts;          // ← varias líneas de introducción

    [TextArea(3,5)]
    public string repeatText;            // texto repetitivo si no cumple requisitos

    public DialogueOption[] options;     // botones y sus respuestas
}
