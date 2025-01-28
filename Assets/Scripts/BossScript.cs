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
                StartCoroutine(Heal());
            }
            else
            {
                //transform.parent.gameObject.SetActive(false);

                if(deathLeadsToScene)
                {
                    StartCoroutine(LoadScene());
                }
            }
        }

        if (invincible)
        {
            bool attackable = true;
            foreach (GameObject puppet in bossActions.fakePuppets)
            {
                if (puppet.activeSelf)
                {
                    attackable = false; 
                    break;
                }
            }
            if (attackable)
            {
                invincible = false;
                healthBar.SetActive(true);
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
        //animator.SetTrigger("Die");
        bossActions.dying = true;
        yield return null; bossActions.enabled = false;
        yield return new WaitForSeconds(timeBeforeLoad);
        StartCoroutine(blackoutSquare.FadeBlackOutSquare());
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneToLoad);
    }
}
