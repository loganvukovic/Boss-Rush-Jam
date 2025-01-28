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
        if (other.tag == "Player")
        {
            other.GetComponentInParent<PlayerAttack>().touchingPuddle = true;
        }
        if (other.tag == "Player" && Input.GetKey(KeyCode.K) && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
        {
            if (other.GetComponentInParent<PlayerMovement>().isGrounded())
            {
                StartCoroutine(Imbue(other));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponentInParent<PlayerAttack>().touchingPuddle = false;
        }
    }

    IEnumerator Imbue(Collider other)
    {
        other.GetComponentInParent<PlayerAttack>().Imbue(element, true);
        other.GetComponentInParent<PlayerAttack>().imbuing = true;
        yield return new WaitForSeconds(1f);
        other.GetComponentInParent<PlayerAttack>().imbuing = false;
        Destroy(gameObject);
    }
}
