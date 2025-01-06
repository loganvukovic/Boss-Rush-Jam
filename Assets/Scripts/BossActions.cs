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

    public Quaternion startRotation;
    public Quaternion targetRotation;

    // Start is called before the first frame update
    void Start()
    {
        rotateTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
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

            //Round rotation to nearest multiple of 90
            if (!rotating && !stageRotating && tf.rotation.y % 90 != 0)
            {
                tf.rotation = Quaternion.Euler(0, Mathf.Round(tf.eulerAngles.y / 90f) * 90f, 0);
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
}
