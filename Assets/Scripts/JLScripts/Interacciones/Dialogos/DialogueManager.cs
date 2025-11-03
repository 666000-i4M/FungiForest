using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI References")]
    public GameObject dialoguePanel;
    public TMP_Text npcNameText;
    public TMP_Text introText;
    public Transform buttonContainer;
    public GameObject buttonPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenDialogue(DialogueData data)
    {
        dialoguePanel.SetActive(true);
        npcNameText.text = data.npcName;

        // Limpia botones anteriores
        foreach (Transform child in buttonContainer)
            Destroy(child.gameObject);

        // Inicia coroutine para mostrar texto inicial
        StartCoroutine(ShowTextThenButtons(data));
    }

    private IEnumerator ShowTextThenButtons(DialogueData data)
    {
        // Decide texto inicial (bienvenida o repetitivo)
        bool firstTime = !GameState.Instance.interactedNPCs.Contains(data.npcName);
        string textToShow = firstTime ? data.introText : string.IsNullOrEmpty(data.repeatText) ? data.introText : data.repeatText;

        // Escribe el texto letra por letra
        introText.text = "";
        foreach (char c in textToShow)
        {
            introText.text += c;
            yield return new WaitForSeconds(0.03f);
        }

        // Texto inicial ya mostrado, ahora lo ocultamos
        introText.text = "";

        // Mostrar botones después
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
                // Mostrar respuesta según botón
                introText.text = option.response;

                // Si el botón otorga un ítem
                if (option.grantsItem)
                    GameState.Instance.obtainedItems.Add(option.grantedItem);

                // Opcional: después de mostrar mensaje de agradecimiento/despedida, limpiar botones
                foreach (Transform child in buttonContainer)
                    Destroy(child.gameObject);
            });
        }

        // Registrar que ya interactuaste con el NPC
        GameState.Instance.interactedNPCs.Add(data.npcName);
    }

    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        introText.text = "";
        foreach (Transform child in buttonContainer)
            Destroy(child.gameObject);
    }
}
