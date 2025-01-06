using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActions : MonoBehaviour
{
    public bool canRotate;
    public bool rotating;

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
                Debug.Log(Quaternion.Slerp(startRotation, targetRotation, timeElapsed / rotationDuration));
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / rotationDuration);
                if (transform.rotation == targetRotation)
                {
                    timeElapsed = 0;
                    rotating = false;
                }
            }

            //Round rotation to nearest multiple of 90
            if(!rotating && transform.rotation.y % 90 != 0)
            {
                transform.rotation = Quaternion.Euler(0, (transform.rotation.y / 90) * 90, 0);
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
