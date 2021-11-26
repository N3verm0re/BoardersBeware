using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorController
{
    public static readonly Vector2 forwardMove = new Vector2(1, 1);
    public static readonly Vector2 backMove = new Vector2(-1, -1);
    public static readonly Vector2 leftMove = new Vector2(-1, 0);
    public static readonly Vector2 rightMove = new Vector2(1, 0);
}

public class ControllerTest : MonoBehaviour
{
    public float speed = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        Vector2 moveVector = new Vector2(0, 0);

        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            moveVector = new Vector2(0, 0);
            speed = 0;
            Debug.Log("Not Moving");
        }
        else if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") > 0)
        {
            moveVector = VectorController.forwardMove;
            speed = 1 / Mathf.Sqrt(2);
            Debug.Log("Moving Forward");
        }
        else if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") < 0)
        {
            moveVector = VectorController.backMove;
            speed = 1 / Mathf.Sqrt(2);
            Debug.Log("Moving Backwards");
        }
        else if (Input.GetAxis("Horizontal") < 0 && Input.GetAxis("Vertical") == 0)
        {
            moveVector = VectorController.leftMove;
            speed = 1;
            Debug.Log("Moving Left");
        }
        else if (Input.GetAxis("Horizontal") > 0 && Input.GetAxis("Vertical") == 0)
        {
            moveVector = VectorController.rightMove;
            speed = 1;
            Debug.Log("Moving Right");
        }

        this.transform.position += new Vector3(moveVector.x, 0, moveVector.y) * speed * Time.deltaTime;
    }
}
