using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydraManager : MonoBehaviour
{
    public int hydraDeaths;
    public AudioSource musicSource;
    public GameObject[] heads;
    public GameObject endingText;

    // Start is called before the first frame update
    void Start()
    {
        hydraDeaths = 0;
        StartCoroutine(StartFight());
    }

    // Update is called once per frame
    void Update()
    {
        if (hydraDeaths >= 4)
        {
            StartCoroutine(LoadEnding());
        }
    }

    public IEnumerator LoadEnding()
    {
        endingText.SetActive(true);
        yield return null;
    }

    public IEnumerator StartFight()
    {
        foreach (var head in heads)
        {
            head.GetComponent<BossActions>().wontShoot = true;
        }
        yield return new WaitForSeconds(3f);
        musicSource.Play();
        foreach (var head in heads)
        {
            head.GetComponent<BossActions>().wontShoot = false;
        }
    }
}
