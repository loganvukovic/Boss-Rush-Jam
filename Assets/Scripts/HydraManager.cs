using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HydraManager : MonoBehaviour
{
    public int hydraDeaths;
    public AudioSource musicSource;
    public GameObject[] heads;
    public GameObject endingText;
    public GameObject[] healthBars;
    public PlayerMovement playerMovement;
    public float timeBeforeLoad;
    public int sceneToLoad;
    public BlackoutSquare blackoutSquare;

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
        //endingText.SetActive(true);
        yield return new WaitForSeconds(timeBeforeLoad);
        StartCoroutine(blackoutSquare.FadeBlackOutSquare());
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneToLoad);
        //yield return null;
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

    public void SwitchHealthBars()
    {
        if (playerMovement.curSide == "North")
        {
            foreach (GameObject bar in healthBars)
            {
                bar.SetActive(false);
            }
            healthBars[0].SetActive(true);
        }
        else if (playerMovement.curSide == "West")
        {
            foreach (GameObject bar in healthBars)
            {
                bar.SetActive(false);
            }
            healthBars[1].SetActive(true);
        }
        else if (playerMovement.curSide == "South")
        {
            foreach (GameObject bar in healthBars)
            {
                bar.SetActive(false);
            }
            healthBars[2].SetActive(true);
        }
        else if (playerMovement.curSide == "East")
        {
            foreach (GameObject bar in healthBars)
            {
                bar.SetActive(false);
            }
            healthBars[3].SetActive(true);
        }
    }
}
