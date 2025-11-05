using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI References")]
    public GameObject dialoguePanel;
    public TMP_Text npcNameText;
    public TMP_Text dialogueText;
    public Transform buttonContainer;
    public Button buttonPrefab;

    private Queue<string> sentences = new Queue<string>();
    private DialogueData currentDialogue;
    private Transform currentNPC;

    private bool isTyping = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        dialoguePanel.SetActive(false);
    }

    // Asignar NPC actual
    public void SetCurrentNPC(Transform npc)
    {
        currentNPC = npc;
        Debug.Log("NPC actual asignado: " + npc.name);
    }

    // Abrir diálogo
    public void OpenDialogue(DialogueData dialogueData)
    {
        if (dialogueData == null) return;

        currentDialogue = dialogueData;

        dialoguePanel.SetActive(true);
        npcNameText.text = dialogueData.npcName;
        dialogueText.text = "";

        sentences.Clear();

        // Encolar todas las líneas de introTexts
        if (dialogueData.introTexts != null && dialogueData.introTexts.Length > 0)
        {
            foreach (string line in dialogueData.introTexts)
                sentences.Enqueue(line);
        }

        StopAllCoroutines();
        StartCoroutine(DisplaySentencesCoroutine());
    }

    // Corrutina principal para mostrar todas las líneas
   /* private IEnumerator DisplaySentencesCoroutine()
    {
        // Ocultar botones mientras se muestran líneas
        foreach (Transform child in buttonContainer)
            Destroy(child.gameObject);

        while (sentences.Count > 0)
        {
            string line = sentences.Dequeue();
            yield return StartCoroutine(TypeSentence(line));

            // Esperar a click izquierdo o espacio para avanzar
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space));
        }

        // Mostrar botones al final del introText
        ShowOptions();
    }*/

     private IEnumerator DisplaySentencesCoroutine()
    {
        // Limpiar botones mientras mostramos líneas
        foreach (Transform child in buttonContainer)
            Destroy(child.gameObject);

        while (sentences.Count > 0)
        {
            string line = sentences.Dequeue();
            yield return StartCoroutine(TypeSentence(line));

            // Esperar a click izquierdo o espacio para avanzar
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space));
        }

        // ✅ Limpiar texto antes de mostrar botones
        dialogueText.text = "";

        // Mostrar botones al final del introText
        ShowOptions();
    }

    // Máquina de escribir
    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char c in sentence.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.02f);
        }
        isTyping = false;
    }
    


    // Mostrar botones dinámicos
    private void ShowOptions()
    {
        // Limpiar botones anteriores
        foreach (Transform child in buttonContainer)
            Destroy(child.gameObject);

        if (currentDialogue.options == null || currentDialogue.options.Length == 0) return;

        foreach (DialogueOption option in currentDialogue.options)
        {
            Button newButton = Instantiate(buttonPrefab, buttonContainer);
            TMP_Text buttonText = newButton.GetComponentInChildren<TMP_Text>();
            buttonText.text = option.text;

            // Validar si el botón está disponible
            bool canUse = true;
            if (option.requiresItem && !GameState.Instance.obtainedItems.Contains(option.requiredItem))
                canUse = false;

            if (option.requiresNPC && !GameState.Instance.interactedNPCs.Contains(option.requiredNPC))
                canUse = false;

            newButton.interactable = canUse;

            // Colores del botón
            ColorBlock colors = newButton.colors;
            colors.disabledColor = new Color(0.5f, 0.5f, 0.5f);   // gris
            colors.highlightedColor = new Color(0.3f, 0.7f, 1f);   // celeste
            newButton.colors = colors;

            // Listener del click
            newButton.onClick.AddListener(() =>
            {
                StartCoroutine(DisplayOptionResponse(option));
            });
        }
    }

    // Mostrar respuesta de botón (puede ser varias líneas)
 private IEnumerator DisplayOptionResponse(DialogueOption option)
{
    // Limpiar botones
    foreach (Transform child in buttonContainer)
        Destroy(child.gameObject);

    sentences.Clear();

    // Encolar todas las líneas del introText del botón
    foreach (string line in option.introTexts)
        sentences.Enqueue(line);

    // Mostrar todas las líneas
    yield return StartCoroutine(DisplaySentencesCoroutine());

        // Si es botón “Irse”, cerramos diálogo
        if (option.text.ToLower() == "irse")
        {
            EndDialogue();
            yield break;
        }
    // Otorgar ítems si corresponde
    if (option.grantsItems && option.grantedItems != null)
    {
        foreach (ItemData item in option.grantedItems)
        {
            if (item != null)
            {
                InventoryManager.Instance.AddItem(item);
                Debug.Log("Se otorgó ítem: " + item.itemName);
            }
        }
}


    // Mostrar botones otra vez si la conversación continúa
    ShowOptions();
}

    // Cerrar diálogo
    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        sentences.Clear();

        foreach (Transform child in buttonContainer)
            Destroy(child.gameObject);
    }
}
