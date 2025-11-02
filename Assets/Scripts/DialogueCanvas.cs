using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class DialogueCanvas : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Button nextButton;
    System.Action onNext;

    public void ShowDialogue(string msg, System.Action next)
    {
        gameObject.SetActive(true);
        text.text = msg;
        onNext = next;
        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(() => onNext());
    }

    public void HideDialogue() { gameObject.SetActive(false); }
}
