using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStats : MonoBehaviour
{
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        damage = damage * PlayerPrefs.GetFloat("DamageMult");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Crate")
        {
            Destroy(other.transform.parent.gameObject);
        }
        if (other.tag == "Slammable" && GetComponentInParent<PlayerAttack>().isSlamming)
        {
            Destroy(other.transform.parent.gameObject);
        }
    }
}
