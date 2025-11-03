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

    private Coroutine typingCoroutine;

    [Header("Camera Settings")]
    public Vector3 cameraOffset = new Vector3(0, 2, -3); // distancia de la cámara al NPC
    public float cameraFocusDuration = 0.5f; // tiempo de transición de la cámara

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Abre el diálogo de un NPC con sus datos
    /// </summary>
    public void OpenDialogue(DialogueData data, Transform npcTransform)
    {
        dialoguePanel.SetActive(true);
        npcNameText.text = data.npcName;

        // Limpiar botones anteriores
        foreach (Transform child in buttonContainer)
            Destroy(child.gameObject);

        // Detener coroutine activa
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        // Iniciar coroutine de escritura de texto
        typingCoroutine = StartCoroutine(ShowTextThenButtons(data));

        // Centrar cámara en el NPC
        if (npcTransform != null)
            StartCoroutine(FocusCameraOnNPC(npcTransform));
    }

    /// <summary>
    /// Escribe texto letra por letra, espera 6 segundos y luego muestra los botones
    /// </summary>
    private IEnumerator ShowTextThenButtons(DialogueData data)
    {
        bool firstTime = !GameState.Instance.interactedNPCs.Contains(data.npcName);
        string textToShow = firstTime ? data.introText : string.IsNullOrEmpty(data.repeatText) ? data.introText : data.repeatText;

        // Escribir letra por letra
        introText.text = "";
        foreach (char c in textToShow)
        {
            introText.text += c;
            yield return new WaitForSeconds(0.03f);
        }

        // Mantener el texto visible 6 segundos
        yield return new WaitForSeconds(6f);

        // Limpiar texto antes de mostrar botones
        introText.text = "";

        // Mostrar botones de opciones
        foreach (DialogueOption option in data.options)
        {
            GameObject btnObj = Instantiate(buttonPrefab, buttonContainer);
            Button btn = btnObj.GetComponent<Button>();
            TMP_Text txt = btnObj.GetComponentInChildren<TMP_Text>();
            txt.text = option.text;

            btn.interactable = option.IsUnlocked();

            btn.onClick.AddListener(() =>
            {
                // Mostrar respuesta del NPC
                introText.text = option.response;

                // Otorgar ítem si corresponde
                if (option.grantsItem)
                    GameState.Instance.obtainedItems.Add(option.grantedItem);

                // Limpiar botones
                foreach (Transform child in buttonContainer)
                    Destroy(child.gameObject);
            });
        }

        // Registrar que ya interactuaste con el NPC
        GameState.Instance.interactedNPCs.Add(data.npcName);
    }

    /// <summary>
    /// Cierra el diálogo y limpia la UI
    /// </summary>
    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        introText.text = "";
        foreach (Transform child in buttonContainer)
            Destroy(child.gameObject);
    }

    /// <summary>
    /// Mueve y rota la cámara suavemente hacia el NPC
    /// </summary>
    private IEnumerator FocusCameraOnNPC(Transform npc)
    {
        Camera mainCam = Camera.main;
        Vector3 startPos = mainCam.transform.position;
        Quaternion startRot = mainCam.transform.rotation;

        Vector3 targetPos = npc.position + cameraOffset;
        Quaternion targetRot = Quaternion.LookRotation(npc.position - targetPos);

        float elapsed = 0f;
        while (elapsed < cameraFocusDuration)
        {
            elapsed += Time.deltaTime;
            mainCam.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / cameraFocusDuration);
            mainCam.transform.rotation = Quaternion.Slerp(startRot, targetRot, elapsed / cameraFocusDuration);
            yield return null;
        }

        mainCam.transform.position = targetPos;
        mainCam.transform.rotation = targetRot;
    }
}
