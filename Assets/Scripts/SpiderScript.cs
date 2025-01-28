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
    public string direction;
    public PuppetManager puppetManager;

    // Start is called before the first frame update
    void Start()
    {
        cloneScript = GetComponent<CloneScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!puppetManager.fightStarted)
        {
            return;
        }

        if (cloneScript.side == "East" || cloneScript.side == "South")
        {
            movingUpDown = true;
        }
        else movingUpDown = false;

        direction = GetComponent<CloneScript>().spawnPoint.GetComponent<PuppetSpawn>().spiderDirection;

        if (direction == "Up")
        {
            if (movingPositive)
            {
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 0, 0);
            }
            else transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 180, 180);
        }
        else if (direction == "Down")
        {
            if (movingPositive)
            {
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 0, 180);
            }
            else transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 180, 180);
        }
        else if (direction == "Left")
        {
            if (movingPositive)
            {
                transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, -90);
            }
            else transform.rotation = Quaternion.Euler(180, transform.eulerAngles.y, -90);
        }
        else if (direction == "Right")
        {
            if (movingPositive)
            {
                transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 90);
            }
            else transform.rotation = Quaternion.Euler(180, transform.eulerAngles.y, 90);
        }

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
