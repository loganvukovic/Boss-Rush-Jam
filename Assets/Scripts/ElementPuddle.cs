using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementPuddle : MonoBehaviour
{
    public string element;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Input.GetKey(KeyCode.K) && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
        {
            if (other.GetComponentInParent<PlayerMovement>().isGrounded())
            {
                other.GetComponentInParent<PlayerAttack>().Imbue(element);
                Destroy(gameObject);
            }
        }
    }
}
