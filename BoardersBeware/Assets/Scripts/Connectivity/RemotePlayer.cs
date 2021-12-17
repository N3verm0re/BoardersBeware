using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemotePlayer : MonoBehaviour
{
    public float speed = 1; 
    public Vector3 receivedPosition;
    public GameObject playerObject;

    private void Start()
    {
        
    }

    void Update()
    {
        if(playerObject != null)
        {
            if (playerObject.transform.position != receivedPosition)
            {
                playerObject.transform.position = Vector3.MoveTowards(playerObject.transform.position, receivedPosition, speed * Time.deltaTime);
            }
        }
    }
}
