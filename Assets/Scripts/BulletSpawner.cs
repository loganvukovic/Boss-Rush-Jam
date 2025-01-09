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

    private GameObject spawnedBullet;

    public bool autoFire;
    public bool bombSpawner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
            if (bombSpawner)
            {
                int bombSpot = Random.Range(-2, 3);
                spawnedBullet = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y + (bombSpot * 2), transform.position.z), transform.rotation, stage);
                spawnedBullet.GetComponent<Bullet>().speed = speed;
                spawnedBullet.GetComponent<Bullet>().bulletLife = bulletLife;
                spawnedBullet.GetComponent<Bullet>().spot = bombSpot;
                spawnedBullet.GetComponent<Bullet>().isBomb = true;
                spawnedBullet.GetComponent<Bullet>().spawner = transform;
            }
            else
            {
                spawnedBullet = Instantiate(bullet, transform.position, transform.rotation, stage);
                spawnedBullet.GetComponent<Bullet>().speed = speed;
                spawnedBullet.GetComponent<Bullet>().bulletLife = bulletLife;
                spawnedBullet.GetComponent<Bullet>().damage = damage;
            }
        }
    }
}
