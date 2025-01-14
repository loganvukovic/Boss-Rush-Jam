using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    public float curHealth;
    public float maxHealth;
    public int curPhase;
    public bool healAfterFirstPhase;
    public bool healing;
    public float phase2Speed;

    public PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        curHealth = maxHealth;
        curPhase = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(curHealth <= 0)
        {
            if (healAfterFirstPhase && curPhase == 1)
            {
                StartCoroutine(Heal());
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerHB")
        {
            float damage;
            if (other.GetComponent<AttackStats>() != null)
                damage = other.GetComponent<AttackStats>().damage;
            else damage = other.GetComponent<Bullet>().damage;
            curHealth -= damage;
        }
    }

    IEnumerator Heal()
    {
        playerMovement.canMove = false;
        healing = true;
        GetComponent<Collider>().enabled = false;
        while (curHealth < maxHealth)
        {
            curHealth += 1;
            yield return new WaitForSeconds(0.01f);
        }
        healing = false;
        GetComponent<Collider>().enabled = true;
        curPhase++;
        GetComponentInParent<BossActions>().IncreaseSpeed(phase2Speed);
        playerMovement.canMove = true;
    }
}
