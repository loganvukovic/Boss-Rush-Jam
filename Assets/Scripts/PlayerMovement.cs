using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject stage;
    public Animator animator;
    public LayerMask groundLayer;

    private float curSpeed;
    public float maxSpeed;
    public float accelerationSpeed;
    public float decelerationSpeed;
    private float horizontalInput;
    private float verticalInput;

    public bool canJump;
    public int maxJumps;
    public bool isJumping;
    public int remainingJumps;
    public float jumpSpeed;
    public float jumpForceTime;
    public Transform groundCheck;
    public bool onPassable;

    private Quaternion startRotation;
    private Quaternion targetRotation;
    [HideInInspector] public bool rotating;
    public bool canRotate;
    public float rotationDuration;
    private float timeElapsed;
    public string curSide;

    public bool attacking;
    private int curCombo;

    private bool canDash = true;
    [HideInInspector] public bool isDashing;
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;

    private bool isSlamming;

    public GameObject projectileSpawner;

    public float curHealth;
    public float maxHealth;
    public bool tookDamage = false;
    private float damageTimer;
    public float invincibilityDuration;
    public float flickerTimer;
    public float flickerDuration;
    public MeshRenderer meshRenderer;
    public Image healthBar;
    public bool canMove;

    public bool inBubble;
    public bool floating;

    void Start()
    {
        curSide = "North";
        timeElapsed = 0;
        curHealth = maxHealth;
        meshRenderer = GetComponent<MeshRenderer>();
        canMove = true;
        canRotate = true;
        inBubble = false;
    }
    void Update()
    {
        if (!canMove)
        {
            return;
        }

        curCombo = GetComponent<PlayerAttack>().curCombo;
        attacking = GetComponent<PlayerAttack>().attacking;
        isSlamming = GetComponent<PlayerAttack>().isSlamming;

        if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-1, -1, 1);
            projectileSpawner.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        else if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            projectileSpawner.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        healthBar.fillAmount = curHealth / maxHealth;
        if (tookDamage)
        {
            damageTimer += Time.deltaTime;
            flickerTimer += Time.deltaTime;
        }

        if (damageTimer >= invincibilityDuration)
        {
            tookDamage = false;
            damageTimer = 0f;
            flickerTimer = 0f;
            meshRenderer.enabled = true;
        }

        if (tookDamage && flickerTimer >= flickerDuration)
        {
            flickerTimer = 0f;
            if (meshRenderer.enabled)
            {
                meshRenderer.enabled = false;
            }
            else meshRenderer.enabled = true;
        }

        if (!rotating && canRotate)
        {
            if (Input.GetKeyDown(KeyCode.Q))
                Rotate(90);
            if (Input.GetKeyDown(KeyCode.E))
                Rotate(-90);
        }

        if (rotating && timeElapsed < rotationDuration)
        {
            rb.mass = 0f;
            timeElapsed += Time.deltaTime;
            stage.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / rotationDuration);
            if (stage.transform.rotation == targetRotation)
            {
                timeElapsed = 0;
                rotating = false;
                rb.mass = 10f;
            }
            rb.velocity = new Vector3(0, 0, 0);
            return;
        }

        if (isDashing)
        {
            return;
        }

        /*if (isGrounded())
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
            if (!onPassable || (!Input.GetKey(KeyCode.S) || !Input.GetKey(KeyCode.DownArrow)))
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
                remainingJumps--;
            }
        }*/

        if (floating)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            rb.velocity = new Vector3(-1 * maxSpeed * horizontalInput, maxSpeed * verticalInput, 0);
            return;
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
        if (isDashing)
            return;

        if (rotating)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        if (canJump && Input.GetKey(KeyCode.Space))
        {
            remainingJumps--;
            isJumping = true;
            animator.SetTrigger("Jump");
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
            if (jumpForceTime > 9f)
            {
                isJumping = false;
                rb.velocity = new Vector3(rb.velocity.x, 2f, rb.velocity.z);
            }
            else
            {
                jumpForceTime += .3f;
            }
        }

        if (isJumping && (!Input.GetKey(KeyCode.Space) || isSlamming))
        {
            isJumping = false;
            rb.velocity = new Vector3(rb.velocity.x, 2f, rb.velocity.z);
        }

        if (isGrounded() && !isJumping)
        {
            remainingJumps = maxJumps;
            if (!Input.GetKey(KeyCode.Space))
            {
                canJump = true;
                jumpForceTime = 0f;
            }
        }
        else if (!isGrounded() && remainingJumps > 0 && !Input.GetKey(KeyCode.Space) && !isSlamming)
        {
            canJump = true;
            jumpForceTime = 0f;
        }
        else canJump = false;

        //animtor stuff
        animator.SetBool("Grounded", isGrounded());
        animator.SetFloat("Speed", horizontalInput);

    }

    public bool isGrounded()
    {
        return Physics.OverlapSphere(groundCheck.position, 0.3f, groundLayer).Length > 0;
    }

    private void Rotate(float angle)
    {
        if (curSide == "North")
        {
            if (angle == -90) curSide = "West";
            else curSide = "East";
        }
        else if (curSide == "West")
        {
            if (angle == -90) curSide = "South";
            else curSide = "North";
        }
        else if (curSide == "South")
        {
            if (angle == -90) curSide = "East";
            else curSide = "West";
        }
        else if (curSide == "East")
        {
            if (angle == -90) curSide = "North";
            else curSide = "South";
        }

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

    /*private void OnColliderStay(Collider other)
    {
        Debug.Log("aaaaa");
        if (other.GetComponent<PassablePlatform>() != null)
        {
            onPassable = true;

            if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
            {
                other.GetComponent<Collider>().enabled = false;
            }
        }
    }*/

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Bubble" && !rotating && !inBubble)
        {
            StartCoroutine(EnterBubble(other));
        }
    }

    public IEnumerator EnterBubble(Collider bubble)
    {
        canRotate = false;
        bubble.GetComponent<Bullet>().bulletLife = 999999f;
        inBubble = true;
        Vector3 bubblePos = new Vector3(bubble.transform.position.x, bubble.transform.position.y - 1.1f, transform.position.z);
        while (transform.position != bubblePos)
        {
            transform.position = Vector3.MoveTowards(transform.position, bubblePos, 1f);
            yield return new WaitForSeconds(0.01f);
        }
        canRotate = true;
        bubble.transform.parent = transform;
        bubble.GetComponent<Bullet>().playerInBubble = true;
        if (bubble.GetComponent<Bullet>().floatInBubble)
        {
            floating = true;
        }
    }
}
