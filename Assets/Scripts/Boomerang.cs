using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    public float flightTimer;
    public float flightTime;
    public float flightSpeed;
    //public Transform spawnPoint;
    public PlayerMovement playerMovement;
    public GameObject emptyObject;
    public GameObject trueSpawn;
    public Transform stage;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FlyAndReturn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator FlyAndReturn()
    {
        trueSpawn = Instantiate(emptyObject, transform.position, transform.rotation, stage);
        while (flightTimer < flightTime)
        {
            if (!playerMovement.rotating)
            {
                float x, y, z;

                x = flightTimer * flightSpeed * transform.right.x;
                y = flightTimer * flightSpeed * transform.right.y;
                z = flightTimer * flightSpeed * transform.right.z;

                transform.position = new Vector3(x + trueSpawn.transform.position.x, y + trueSpawn.transform.position.y, z + trueSpawn.transform.position.z);
                flightTimer += Time.deltaTime;
                yield return null;
            }
        }
        while (flightSpeed > 0)
        {
            if (!playerMovement.rotating)
            {
                flightSpeed -= 0.05f;
                float x, y, z;

                x = flightTimer * flightSpeed * transform.right.x;
                y = flightTimer * flightSpeed * transform.right.y;
                z = flightTimer * flightSpeed * transform.right.z;

                transform.position = new Vector3(x + trueSpawn.transform.position.x, y + trueSpawn.transform.position.y, z + trueSpawn.transform.position.z);
                flightTimer += Time.deltaTime;
                yield return null;
            }
        }
        while (transform.position != trueSpawn.transform.position)
        {
            if(!playerMovement.rotating)
            {
                if (flightSpeed < -10) flightSpeed = -10;
                flightSpeed -= 0.1f;
                transform.position = Vector3.MoveTowards(transform.position, trueSpawn.transform.position, 1f * Time.deltaTime);
                yield return null;
            }
        }
        Destroy(trueSpawn.gameObject);
        Destroy(gameObject);
    }
}
