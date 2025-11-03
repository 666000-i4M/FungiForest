using UnityEngine;

public class InteractableNPC : MonoBehaviour
{
    public DialogueData dialogueData;
    public float interactDistance = 3f;
    private Transform player;

    void Start() => player = GameObject.FindGameObjectWithTag("Player").transform;

    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) <= interactDistance)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                Debug.Log("H presionada cerca del NPC");
                DialogueManager.Instance.OpenDialogue(dialogueData);
            }
        }
    }



}
