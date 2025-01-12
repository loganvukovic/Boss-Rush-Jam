using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassablePlatform : MonoBehaviour
{
    public Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform.position.y < transform.position.y)
        {
            GetComponent<Collider>().enabled = false;
        }
        else
        {
            GetComponent<Collider>().enabled = true;
        }
    }

    private void OnColliderStay(Collider other)
    {
        Debug.Log("aaaaa");
        if (other.GetComponentInParent<PlayerMovement>() != null)
        {
            other.GetComponentInParent<PlayerMovement>().onPassable = true;

            if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
            {
                GetComponent<Collider>().enabled = false;
            }
        }
    }

    private void OnColliderExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerMovement>().onPassable = false;
        }
    }
}
