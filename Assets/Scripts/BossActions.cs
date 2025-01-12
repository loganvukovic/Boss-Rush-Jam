using System.Collections;
using System.Collections.Generic;
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
    public float attackTimer;
    public float attackCooldown;

    // Start is called before the first frame update
    void Start()
    {
        rotateTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Round rotation to nearest multiple of 90
        if (!rotating && !stageRotating && tf.rotation.y % 90 != 0)
        {
            tf.rotation = Quaternion.Euler(0, Mathf.Round(tf.eulerAngles.y / 90f) * 90f, 0);
        }

        stageRotating = playerMovement.rotating;

        if (stageRotating)
        {
            return;
        }

        attackTimer += Time.deltaTime;
        if (attackTimer > attackCooldown)
        {
            PickAttack(Random.Range(0, spawners.Length));
        }

        if (canRotate)
        {
            rotateTimer += Time.deltaTime;

            if (rotateTimer > rotateCooldown)
            {
                if (Random.Range(0f, 1f) < 0.5f)
                {
                    Rotate(90);
                }
                else Rotate(-90);
            }
            if (rotating && timeElapsed < rotationDuration)
            {
                timeElapsed += Time.deltaTime;
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / rotationDuration);
                if (transform.rotation == targetRotation)
                {
                    timeElapsed = 0;
                    rotating = false;
                }
            }
        }
    }

    private void Rotate(float angle)
    {
        rotateTimer = 0f;
        startRotation = transform.rotation;
        targetRotation = startRotation * Quaternion.Euler(0, angle, 0);
        rotating = true;
    }

    private void PickAttack(int attack)
    {
        attackTimer = 0;
        spawners[attack].Fire();
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
}
