using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossScript : MonoBehaviour
{
    public float curHealth;
    public float maxHealth;
    public int curPhase;
    public bool healAfterFirstPhase;
    public bool healing;
    public float phase2Speed;
    public int damageFromBubble;
    public bool isPuppet;
    public bool isHydra;
    public bool deathLeadsToScene;
    public int sceneToLoad;
    public float timeBeforeLoad;
    public BlackoutSquare blackoutSquare;

    public bool elemental;
    public string weakness;
    public bool invincible;

    public PlayerMovement playerMovement;
    public BossActions bossActions;
    public GameObject healthBar;
    public Renderer lightning;
    public Renderer puppetRenderer;
    public Collider puppetHitbox;
    public string elementToGive;
    public HydraManager hydraManager;
    public Animator bossAnimator;
    public GameObject empty;
    public GameObject stage;
    public bool dying;
    public GameObject shield;

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
                bossAnimator.SetTrigger("Enrage");
                StartCoroutine(Heal());
            }
            else
            {
                //transform.parent.gameObject.SetActive(false);

                if(deathLeadsToScene && !GetComponentInParent<BossActions>().dying)
                {
                    StartCoroutine(LoadScene());
                }
                else if (isPuppet && !GetComponentInParent<BossActions>().dying)
                {
                    StartCoroutine(PuppetDeath());
                }
                else if (isHydra && !GetComponentInParent<BossActions>().dying)
                {
                    StartCoroutine(HydraDeath());
                }
            }
        }

        if (invincible)
        {
            bool attackable = true;
            foreach (GameObject puppet in bossActions.fakePuppets)
            {
                if (!puppet.GetComponent<BossActions>().dying)
                {
                    attackable = false; 
                    break;
                }
            }
            if (attackable)
            {
                invincible = false;
                healthBar.SetActive(true);
                bossAnimator.SetTrigger("ShieldBreak");
                shield.SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerHB" && (!elemental || (elemental && weakness == other.GetComponentInParent<PlayerAttack>().curElement)) && !invincible)
        {
            float damage;
            if (other.GetComponent<AttackStats>() != null)
                damage = other.GetComponent<AttackStats>().damage;
            else damage = other.GetComponent<Bullet>().damage;
            curHealth -= damage;
            if (bossActions != null)
            {
                bossActions.hitCounter++;
            }
        }
        else if (other.tag == "PlayerHB" && GetComponentInParent<BossActions>().dying)
        {
            other.GetComponentInParent<PlayerAttack>().Imbue(elementToGive, false);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Bubble" && playerMovement.inBubble && !playerMovement.rotating)
        {
            if (other.GetComponent<Bullet>().element == weakness)
            {
                curHealth -= damageFromBubble;
                Destroy(other.gameObject);

                if (other.GetComponent<Bullet>().element == "Electric")
                {
                    StartCoroutine(StaticLightning());
                }
            }
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

    IEnumerator StaticLightning()
    {
        lightning.enabled = true;
        yield return new WaitForSeconds(0.3f);
        lightning.enabled = false;
    }

    IEnumerator LoadScene()
    {
        bossAnimator.SetTrigger("Die");
        bossActions.dying = true;
        yield return null; bossActions.enabled = false;
        yield return new WaitForSeconds(timeBeforeLoad);
        StartCoroutine(blackoutSquare.FadeBlackOutSquare());
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneToLoad);
    }

    IEnumerator PuppetDeath()
    {
        bossAnimator.SetTrigger("Die");
        //puppetRenderer.enabled = false;
        GetComponentInParent<BossActions>().dying = true;
        dying = true;
        GetComponent<Collider>().enabled = false;
        yield return null;
        GetComponentInParent<BossActions>().enabled = false;
        if (GetComponentInParent<FollowAndSlam>() != null)
        {
            GetComponentInParent<FollowAndSlam>().enabled = false;
            float deathY = GetComponentInParent<CloneScript>().spawnPoint.transform.position.y - 4.5f;
            GameObject deathPoint = Instantiate(empty, new Vector3(transform.position.x, deathY, transform.position.z), transform.rotation, stage.transform);
            while (transform.parent.transform.position != deathPoint.transform.position)
            {
                transform.parent.transform.position = Vector3.MoveTowards(transform.parent.transform.position, deathPoint.transform.position, 5f * Time.deltaTime);
                yield return null;
            }
            Destroy(deathPoint.gameObject);
        }
        if (GetComponentInParent<SpiderScript>() != null)
        {
            GetComponentInParent<SpiderScript>().enabled = false;
            transform.parent.transform.localRotation = Quaternion.Euler(0, 0, 0);
            float deathY;
            if (GetComponentInParent<CloneScript>().side != "South" || GetComponentInParent<CloneScript>().side != "North") deathY = GetComponentInParent<CloneScript>().spawnPoint.transform.position.y - 3.8f;
            else deathY = GetComponentInParent<CloneScript>().spawnPoint.transform.position.y;
                GameObject deathPoint = Instantiate(empty, new Vector3(transform.position.x, 6.4f, transform.position.z), transform.rotation, stage.transform);
            while (transform.parent.transform.position != deathPoint.transform.position)
            {
                transform.parent.transform.position = Vector3.MoveTowards(transform.parent.transform.position, deathPoint.transform.position, 5f * Time.deltaTime);
                yield return null;
            }
            Destroy(deathPoint.gameObject);
            bossAnimator.SetTrigger("Bounce");
        }
    }

    IEnumerator HydraDeath()
    {
        bossAnimator.SetTrigger("Die");
        GetComponentInParent<BossActions>().dying = true;
        hydraManager.hydraDeaths++;
        yield return null;
        GetComponentInParent<BossActions>().enabled = false;
    }
}
