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

    public GameObject bullet;
    public Transform stage;

    public PlayerMovement playerMovement;
    private GameObject spawnedBullet;
    public BossActions bossActions;

    public bool autoFire;
    public bool bombSpawner;
    public bool gasterBlaster;

    // Start is called before the first frame update
    void Start()
    {
        
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
    }

    public void Fire()
    {
        if (bullet)
        {
            //Bomb
            if (bombSpawner)
            {
                int bombSpot = Random.Range(-2, 3);
                spawnedBullet = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y + (bombSpot * 2), transform.position.z), transform.rotation, transform);
                spawnedBullet.GetComponent<Bullet>().speed = speed;
                spawnedBullet.GetComponent<Bullet>().bulletLife = bulletLife;
                spawnedBullet.GetComponent<Bullet>().spot = bombSpot;
                spawnedBullet.GetComponent<Bullet>().isBomb = true;
                spawnedBullet.GetComponent<Bullet>().spawner = transform;
                spawnedBullet.GetComponent<Bullet>().playerMovement = playerMovement;
                spawnedBullet.GetComponent<Bullet>().bossActions = bossActions;
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
