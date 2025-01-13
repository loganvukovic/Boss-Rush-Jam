using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossActions : MonoBehaviour
{
    public Transform tf;
    public PlayerMovement playerMovement;

    public bool canRotate;
    public bool rotating;
    public bool stageRotating;

    public float rotateTimer;
    public float rotateCooldown;
    public float timeElapsed;
    public float rotationDuration;

    private Quaternion startRotation;
    private Quaternion targetRotation;

    public BulletSpawner[] spawners;
    public int sideDependentSpawners = 0;
    public BulletSpawner[] northSpawners;
    public BulletSpawner[] westSpawners;
    public BulletSpawner[] southSpawners;
    public BulletSpawner[] eastSpawners;
    public string playerSide;
    public float attackTimer;
    public float attackCooldown;

    public bool healing;

    public string curSide;

    // Start is called before the first frame update
    void Start()
    {
        rotateTimer = 0;
        curSide = "North";
    }

    // Update is called once per frame
    void Update()
    {
        playerSide = playerMovement.curSide;

        if (sideDependentSpawners > 0)
        {
            for (int i = 0; i < sideDependentSpawners; i++)
            {
                if (playerSide == "North")
                {
                    spawners[i] = northSpawners[i];
                }
                if (playerSide == "South")
                {
                    spawners[i] = southSpawners[i];
                }
                if (playerSide == "West")
                {
                    spawners[i] = westSpawners[i];
                }
                if (playerSide == "East")
                {
                    spawners[i] = eastSpawners[i];
                }
            }
        }

        if (GetComponentInChildren<BossScript>().healing)
        {
            healing = true;
            return;
        }
        else healing = false;

        stageRotating = playerMovement.rotating;

        if (canRotate)
        {
            rotateTimer += Time.deltaTime;
            if (rotateTimer > rotateCooldown)
            {
                if (Random.Range(0f, 1f) < 0.5f)
                {
                    Rotate(90);
                }
                else
                {
                    Rotate(-90);
                }
            }
            if (rotating && timeElapsed < rotationDuration)
            {
                timeElapsed += Time.deltaTime;
                transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / rotationDuration);

                if (Quaternion.Angle(transform.localRotation, targetRotation) < 0.1f)
                {
                    timeElapsed = 0;
                    rotating = false;
                }
            }
        }

        //Round rotation to nearest multiple of 90
        /*if (!rotating && !stageRotating && tf.rotation.y % 90 != 0)
        {
            Debug.Log((tf.eulerAngles.y / 90f) * 90f);
            tf.rotation = Quaternion.Euler(0, Mathf.Round(tf.eulerAngles.y / 90f) * 90f, 0);
        }*/

        if (stageRotating)
        {
            return;
        }

        attackTimer += Time.deltaTime;
        if (attackTimer > attackCooldown)
        {
            PickAttack(Random.Range(0, spawners.Length));
        }
    }

    private void Rotate(float angle)
    {
        if (curSide == "North")
        {
            if (angle == 90) curSide = "West";
            else curSide = "East";
        }
        else if (curSide == "West")
        {
            if (angle == 90) curSide = "South";
            else curSide = "North";
        }
        else if (curSide == "South")
        {
            if (angle == 90) curSide = "East";
            else curSide = "West";
        }
        else if (curSide == "East")
        {
            if (angle == 90) curSide = "North";
            else curSide = "South";
        }

        rotateTimer = 0f;
        startRotation = transform.localRotation;
        targetRotation = startRotation * Quaternion.Euler(0, angle, 0);
        rotating = true;
    }

    private void PickAttack(int attack)
    {
        attackTimer = 0;
        spawners[4].Fire();
        Debug.Log(spawners[attack].gameObject);
        attackCooldown = spawners[attack].cooldown;

        BulletSpawner[] linkedSpawners = spawners[attack].linkedSpawners;
        if(linkedSpawners.Length != 0)
        {
            foreach (BulletSpawner spawner in linkedSpawners)
            {
                spawner.Fire();
            }
        }
    }

    public void IncreaseSpeed(float speed)
    {
        foreach (BulletSpawner spawner in spawners)
        {
            spawner.cooldown = spawner.cooldown / speed;
        }
    }
}
