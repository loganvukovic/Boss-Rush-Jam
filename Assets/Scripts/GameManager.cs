using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public BlackoutSquare blackOutSquare;
    public GameObject gameOverScreen;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            PlayerPrefs.SetInt("RotationDirection", 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RetryButton()
    {
        Time.timeScale = 1f;
        blackOutSquare.gameObject.SetActive(true);
        StartCoroutine(Retry());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartButton()
    {
        blackOutSquare.gameObject.SetActive(true);
        StartCoroutine(StartGame());
    }

    public IEnumerator Retry()
    {
        gameOverScreen.SetActive(false);
        StartCoroutine(blackOutSquare.FadeBlackOutSquare());
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public IEnumerator StartGame()
    {
        StartCoroutine(blackOutSquare.FadeBlackOutSquare());
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(0);
    }
}
