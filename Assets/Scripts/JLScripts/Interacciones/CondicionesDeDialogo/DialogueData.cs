using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueData", menuName = "Dialogue/Data")]
public class DialogueData : ScriptableObject
{
    public string npcName;
    [TextArea(3,5)] public string introText;
    public DialogueOption[] options;
}
