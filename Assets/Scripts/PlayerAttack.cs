using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    public Collider[] hitboxes;
    public Renderer axe;
    public Animator animator;

    public Rigidbody rb;

    private bool rotating;
    public float attackTimer;
    public float attackCooldown;
    public float comboTimer;
    public bool attacking;
    public bool buffering;
    public float bufferTimer;
    public float bufferTime;

    public int curCombo;
    public float combo1Length;
    public float combo2Length;
    public float combo3Length;

    public bool isGrounded;
    public bool isDashing;

    public bool isSlamming;
    public float slamSpeed;

    public float shootTimer;
    public float shootCooldown;
    public BulletSpawner bulletSpawner;

    public string curElement;
    public float elementTimer;
    public float elementDuration;

    public Material noElement;
    public Material waterMaterial;
    public Material fireMaterial;
    public Material grassMaterial;
    public Material electricMaterial;
    public Image elementBar;
    public bool touchingPuddle;
    public bool imbuing;

    void Start()
    {
        curElement = "None";
        
        foreach (Collider hitbox in hitboxes)
        {
            if (hitbox != null)
            {
                hitbox.enabled = false;
            }
        }

    }

    void Update()
    {
        if(isGrounded)
        {
            animator.SetBool("Slamming", false);
        }

        if (elementBar != null)
        {
            elementBar.fillAmount = elementTimer / elementDuration;
        }

        attackTimer += Time.deltaTime;
        shootTimer += Time.deltaTime;

        if(buffering)
        {
            bufferTimer += Time.deltaTime;
            if (bufferTimer > bufferTime)
            {
                buffering = false;
            }
        }

        if (attackTimer > combo1Length)
        {
            hitboxes[0].enabled = false;
            hitboxes[4].enabled = false;
            hitboxes[5].enabled = false;
        }
        if (attackTimer > combo2Length)
        {
            hitboxes[1].enabled = false;
        }
        if (attackTimer > combo3Length)
        {
            hitboxes[2].enabled = false;
        }

        if (attackTimer > attackCooldown && shootTimer > attackCooldown && !imbuing)
        {
            attacking = false;
        }
        else if (imbuing)
        {
            attacking = true;
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
            hitboxes[3].enabled = false;
        }

        if(isSlamming)
        {
            rb.velocity = new Vector3(0, -1 * slamSpeed, 0);
        }

        if (rotating)
            return;

        if ((Input.GetKeyDown(KeyCode.K) || buffering) && !attacking && attackTimer > attackCooldown && !isDashing && !isSlamming)
        {
            Attack();
        }
        else if (Input.GetKeyDown(KeyCode.K) && !isDashing && !isSlamming)
        {
            buffering = true;
            bufferTimer = 0f;
        }

        if (Input.GetKeyDown(KeyCode.J) && !attacking && shootTimer > shootCooldown && !isDashing && !isSlamming)
        {
            //Shoot();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetAxis("Vertical") < 0)
        {
            animator.SetBool("Slamming", true);
            GroundSlam();
        }

        if (curElement != null && elementTimer > 0)
        {
            elementTimer -= Time.deltaTime;
        }
        else if (elementTimer < 0)
        {
            Imbue("None", false);
        }

    }

    private void Attack()
    {
        if (Input.GetAxis("Vertical") < 0 && !isGrounded)
        {
            hitboxes[5].enabled = true;
            attackTimer = 0f;
            attacking = true;
            animator.SetTrigger("DAir");
            //animator.SetInteger("AtkDirection", 3);
        }
        else if ((Input.GetAxis("Vertical") < 0 && !touchingPuddle) || Input.GetAxis("Vertical") >= 0)
        {
            if (Input.GetAxis("Vertical") > 0)
            {
                hitboxes[4].enabled = true;
                attackTimer = 0f;
                attacking = true;
                animator.SetTrigger("UAir");
                //animator.SetInteger("AtkDirection", 1);
            }

            else if (curCombo == 0 && isGrounded)
            {
                hitboxes[0].enabled = true;
                attackTimer = 0f;
                attacking = true;
                curCombo = 1;
                animator.SetTrigger("Attack1");
            }
            else if (!isGrounded)
            {
                hitboxes[0].enabled = true;
                attackTimer = 0f;
                attacking = true;
                curCombo = 1;
                animator.SetTrigger("FAir");
            }

            else if (curCombo == 1 && attackTimer < comboTimer && isGrounded)
            {
                hitboxes[1].enabled = true;
                attackTimer = 0f;
                attacking = true;
                curCombo = 2;
                animator.SetTrigger("Attack2");
            }

            else if (curCombo == 2 && attackTimer < comboTimer && isGrounded)
            {
                hitboxes[2].enabled = true;
                attackTimer = 0f;
                attacking = true;
                curCombo = 3;
                animator.SetTrigger("Attack3");
            }
        }

        /*if(attacking == true)
        {
            animator.SetTrigger("Attack");
        }*/
    }

    private void Shoot()
    {
        bulletSpawner.Fire();
        shootTimer = 0f;
    }
    private void GroundSlam()
    {
        if (!isGrounded)
        {
            rb.velocity = new Vector3(0, -1 * slamSpeed, 0);
            isSlamming = true;
            hitboxes[3].enabled = true;
        }
    }

    public void Imbue(string element, bool playAnimation)
    {
        curElement = element;

        if (element != "None")
        {
            elementTimer = elementDuration;
        }

        if (element == "None")
        {
            axe.material = noElement;
        }
        else if (element == "Water")
        {
            if (playAnimation) animator.SetTrigger("Absorb");
            axe.material = waterMaterial;
        }
        else if (element == "Fire")
        {
            if (playAnimation) animator.SetTrigger("Absorb");
            axe.material = fireMaterial;
        }
        else if (element == "Grass")
        {
            if (playAnimation) animator.SetTrigger("Absorb");
            axe.material = grassMaterial;
        }
        else if (element == "Electric")
        {
            if (playAnimation) animator.SetTrigger("Absorb");
            axe.material = electricMaterial;
        }

        if (element == "Water")
        {
            elementBar.enabled = true;
            elementBar.color = new Color32(0, 0, 255, 255);
        }
        else if (element == "Fire")
        {
            elementBar.enabled = true;
            elementBar.color = new Color32(255, 0, 0, 255);
        }
        else if (element == "Grass")
        {
            elementBar.enabled = true;
            elementBar.color = new Color32(0, 255, 0, 255);
        }
        else if (element == "Electric")
        {
            elementBar.enabled = true;
            elementBar.color = new Color32(255, 255, 0, 255);
        }
        else
        {
            elementBar.enabled = false;
        }
    }
}
