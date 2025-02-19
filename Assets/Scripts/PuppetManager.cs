using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuppetManager : MonoBehaviour
{
    public BossActions mainBoss;
    public BossActions swordPuppet;
    public AudioSource audioSource;
    public bool fightStarted;
    public BossActions spiderPuppet;
    public Animator bossAnimator;
    public GameObject shield;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartFight());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator StartFight()
    {
        mainBoss.canClone = false;
        fightStarted = false;
        mainBoss.wontShoot = true;
        swordPuppet.wontShoot = true;
        spiderPuppet.wontShoot = true;
        yield return new WaitForSeconds(3f);
        bossAnimator.SetTrigger("ActivateShield");
        yield return new WaitForSeconds(3f);
        shield.SetActive(true);
        //bossAnimator.SetTrigger("RevivePuppets");
        yield return new WaitForSeconds(2f);
        mainBoss.canClone = true;
        mainBoss.ChooseSpot(1);
        mainBoss.wontShoot = false;
        swordPuppet.wontShoot = false;
        spiderPuppet.wontShoot = false;
        fightStarted = true;
        audioSource.Play();
    }
}
