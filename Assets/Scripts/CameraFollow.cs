using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject playerToFollow; //
    [SerializeField] private Vector3 distanceToFollow = new Vector3(0f, 2f, -5f); 
    [SerializeField] private float smoothSpeed = 0f; // 0 = sin suavidad, 1 = muy suave

    void Start()
    {
        if (distanceToFollow == Vector3.zero)
            distanceToFollow = transform.position - playerToFollow.transform.position;
        
        transform.position = playerToFollow.transform.position + distanceToFollow;
    }

    void LateUpdate() 
    {
        
        Vector3 desiredPosition = playerToFollow.transform.position + distanceToFollow;
        
        
        if (smoothSpeed > 0f)
        {
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = desiredPosition;
        }

        transform.LookAt(playerToFollow.transform);
    }

    /* [SerializeField] GameObject playerToFollow;
    Vector3 distanceToFollow;
    void Start()
    {
        distanceToFollow = transform.position - playerToFollow.transform.position;
    }
    void Update()
    {
        transform.position = playerToFollow.transform.position + distanceToFollow;
    } */
}
