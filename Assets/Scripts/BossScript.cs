using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    public float curHealth;
    public float maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        curHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
