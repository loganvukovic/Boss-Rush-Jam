using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassablePlatform : MonoBehaviour
{
    public Transform playerTransform;
    public bool isPassing;

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
        else if (!isPassing)
        {
            GetComponent<Collider>().enabled = true;
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.collider.GetComponentInParent<PlayerMovement>() != null)
        {
            other.collider.GetComponentInParent<PlayerMovement>().onPassable = true;

            if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
            {
                StartCoroutine(Pass());
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.collider.tag == "Player")
        {
            other.collider.GetComponentInParent<PlayerMovement>().onPassable = false;
        }
    }

    public IEnumerator Pass()
    {
        isPassing = true;
        GetComponent<Collider>().enabled = false;
        playerTransform.GetComponentInParent<PlayerMovement>().onPassable = false;
        Vector3 velocity = playerTransform.GetComponentInParent<Rigidbody>().velocity;
        playerTransform.GetComponentInParent<Rigidbody>().velocity = new Vector3(velocity.x, -6f, velocity.z);
        yield return new WaitForSeconds(0.8f);
        isPassing = false;
    }
}
