using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MUNGOController : MonoBehaviour , Interactable
{
    [Header("UI References")]
    public GameObject dialogueCanvas;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;

    //si

    [Header("Dialogues")]
    public string[] dialogues = {
        "Bienvenido de nuevo tu debes de ser Leo tu abuela me a dicho que estabas de regreso, hace mucho que no visitas el pueblo, te traigo un obsequio para que no te pierdas",
        "Espero que pases un agradable momento aquí, si ves algo raro o poco usual como brillo no tengas miedo yo me encargo de solucionar eso con Axul"
    };

    private int currentLine = 0;
    private bool hasTalked = false;
    private Animator animator; // Para anim enojado

    void Start()
    {
        animator = GetComponent<Animator>();
        nextButton.onClick.AddListener(NextLine);
        dialogueCanvas.SetActive(false);
    }

    public void Interact()
    {
        if (!hasTalked)
        {
            hasTalked = true;
            StartCoroutine(ShowDialogue());
        }
    }

    IEnumerator ShowDialogue()
    {
        dialogueCanvas.SetActive(true);
        for (int i = 0; i < dialogues.Length; i++)
        {
            dialogueText.text = dialogues[i];
            currentLine = i;
            yield return new WaitUntil(() => currentLine > i); // Espera clic Siguiente
        }
        // Anim enojado en última línea
        if (animator) animator.SetTrigger("Angry");
        dialogueCanvas.SetActive(false);
        // Llama funciones de siguiente etapa
        GameManager.Instance.GiveMap();
        GameManager.Instance.AddMission("Ver a Axul");
    }

    void NextLine()
    {
        currentLine++;
    }
}
