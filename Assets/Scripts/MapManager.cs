using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public GameObject mapPanel;
    public Transform player;
    public Image playerDot;
    public Vector2 mapBoundsMin = new Vector2(-100f, -100f);
    public Vector2 mapBoundsMax = new Vector2(100f, 100f);
    private bool isMapOpen = false;

    void Start()
    {
        if (mapPanel != null) mapPanel.SetActive(false);
        if (playerDot != null) playerDot.gameObject.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            isMapOpen = !isMapOpen;
            mapPanel.SetActive(isMapOpen);
            if (player.GetComponent<PlayerMovement>())
                player.GetComponent<PlayerMovement>().enabled = !isMapOpen;
            if (isMapOpen && playerDot != null)
                playerDot.gameObject.SetActive(true);
            Debug.Log("Mapa abierto: " + isMapOpen);
        }

        if (isMapOpen && playerDot != null && mapPanel != null)
        {
            RectTransform panelRect = mapPanel.GetComponent<RectTransform>();
            float x = Mathf.Lerp(0, panelRect.rect.width, 
                (player.position.x - mapBoundsMin.x) / (mapBoundsMax.x - mapBoundsMin.x));
            float y = Mathf.Lerp(0, panelRect.rect.height, 
                (player.position.z - mapBoundsMin.y) / (mapBoundsMax.y - mapBoundsMin.y));
            playerDot.rectTransform.anchoredPosition = new Vector2(x, y);
            Debug.Log($"Player position: {player.position}, PlayerDot anchored position: {new Vector2(x, y)}, MapPanel size: {panelRect.rect.size}");
        }
    }
}
