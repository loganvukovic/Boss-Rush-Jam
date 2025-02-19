using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
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
    public Collider hitbox;
    public PuppetManager puppetManager;
    public Animator fatAnimator;

    // Start is called before the first frame update
    void Start()
    {
        hitbox.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!puppetManager.fightStarted)
        {
            return;
        }

        if (playerMovement.curSide == cloneScript.side && !playerMovement.rotating && !GetComponent<CloneScript>().moving && !GetComponentInChildren<BossScript>().dying)
        {
            fatAnimator.SetBool("Stop", false);
            if (!isSlamming && !GetComponent<CloneScript>().moving)
            {
                fatAnimator.SetBool("Following", true);
                FollowPlayer();
            }
            if (!isSlamming && Mathf.Abs(transform.position.x - playerMovement.transform.position.x) < 0.1f && !GetComponent<CloneScript>().moving)
            {
                StartCoroutine(SlamAfterDelay());
            }
        }
        else if (!GetComponentInChildren<BossScript>().dying)
        {
            fatAnimator.SetBool("Following", false);
            fatAnimator.SetBool("Stop", true);
            StopAllCoroutines();
            StartCoroutine(ReturnToStart());
            isSlamming = false;
        }
        else
        {
            fatAnimator.SetBool("Following", false);
            StopAllCoroutines();
        }
    }

    public void FollowPlayer()
    {
        float newX = Mathf.MoveTowards(transform.position.x, playerMovement.transform.position.x, followSpeed * Time.deltaTime);
        Debug.Log(newX);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    public IEnumerator SlamAfterDelay()
    {
        fatAnimator.SetBool("Following", false);
        isSlamming = true;
        Vector3 originalPosition = transform.position;
        Vector3 targetPosition = new Vector3(playerMovement.transform.position.x, transform.position.y - 6f, playerMovement.transform.position.z);
        yield return new WaitForSeconds(slamDelay);
        fatAnimator.SetTrigger("Slam");
        yield return new WaitForSeconds(.5f);
        hitbox.enabled = true;
        while (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, fallSpeed * Time.deltaTime);
            yield return null;
        }
        hitbox.enabled = false;
        yield return new WaitForSeconds(resumeDelay);
        fatAnimator.SetTrigger("Rise");
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
                other.GetComponentInParent<PlayerMovement>().tookDamage = true;
            }
        }
    }
}
