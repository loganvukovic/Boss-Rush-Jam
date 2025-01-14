using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletLife = 1f;
    public float rotation = 0f;
    public float speed = 1f;
    public float damage = 5f;

    public float timer = 0f;
    private Vector3 spawnPoint;
    public bool isBomb;
    public bool LRBombs;
    public bool isBox = false;
    public bool destroyOnHit;
    public bool keepMovingOnBossMove;
    public int spot;
    public GameObject bombBox;
    public GameObject spawn;
    public GameObject spawnedBox;
    public Transform spawner;
    public PlayerMovement playerMovement;
    public BossActions bossActions;

    // Start is called before the first frame update
    void Start()
    {
        //spawn = Instantiate(spawn, transform.position, transform.rotation);

        if (isBomb)
        {
            if (!LRBombs)
            {
                for (int i = -2; i < 3; i++)
                {
                    if (spot != i)
                    {
                        spawnedBox = Instantiate(bombBox, new Vector3(spawner.position.x, spawner.position.y + (i * 2), spawner.position.z), transform.rotation, transform);
                        spawnedBox.GetComponent<Bullet>().destroyOnHit = true;
                        spawnedBox.GetComponent<Bullet>().bulletLife = bulletLife;
                        spawnedBox.GetComponent<Bullet>().spawner = spawner;
                        spawnedBox.GetComponent<Bullet>().playerMovement = playerMovement;
                        spawnedBox.GetComponent<Bullet>().bossActions = bossActions;
                        spawnedBox.GetComponent<Bullet>().isBox = true;
                    }
                }
            }
            else
            {
                for (int i = -6; i < 10; i++)
                {
                    if (spot != i)
                    {
                        if ((bossActions.curSide == "North" || bossActions.curSide == "South") && (playerMovement.curSide == "North" || playerMovement.curSide == "South")
                            || (bossActions.curSide == "West" || bossActions.curSide == "East") && (playerMovement.curSide == "West" || playerMovement.curSide == "East"))
                        {
                            spawnedBox = Instantiate(bombBox, new Vector3(spawner.position.x + (i * 2), spawner.position.y, spawner.position.z), transform.rotation, transform);
                        }
                        else
                        {
                            spawnedBox = Instantiate(bombBox, new Vector3(transform.position.x, spawner.position.y, spawner.position.z + (i * 2)), transform.rotation, transform);
                        }
                        spawnedBox.GetComponent<Bullet>().destroyOnHit = true;
                        spawnedBox.GetComponent<Bullet>().bulletLife = bulletLife;
                        spawnedBox.GetComponent<Bullet>().spawner = transform;
                        spawnedBox.GetComponent<Bullet>().playerMovement = playerMovement;
                        spawnedBox.GetComponent<Bullet>().bossActions = bossActions;
                        spawnedBox.GetComponent<Bullet>().isBox = true;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(bossActions.healing)
        {
            Destroy(gameObject);
        }

        if (isBomb)
        {
            spawnPoint = new Vector3(spawner.transform.position.x, spawner.transform.position.y + (spot * 2), spawner.transform.position.z);
        }
        else spawnPoint = spawner.transform.position;

        if (playerMovement.rotating || (bossActions.rotating && !keepMovingOnBossMove))
            return;

        if (timer > bulletLife)
            Destroy(this.gameObject);
        timer += Time.deltaTime;
        if (!isBox)
        {
            transform.position = Movement(timer);
        }
    }

    private Vector3 Movement(float timer)
    {
        float x, y, z;

            x = timer * speed * transform.right.x;
            y = timer * speed * transform.right.y;
            z = timer * speed * transform.right.z;

        return new Vector3(x + spawnPoint.x, y + spawnPoint.y, z + spawnPoint.z);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && !other.GetComponentInParent<PlayerMovement>().tookDamage && !playerMovement.rotating && !bossActions.rotating)
        {
            other.GetComponentInParent<PlayerMovement>().curHealth -= damage;
            other.GetComponentInParent<PlayerMovement>().tookDamage = true;
            if (destroyOnHit)
            {
                Destroy(this.gameObject);
            }
        }
        if (other.tag == "PlayerHB" && this.tag == "Destroyable")
        {
            Destroy(this.gameObject);
        }
    }
}
