using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    public GameObject[] hitboxes;
    public Animator animator;

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

    void Start()
    {
        curElement = "None";
        
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
        //elementBar.fillAmount = elementTimer / elementDuration;

        attackTimer += Time.deltaTime;
        shootTimer += Time.deltaTime;

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

        if (attackTimer > attackCooldown && shootTimer > attackCooldown)
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

        if(isSlamming)
        {
            rb.velocity = new Vector3(0, -1 * slamSpeed, 0);
        }

        if (rotating)
            return;

        if (Input.GetKeyDown(KeyCode.K) && !attacking && attackTimer > attackCooldown && !isDashing && !isSlamming)
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.J) && !attacking && shootTimer > shootCooldown && !isDashing && !isSlamming)
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetAxis("Vertical") < 0)
        {
            GroundSlam();
        }

        if (curElement != null && elementTimer > 0)
        {
            elementTimer -= Time.deltaTime;
        }
        else if (elementTimer < 0)
        {
            Imbue("None");
        }

    }

    private void Attack()
    {
        if (Input.GetAxis("Vertical") > 0)
        {
            hitboxes[4].SetActive(true);
            attackTimer = 0f;
            attacking = true;
            animator.SetInteger("AtkDirection", 1);
        }
        else if (Input.GetAxis("Vertical") < 0 && !isGrounded)
        {
            hitboxes[5].SetActive(true);
            attackTimer = 0f;
            attacking = true;
            animator.SetInteger("AtkDirection", 3);
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

        if(attacking == true)
        {
            animator.SetTrigger("Attack");
        }
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
            hitboxes[3].SetActive(true);
        }
    }

    public void Imbue(string element)
    {
        curElement = element;

        if (element != "None")
        {
            elementTimer = elementDuration;
        }

        if (element == "None")
        {
            foreach (GameObject hitbox in hitboxes)
            {
                hitbox.GetComponent<Renderer>().material = noElement;
            }
        }
        else if (element == "Water")
        {
            foreach (GameObject hitbox in hitboxes)
            {
                hitbox.GetComponent<Renderer>().material = waterMaterial;
            }
        }
        else if (element == "Fire")
        {
            foreach (GameObject hitbox in hitboxes)
            {
                hitbox.GetComponent<Renderer>().material = fireMaterial;
            }
        }
        else if (element == "Grass")
        {
            foreach (GameObject hitbox in hitboxes)
            {
                hitbox.GetComponent<Renderer>().material = grassMaterial;
            }
        }
        else if (element == "Electric")
        {
            foreach (GameObject hitbox in hitboxes)
            {
                hitbox.GetComponent<Renderer>().material = electricMaterial;
            }
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
