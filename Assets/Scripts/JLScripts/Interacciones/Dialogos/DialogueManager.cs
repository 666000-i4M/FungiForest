using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject dialoguePanel;
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI introText;
    public Transform buttonContainer;
    public GameObject buttonPrefab;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void OpenDialogue(DialogueData data)
    {
        dialoguePanel.SetActive(true);
        npcNameText.text = data.npcName;
        introText.text = data.introText;

        foreach (Transform child in buttonContainer)
            Destroy(child.gameObject);

        foreach (DialogueOption option in data.options)
        {
            GameObject btnObj = Instantiate(buttonPrefab, buttonContainer);
            Button btn = btnObj.GetComponent<Button>();
            TMP_Text txt = btnObj.GetComponentInChildren<TMP_Text>();
            txt.text = option.text;

            bool unlocked = option.IsUnlocked();
            btn.interactable = unlocked;

            btn.onClick.AddListener(() =>
            {
                introText.text = option.response;
                if (option.grantsItem)
                    GameState.Instance.obtainedItems.Add(option.grantedItem);
            });
        }

        GameState.Instance.interactedNPCs.Add(data.npcName);
    }

    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}
