using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    // player
    
    Transform player;
    void Start() { player = GameManager.Instance.playerMarker.transform; gameObject.SetActive(false); }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            gameObject.SetActive(!gameObject.activeSelf);
            if (GameManager.Instance.hasMap)
            {
                // Actualiza posiciones simples
                Vector3 playerPos = Camera.main.transform.position;
                player.localPosition = new Vector3(0, 0, 0); // Centro
                GameManager.Instance.axulMarker.transform.localPosition = new Vector3(150, 50, 0); // Derecha arriba
            }
        }
    }
}
