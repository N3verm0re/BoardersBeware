using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Vector3 CameraRelativeVector;

    private void Start()
    {
        this.transform.position = player.transform.position + CameraRelativeVector;
    }

    private void Update()
    {
        this.transform.position = player.transform.position + CameraRelativeVector;
    }
}
