using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject[] hitboxes;

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

        isGrounded = GetComponent<PlayerMovement>().isGrounded();
        rotating = GetComponent<PlayerMovement>().rotating;

        if (rotating)
            return;

        if (Input.GetKeyDown(KeyCode.K) && !attacking && attackTimer > attackCooldown)
        {
            if (curCombo == 0 || !isGrounded)
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


    }
}
