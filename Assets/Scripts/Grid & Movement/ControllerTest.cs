using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorController
{
    public static readonly Vector2 forwardMove = new Vector2(1, 1);
    public static readonly Vector2 backMove = new Vector2(-1, -1);
    public static readonly Vector2 leftMove = new Vector2(-1, 1);
    public static readonly Vector2 rightMove = new Vector2(1, -1);
    public static readonly Vector2 nwMove = new Vector2(1, 0);
    public static readonly Vector2 neMove = new Vector2(0, 1);
    public static readonly Vector2 swMove = new Vector2(0, -1);
    public static readonly Vector2 seMove = new Vector2(-1, 0);
}

public class ControllerTest : MonoBehaviour
{
    public float speed = 0;

    void Start()
    {

    }

    private void Update()
    {
        Vector2 moveVector = new Vector2(0, 0);

        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) //Doesnt move
        {
            moveVector = new Vector2(0, 0);
            speed = 0;
            Debug.Log("Not Moving");
        }
        else if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") > 0) //Moves Forward (x+y diagonal)
        {
            moveVector = VectorController.forwardMove;
            speed = 1 / Mathf.Sqrt(2);
            Debug.Log("Moving Forward");
        }
        else if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") < 0) //Moves Backward (-x-y diagonal)
        {
            moveVector = VectorController.backMove;
            speed = 1 / Mathf.Sqrt(2);
            Debug.Log("Moving Backwards");
        }
        else if (Input.GetAxis("Horizontal") < 0 && Input.GetAxis("Vertical") == 0) //Moves Left (-x+y diagonal)
        {
            moveVector = VectorController.leftMove;
            speed = 1 / Mathf.Sqrt(2);
            Debug.Log("Moving Left");
        }
        else if (Input.GetAxis("Horizontal") > 0 && Input.GetAxis("Vertical") == 0) //Moves Right (x-y diagonal)
        {
            moveVector = VectorController.rightMove;
            speed = 1 / Mathf.Sqrt(2);
            Debug.Log("Moving Right");
        }
        else if (Input.GetAxis("Horizontal") > 0 && Input.GetAxis("Vertical") > 0) //Moves Northwest (x axis)
        {
            moveVector = VectorController.nwMove;
            speed = 1;
            Debug.Log("Moving Northwest");
        }
        else if (Input.GetAxis("Horizontal") > 0 && Input.GetAxis("Vertical") < 0) //Moves Southwest (-y axis)
        {
            moveVector = VectorController.swMove;
            speed = 1;
            Debug.Log("Moving Southwest");
        }
        else if (Input.GetAxis("Horizontal") < 0 && Input.GetAxis("Vertical") > 0) //Moves Northeast (y axis)
        {
            moveVector = VectorController.neMove;
            speed = 1;
            Debug.Log("Moving Northeast");
        }
        else if (Input.GetAxis("Horizontal") < 0 && Input.GetAxis("Vertical") < 0) //Moves Southeast (-x axis)
        {
            moveVector = VectorController.seMove;
            speed = 1;
            Debug.Log("Moving Southeast");
        }

        this.transform.position += new Vector3(moveVector.x, 0, moveVector.y) * speed * Time.deltaTime;
    }
}
