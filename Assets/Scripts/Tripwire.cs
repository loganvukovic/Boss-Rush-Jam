using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tripwire : MonoBehaviour
{
    public BulletSpawner linkedSpawner;
    public float life;
    public float timer;
    public PlayerMovement playerMovement;
    public BossActions bossActions;
    public GameObject tempSpawner;
    public GameObject spawner;
    public Transform stage;

    // Start is called before the first frame update
    void Start()
    {
        tempSpawner = Instantiate(spawner, linkedSpawner.transform.position, linkedSpawner.transform.rotation, stage);
        tempSpawner.GetComponent<BulletSpawner>().playerMovement = playerMovement;
        tempSpawner.GetComponent<BulletSpawner>().stage = stage;
        tempSpawner.GetComponent<BulletSpawner>().bossActions = bossActions;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (bossActions.moving)
        {
            Destroy(gameObject);
        }

        if (!playerMovement.rotating)
        {
            timer += Time.deltaTime;

            if (timer > life)
            {
                Destroy(gameObject);
            }
        }*/
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            tempSpawner.GetComponent<BulletSpawner>().Fire();
            //Destroy(tempSpawner);
            Destroy(gameObject);
        }
    }
}
