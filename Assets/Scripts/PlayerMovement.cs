using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject stage;
    
    private float curSpeed;
    public float maxSpeed;
    public float accelerationSpeed;
    public float decelerationSpeed;
    private float horizontalInput;


    private Quaternion startRotation;
    private Quaternion targetRotation;
    private bool rotating;
    public float rotationDuration;
    private float timeElapsed;
    public string curSide;

    void Start()
    {
        curSide = "North";
        timeElapsed = 0;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !rotating)
        {
            startRotation = stage.transform.rotation;
            targetRotation = startRotation * Quaternion.Euler(0, 90, 0);
            rotating = true;
        }
        if (Input.GetKeyDown(KeyCode.E) & !rotating)
        {
            startRotation = stage.transform.rotation;
            targetRotation = startRotation * Quaternion.Euler(0, 90, 0);
            rotating = true;
        }

        if (rotating && timeElapsed < rotationDuration)
        {
            timeElapsed += Time.deltaTime;
            stage.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / rotationDuration);
            if (stage.transform.rotation == targetRotation)
            {
                timeElapsed = 0;
                rotating = false;
            }
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            return;
        }

        horizontalInput = Input.GetAxis("Horizontal");

        rb.velocity = new Vector3(-1 * maxSpeed * horizontalInput, rb.velocity.y, 0);
    }

    private void FixedUpdate()
    {

    }
}
