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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (bossActions.moving)
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
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            linkedSpawner.Fire();
            Destroy(gameObject);
        }
    }
}
