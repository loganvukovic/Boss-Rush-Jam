using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!other.GetComponentInParent<PlayerMovement>().tookDamage)
            {
                other.GetComponentInParent<PlayerMovement>().curHealth -= damage;
                other.GetComponentInParent<PlayerMovement>().tookDamage = true;
            }
        }
    }
}
