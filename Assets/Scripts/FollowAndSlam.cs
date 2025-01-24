using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FollowAndSlam : MonoBehaviour
{
    public CloneScript cloneScript;
    public PlayerMovement playerMovement;
    public bool isSlamming;
    public float followSpeed;
    public float slamDelay;
    public float fallSpeed;
    public float resumeDelay;
    public float raiseSpeed;
    public float contactDamage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.curSide == cloneScript.side && !playerMovement.rotating)
        {
            if(!isSlamming && !GetComponent<CloneScript>().moving)
            {
                FollowPlayer();
            }
            if (!isSlamming && Mathf.Abs(transform.position.x - playerMovement.transform.position.x) < 0.1f && !GetComponent<CloneScript>().moving)
            {
                StartCoroutine(SlamAfterDelay());
            }
        }
        else if (playerMovement.curSide != cloneScript.side)
        {
            StopAllCoroutines();
            StartCoroutine(ReturnToStart());
            isSlamming = false;
        }
    }

    public void FollowPlayer()
    {
        float newX = Mathf.MoveTowards(transform.position.x, playerMovement.transform.position.x, followSpeed * Time.deltaTime);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    public IEnumerator SlamAfterDelay()
    {
        isSlamming = true;
        Vector3 originalPosition = cloneScript.spawnPoint.transform.position;
        Vector3 targetPosition = new Vector3(playerMovement.transform.position.x, transform.position.y - 3, playerMovement.transform.position.z);
        yield return new WaitForSeconds(slamDelay);
        while (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, fallSpeed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(resumeDelay);
        while (transform.position != originalPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, raiseSpeed * Time.deltaTime);
            yield return null;
        }
        isSlamming = false;
    }

    public IEnumerator ReturnToStart()
    {
        while (transform.position != cloneScript.spawnPoint.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, cloneScript.spawnPoint.transform.position, raiseSpeed * Time.deltaTime);
            yield return null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!other.GetComponentInParent<PlayerMovement>().tookDamage && !other.GetComponentInParent<PlayerMovement>().rotating)
            {
                other.GetComponentInParent<PlayerMovement>().curHealth -= contactDamage;
            }
        }
    }
}
