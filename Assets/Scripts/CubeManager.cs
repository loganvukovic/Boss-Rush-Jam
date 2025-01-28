using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    public BossActions bossActions;
    public GameObject[] spawners;
    public BulletSpawner spearSpawner;
    public Animator animator;
    public AudioSource audioSource;

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
        yield return new WaitForSeconds(3f);
        spearSpawner.Fire();
        foreach (var spawner in spearSpawner.linkedSpawners)
        {
            spawner.Fire();
        }
        yield return new WaitForSeconds(5f);
        animator.SetTrigger("Rise");
        yield return new WaitForSeconds(1.5f);
        audioSource.Play();
        yield return new WaitForSeconds(1f);
        bossActions.enabled = true;
        foreach (var spawner in spawners)
        {
            spawner.SetActive(true);
        }
    }
}
