using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderScript : MonoBehaviour
{
    public CloneScript cloneScript;
    public PlayerMovement playerMovement;
    public bool movingUpDown;
    public bool movingPositive;
    public float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        cloneScript = GetComponent<CloneScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cloneScript.side == "East" || cloneScript.side == "South")
        {
            movingUpDown = true;
        }
        else movingUpDown = false;

        if (!cloneScript.moving && !playerMovement.rotating && (!movingUpDown && (transform.position.x > 10 || transform.position.z > 10))
            || (movingUpDown && (transform.position.y > 17)))
        {
            movingPositive = false;
        }
        else if (!cloneScript.moving && !playerMovement.rotating && (!movingUpDown && (transform.position.x < -10 || transform.position.z < -10))
            || (movingUpDown && (transform.position.y < 7)))
        {
            movingPositive = true;
        }

        if(!cloneScript.moving && !playerMovement.rotating)
        {
            if (!movingUpDown)
            {
                if (((cloneScript.side == "North" ||  cloneScript.side == "South") && (playerMovement.curSide == "North" || playerMovement.curSide == "South"))
                    || ((cloneScript.side == "West" || cloneScript.side == "East") && (playerMovement.curSide == "West" || playerMovement.curSide == "East")))
                {
                    if (movingPositive)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(11, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);
                    }
                    else
                    {
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(-11, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);
                    }
                }
                else
                {
                    if (movingPositive)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, 11), moveSpeed * Time.deltaTime);
                    }
                    else
                    {
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, -11), moveSpeed * Time.deltaTime);
                    }
                }
            }
            else
            {
                if (movingPositive)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 18, transform.position.z), moveSpeed * Time.deltaTime);
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 6, transform.position.z), moveSpeed * Time.deltaTime);
                }
            }
        }
    }
}
