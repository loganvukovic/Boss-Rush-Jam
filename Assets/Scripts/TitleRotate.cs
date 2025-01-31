using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleRotate : MonoBehaviour
{
    public GameObject tower;
    public int rotationDirection;
    public bool rotating;
    public Quaternion startRotation;
    public Quaternion targetRotation;
    public float timeElapsed;
    public float rotationDuration;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("RotationDirection"))
        {
            rotationDirection = PlayerPrefs.GetInt("RotationDirection");
        }
        else rotationDirection = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            InvertRotation();
        }

        if (!rotating)
        {
            if (Input.GetKeyDown(KeyCode.E))
                Rotate(90 * rotationDirection);
            if (Input.GetKeyDown(KeyCode.Q))
                Rotate(-90 * rotationDirection);
        }

        if (rotating && timeElapsed < rotationDuration)
        {
            timeElapsed += Time.deltaTime;
            tower.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / rotationDuration);
            if (tower.transform.rotation == targetRotation)
            {
                timeElapsed = 0;
                rotating = false;
            }
        }
    }

    private void Rotate(float angle)
    { 
        startRotation = tower.transform.rotation;
        targetRotation = startRotation * Quaternion.Euler(0, angle, 0);
        rotating = true;
    }

    public void InvertRotation()
    {
        if (rotationDirection == 1) rotationDirection = -1;
        else rotationDirection = 1;
        PlayerPrefs.SetInt("RotationDirection", rotationDirection);
    }
}
