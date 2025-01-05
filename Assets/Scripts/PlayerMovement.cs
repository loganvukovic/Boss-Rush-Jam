using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject stage;
    public LayerMask groundLayer;

    private float curSpeed;
    public float maxSpeed;
    public float accelerationSpeed;
    public float decelerationSpeed;
    private float horizontalInput;

    public bool canJump;
    public int maxJumps;
    public int remainingJumps;
    public float jumpSpeed;
    public Transform groundCheck;

    private Quaternion startRotation;
    private Quaternion targetRotation;
    [HideInInspector] public bool rotating;
    public float rotationDuration;
    private float timeElapsed;
    public string curSide;

    public bool attacking;

    private bool canDash = true;
    [HideInInspector] public bool isDashing;
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;

    private bool isSlamming;

    void Start()
    {
        curSide = "North";
        timeElapsed = 0;
    }
    void Update()
    {
        attacking = GetComponent<PlayerAttack>().attacking;
        isSlamming = GetComponent<PlayerAttack>().isSlamming;

        if (horizontalInput < 0)
            transform.localScale = new Vector3 (-1, 1, 1);

        else if (horizontalInput > 0)
            transform.localScale = new Vector3(1, 1, 1);

        if (!rotating)
        {
            if (Input.GetKeyDown(KeyCode.Q))
                Rotate(90);
            if (Input.GetKeyDown(KeyCode.E))
                Rotate(-90);
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

        if (isDashing)
        {
            return;
        }

        if (isGrounded())
        {
            canJump = true;
            remainingJumps = maxJumps;
        }
        else if (!isGrounded() && remainingJumps > 0)
        {
            canJump = true;
        }
        else canJump = false;

        if (canJump && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
            remainingJumps--;
        }


        if (attacking && isGrounded())
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }

        else
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && (isGrounded() || Input.GetAxis("Vertical") >= 0))
            {
                StartCoroutine(Dash());
            }
            else if (isSlamming)
            {
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
            }
            else
            {
                horizontalInput = Input.GetAxis("Horizontal");
                rb.velocity = new Vector3(-1 * maxSpeed * horizontalInput, rb.velocity.y, 0);
            }
        }
    }

    private void FixedUpdate()
    {

    }

    public bool isGrounded()
    {
        return Physics.OverlapSphere(groundCheck.position, 0.3f, groundLayer).Length > 0;
    }

    private void Rotate(float angle)
    {
        startRotation = stage.transform.rotation;
        targetRotation = startRotation * Quaternion.Euler(0, angle, 0);
        rotating = true;
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb.velocity = new Vector3(-transform.localScale.x * dashingPower, 0f, 0f);
        //audioSource.clip = dashSound;
        //audioSource.Play();
        yield return new WaitForSeconds(dashingTime);
        rb.velocity = new Vector3(rb.velocity.x, 0f, 0f);
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
