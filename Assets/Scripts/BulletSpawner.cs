using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public enum BulletType {Normal, Aimed, Bomb, Laser, Spear}
    public BulletType bulletType;

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

    public Vector3 aimedOffset;
    public float angle;
    public bool autoFire;
    public bool sideDependant;
    public string side;
    public bool bombSpawner;
    public bool LRBombs;
    public bool gasterBlaster;
    public bool stayOnBossRotate = false;
    public bool keepMovingOnBossMove = false;

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
        if (cooldown < 1)
        {
            cooldown = 1;
        }

        if (bulletType == BulletType.Aimed)
        {
            Vector3 playerDirection = (playerMovement.transform.position + aimedOffset - transform.position).normalized;
            angle = Mathf.Rad2Deg * Mathf.Atan2(playerDirection.y, playerDirection.x);
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        if (playerMovement.rotating)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer >= firingRate && autoFire)
        {
            if ((sideDependant && side == playerMovement.curSide) || !sideDependant)
            Fire();
            timer = 0;
        }

        if (justFired)
        {
            if (followUps.Length > 0)
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
            else if (bulletType == BulletType.Spear)
            {
                followUpTimer += Time.deltaTime;

                if (followUpTimer > followUpTime)
                {
                    justFired = false;
                    followUpTimer = 0;
                    if (spawnedBullet != null)
                    {
                        spawnedBullet.GetComponent<Animator>().SetBool("Extend", true);
                    }
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
            if (bulletType == BulletType.Bomb)
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
                spawnedBullet.GetComponent<Bullet>().keepMovingOnBossMove = keepMovingOnBossMove;
            }
            //Laser
            else if (bulletType == BulletType.Laser)
            {
                /*if (stayOnBossRotate)
                {
                    spawnedBullet = Instantiate(bullet, transform.position, transform.rotation, stage.transform);
                }
                else*/ spawnedBullet = Instantiate(bullet, transform.position, transform.rotation, transform);
                spawnedBullet.GetComponent<Bullet>().bulletLife = bulletLife;
                spawnedBullet.GetComponent<Bullet>().damage = damage;
                spawnedBullet.GetComponent<Bullet>().destroyOnHit = false;
                spawnedBullet.GetComponent<Bullet>().playerMovement = playerMovement;
                spawnedBullet.GetComponent<Bullet>().bossActions = bossActions;
                spawnedBullet.GetComponent<Bullet>().spawner = transform;
                spawnedBullet.GetComponent<Bullet>().keepMovingOnBossMove = keepMovingOnBossMove;
            }
            //Spear
            else if (bulletType == BulletType.Spear)
            {
                spawnedBullet = Instantiate(bullet, transform.position, transform.rotation, transform);
                spawnedBullet.GetComponent<Bullet>().bulletLife = bulletLife;
                spawnedBullet.GetComponent<Bullet>().damage = damage;
                spawnedBullet.GetComponent<Bullet>().destroyOnHit = false;
                spawnedBullet.GetComponent<Bullet>().playerMovement = playerMovement;
                spawnedBullet.GetComponent<Bullet>().bossActions = bossActions;
                spawnedBullet.GetComponent<Bullet>().spawner = transform;
                spawnedBullet.GetComponent<Bullet>().keepMovingOnBossMove = keepMovingOnBossMove;
            }
            //Normal Bullet
            else
            {
                spawnedBullet = Instantiate(bullet, transform.position, transform.rotation, stage.transform);
                spawnedBullet.GetComponent<Bullet>().speed = speed;
                spawnedBullet.GetComponent<Bullet>().bulletLife = bulletLife;
                spawnedBullet.GetComponent<Bullet>().damage = damage;
                spawnedBullet.GetComponent<Bullet>().destroyOnHit = true;
                spawnedBullet.GetComponent<Bullet>().playerMovement = playerMovement;
                spawnedBullet.GetComponent<Bullet>().bossActions = bossActions;
                spawnedBullet.GetComponent<Bullet>().spawner = transform;
                spawnedBullet.GetComponent<Bullet>().keepMovingOnBossMove = keepMovingOnBossMove;
            }
        }
    }
}
