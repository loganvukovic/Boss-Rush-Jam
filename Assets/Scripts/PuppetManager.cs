using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuppetManager : MonoBehaviour
{
    public BossActions mainBoss;
    public BossActions swordPuppet;
    public AudioSource audioSource;
    public bool fightStarted;
    public BulletSpawner spiderBullets;

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
        spiderBullets.enabled = false;
        yield return new WaitForSeconds(3f);
        mainBoss.canClone = true;
        mainBoss.ChooseSpot(0);
        mainBoss.wontShoot = false;
        swordPuppet.wontShoot = false;
        spiderBullets.enabled = true;
        fightStarted = true;
        //audioSource.Play();
    }
}
