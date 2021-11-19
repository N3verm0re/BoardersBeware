using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorController
{
    public readonly Vector2 forwardMove = new Vector2(1, 1);
    public readonly Vector2 backMove = new Vector2(-1, -1);
    public readonly Vector2 leftMove = new Vector2(-1, 0);
    public readonly Vector2 rigthMove = new Vector2(1, 0);
}

public class ControllerTest : MonoBehaviour
{
    public float speed = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        private Vector2 moveVector = new Vector2(0, 0); 

        if(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            moveVector = new Vector2(0, 0);
            speed = 0;
        }
        else if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") > 0)
        {
            moveVector = VectorController.forwardMove;
            speed = 0.5;
        }
        else if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") < 0)
        {
            moveVector = VectorController.backMove;
            speed = 0.5;
        }
        else if (Input.GetAxis("Horizontal") < 0 && Input.GetAxis("Vertical") == 0)
        {
            moveVector = VectorController.leftMove;
        }
        else if (Input.GetAxis("Horizontal") > 0 && Input.GetAxis("Vertical") == 0)
        {
            moveVector = VectorController.rightMove;
        }

        gameObject.position += 
    }
}
