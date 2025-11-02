using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionManager : MonoBehaviour
{
    public float interactionDistance = 10f;
    public Camera mainCamera;
    [SerializeField] public Image interactionImage;
    public MissionManager missionManager;
    private LayerMask interactableLayer;

    void Start()
    {
        mainCamera = Camera.main;
        if (interactionImage != null)
            interactionImage.gameObject.SetActive(false);
        interactableLayer = ~LayerMask.GetMask("Player");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactableLayer))
            {
                if (hit.collider.CompareTag("NPC"))
                {
                    if (interactionImage != null)
                        interactionImage.gameObject.SetActive(true); // Mostrar la imagen
                    if (missionManager != null)
                        missionManager.CompleteMission(0);
                }
            }
            else
            {
                Debug.Log("No se detectó ningún objeto en el raycast");
            }
        }
        if (Input.GetKeyUp(KeyCode.E) && interactionImage != null)
        {
            interactionImage.gameObject.SetActive(false); 
        }
    }
}
