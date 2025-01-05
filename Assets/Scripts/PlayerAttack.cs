using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject[] hitboxes;

    public Rigidbody rb;

    private bool rotating;
    public float attackTimer;
    public float attackCooldown;
    public float comboTimer;
    public bool attacking;

    public int curCombo;
    public float combo1Length;
    public float combo2Length;
    public float combo3Length;

    public bool isGrounded;
    public bool isDashing;

    public bool isSlamming;
    public float slamSpeed;

    void Start()
    {
        foreach (GameObject hitbox in hitboxes)
        {
            if (hitbox != null)
            {
                hitbox.SetActive(false);
            }
        }

    }

    void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer > combo1Length)
        {
            hitboxes[0].SetActive(false);
            hitboxes[4].SetActive(false);
            hitboxes[5].SetActive(false);
        }
        if (attackTimer > combo2Length)
        {
            hitboxes[1].SetActive(false);
        }
        if (attackTimer > combo3Length)
        {
            hitboxes[2].SetActive(false);
        }

        if (attackTimer > attackCooldown)
        {
            attacking = false;
        }

        if (attackTimer > comboTimer)
        {
            curCombo = 0;
        }

        isDashing = GetComponent<PlayerMovement>().isDashing;
        isGrounded = GetComponent<PlayerMovement>().isGrounded();
        rotating = GetComponent<PlayerMovement>().rotating;
        if (isGrounded)
        {
            isSlamming = false;
            hitboxes[3].SetActive(false);
        }

        if (rotating)
            return;

        if (Input.GetKeyDown(KeyCode.K) && !attacking && attackTimer > attackCooldown && !isDashing && !isSlamming)
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetAxis("Vertical") < 0)
        {
            GroundSlam();
        }

    }

    private void Attack()
    {
        if (Input.GetAxis("Vertical") > 0)
        {
            hitboxes[4].SetActive(true);
            attackTimer = 0f;
            attacking = true;
        }
        else if (Input.GetAxis("Vertical") < 0 && !isGrounded)
        {
            hitboxes[5].SetActive(true);
            attackTimer = 0f;
            attacking = true;
        }

        else if (curCombo == 0 || !isGrounded)
        {
            hitboxes[0].SetActive(true);
            attackTimer = 0f;
            attacking = true;
            curCombo = 1;
        }

        else if (curCombo == 1 && attackTimer < comboTimer && isGrounded)
        {
            hitboxes[1].SetActive(true);
            attackTimer = 0f;
            attacking = true;
            curCombo = 2;
        }

        else if (curCombo == 2 && attackTimer < comboTimer && isGrounded)
        {
            hitboxes[2].SetActive(true);
            attackTimer = 0f;
            attacking = true;
            curCombo = 3;
        }
    }
    private void GroundSlam()
    {
        if (!isGrounded)
        {
            rb.velocity = new Vector3(0, -1 * slamSpeed, 0);
            isSlamming = true;
            hitboxes[3].SetActive(true);
        }
    }
}
