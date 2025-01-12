using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public float speed;
    public float bulletLife;
    public float damage;

    public float timer;
    public float firingRate;
    public float cooldown;

    public GameObject bullet;
    public Transform stage;

    public PlayerMovement playerMovement;
    private GameObject spawnedBullet;
    public BossActions bossActions;

    public bool autoFire;
    public bool bombSpawner;
    public bool LRBombs;
    public bool gasterBlaster;

    public BulletSpawner[] linkedSpawners;
    public BulletSpawner[] followUps;
    public float followUpTimer;
    public float followUpTime;
    public bool justFired;


    // Start is called before the first frame update
    void Start()
    {
        justFired = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.rotating)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer >= firingRate && autoFire)
        {
            Fire();
            timer = 0;
        }

        if (followUps.Length > 0)
        {
            if (justFired)
            {
                followUpTimer += Time.deltaTime;

                if (followUpTimer > followUpTime)
                {
                    foreach (BulletSpawner spawner in followUps)
                    {
                        spawner.Fire();
                    }

                    justFired = false;
                    followUpTimer = 0;
                }
            }
        }
    }

    public void Fire()
    {
        if (bullet)
        {
            justFired = true;

            //Bomb
            if (bombSpawner)
            {
                int bombSpot = Random.Range(-2, 3);
                if (!LRBombs)
                {
                    spawnedBullet = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y + (bombSpot * 2), transform.position.z), transform.rotation, transform);
                }
                else spawnedBullet = Instantiate(bullet, new Vector3(transform.position.x + (bombSpot * 2), transform.position.y, transform.position.z), transform.rotation, transform);
                spawnedBullet.GetComponent<Bullet>().speed = speed;
                spawnedBullet.GetComponent<Bullet>().bulletLife = bulletLife;
                spawnedBullet.GetComponent<Bullet>().spot = bombSpot;
                spawnedBullet.GetComponent<Bullet>().isBomb = true;
                spawnedBullet.GetComponent<Bullet>().destroyOnHit = true;
                spawnedBullet.GetComponent<Bullet>().spawner = transform;
                spawnedBullet.GetComponent<Bullet>().playerMovement = playerMovement;
                spawnedBullet.GetComponent<Bullet>().bossActions = bossActions;
                spawnedBullet.GetComponent<Bullet>().LRBombs = LRBombs;
            }
            //Laser
            else if (gasterBlaster)
            {
                spawnedBullet = Instantiate(bullet, transform.position, transform.rotation, transform);
                spawnedBullet.GetComponent<Bullet>().bulletLife = bulletLife;
                spawnedBullet.GetComponent<Bullet>().damage = damage;
                spawnedBullet.GetComponent<Bullet>().destroyOnHit = false;
                spawnedBullet.GetComponent<Bullet>().playerMovement = playerMovement;
                spawnedBullet.GetComponent<Bullet>().bossActions = bossActions;
                spawnedBullet.GetComponent<Bullet>().spawner = transform;
            }
            //Normal Bullet
            else
            {
                spawnedBullet = Instantiate(bullet, transform.position, transform.rotation, transform);
                spawnedBullet.GetComponent<Bullet>().speed = speed;
                spawnedBullet.GetComponent<Bullet>().bulletLife = bulletLife;
                spawnedBullet.GetComponent<Bullet>().damage = damage;
                spawnedBullet.GetComponent<Bullet>().destroyOnHit = true;
                spawnedBullet.GetComponent<Bullet>().playerMovement = playerMovement;
                spawnedBullet.GetComponent<Bullet>().bossActions = bossActions;
                spawnedBullet.GetComponent<Bullet>().spawner = transform;
            }
        }
    }
}
