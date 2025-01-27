using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BulletSpawner : MonoBehaviour
{
    public enum BulletType {Normal, Aimed, Bomb, Laser, Spear, Lightning, Circle, Cone, Mouse}
    public BulletType bulletType;

    public float speed;
    public float bulletLife;
    public float damage;

    public float timer;
    public float firingRate;
    public float cooldown;
    public bool fireAtStart;

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
    public int bulletsInCircle;
    public int coneAngle;
    public int coneBulletCount;

    public BulletSpawner[] linkedSpawners;
    public BulletSpawner[] followUps;
    public float followUpTimer;
    public float followUpTime;
    public bool justFired;
    public BulletSpawner lightningConnector1;
    public BulletSpawner lightningConnector2;


    // Start is called before the first frame update
    void Start()
    {
        justFired = false;
        if (fireAtStart)
        {
            Fire();
        }
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

        /*if (bulletType == BulletType.Lightning)
        {
            Vector3 lightningDirection = (lightningConnector2.transform.position - lightningConnector1.transform.position).normalized;
            Debug.Log(angle);
            angle = Mathf.Rad2Deg * Mathf.Atan2(lightningDirection.y, lightningDirection.x);
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
            transform.position = lightningConnector1.transform.position;
        }*/

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
            followUpTimer += Time.deltaTime;
            if (followUps.Length > 0)
            {
                if (followUpTimer > followUpTime)
                {
                    foreach (BulletSpawner spawner in followUps)
                    {
                        spawner.Fire();
                    }
                }
            }
            if (bulletType == BulletType.Spear)
            {
                if (followUpTimer > followUpTime)
                {
                    if (spawnedBullet != null)
                    {
                        spawnedBullet.GetComponent<Animator>().SetTrigger("Extend");
                    }
                }
            }
            if (followUpTimer > followUpTime)
            {
                justFired = false;
                followUpTimer = 0;
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
            else if (bulletType == BulletType.Laser || bulletType == BulletType.Lightning)
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
            else if (bulletType == BulletType.Circle)
            {
                float angleIncrement = 360f / bulletsInCircle;
                Quaternion bulletRotation;
                for (int i = 0; i < bulletsInCircle; i++)
                {
                    float angle = angleIncrement * i;
                    if (playerMovement.curSide == "West" || playerMovement.curSide == "East")
                    {
                        bulletRotation = Quaternion.Euler(0f, 0f, angle);
                    }
                    else
                    {
                        bulletRotation = Quaternion.Euler(0f, 90f, angle);
                    }
                    //bulletRotation = Quaternion.Euler(0f, 0f, angle);
                    spawnedBullet = Instantiate(bullet, transform.position, bulletRotation, transform);
                    spawnedBullet.GetComponent<Bullet>().speed = speed;
                    spawnedBullet.GetComponent<Bullet>().bulletLife = bulletLife;
                    spawnedBullet.GetComponent<Bullet>().damage = damage;
                    spawnedBullet.GetComponent<Bullet>().destroyOnHit = true;
                    spawnedBullet.GetComponent<Bullet>().playerMovement = playerMovement;
                    spawnedBullet.GetComponent<Bullet>().bossActions = bossActions;
                    spawnedBullet.GetComponent<Bullet>().spawner = transform;
                }
            }
            else if (bulletType == BulletType.Cone)
            {
                float startAngle = transform.eulerAngles.z - (coneAngle / 2);
                float angleIncrement = coneAngle / (coneBulletCount - 1);
                for (int i = 0; i < coneBulletCount; i++)
                {
                    float angle = startAngle + (angleIncrement * i);
                    Quaternion bulletRotation;
                    if (((playerMovement.curSide == "West" || playerMovement.curSide == "East") && (GetComponentInParent<CloneScript>().side == "West" || GetComponentInParent<CloneScript>().side == "East"))
                        || ((playerMovement.curSide == "North" || playerMovement.curSide == "South") && (GetComponentInParent<CloneScript>().side == "North" || GetComponentInParent<CloneScript>().side == "South")))
                    {
                        bulletRotation = Quaternion.Euler(0f, 0f, angle);
                    }
                    else
                    {
                        bulletRotation = Quaternion.Euler(0f, 90f, angle);
                    }
                    spawnedBullet = Instantiate(bullet, transform.position, bulletRotation, stage.transform);
                    spawnedBullet.GetComponent<Bullet>().speed = speed;
                    spawnedBullet.GetComponent<Bullet>().bulletLife = bulletLife;
                    spawnedBullet.GetComponent<Bullet>().damage = damage;
                    spawnedBullet.GetComponent<Bullet>().destroyOnHit = true;
                    spawnedBullet.GetComponent<Bullet>().playerMovement = playerMovement;
                    spawnedBullet.GetComponent<Bullet>().bossActions = bossActions;
                    spawnedBullet.GetComponent<Bullet>().spawner = transform;
                    spawnedBullet.GetComponent<Bullet>().inCone = true;
                    spawnedBullet.GetComponent<Bullet>().throwPoint = GetComponentInParent<CloneScript>().spawnPoint.GetComponent<PuppetSpawn>().throwPoints[i].transform;
                }
                //StartCoroutine(DetachSpawner());
            }
            else if (bulletType == BulletType.Mouse)
            {
                spawnedBullet = Instantiate(bullet, transform.position, transform.rotation, stage.transform);
                spawnedBullet.GetComponent<MouseScript>().speed = speed;
                spawnedBullet.GetComponent<MouseScript>().chasingTime = bulletLife;
                spawnedBullet.GetComponent<MouseScript>().bossActions = bossActions;
                spawnedBullet.GetComponent<MouseScript>().playerMovement = playerMovement;
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

    IEnumerator DetachSpawner()
    {
        Transform spawner = transform.parent;
        Quaternion angle = transform.rotation;
        transform.parent = stage.transform;
        yield return new WaitForSeconds(bulletLife);
        transform.position = spawner.position;
        transform.parent = spawner;
        transform.rotation = angle;
    }
}
