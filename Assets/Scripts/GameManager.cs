using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public BlackoutSquare blackOutSquare;
    public GameObject gameOverScreen;
    public GameObject pauseScreen;
    public GameObject[] titleUI;
    public GameObject[] difficultyButtons;
    public GameObject controlsMenu;
    private bool isPaused;
    private bool controls;
    private bool difficulty;

    // Start is called before the first frame update
    void Start()
    {
        /*if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            PlayerPrefs.SetInt("RotationDirection", 1);
        }*/

        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
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
        if (!difficulty)
        {
            foreach (GameObject obj in titleUI)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in difficultyButtons)
            {
                obj.SetActive(true);
            }
            difficulty = true;
        }
        else
        {
            foreach (GameObject obj in titleUI)
            {
                obj.SetActive(true);
            }
            foreach (GameObject obj in difficultyButtons)
            {
                obj.SetActive(false);
            }
            difficulty = false;
        }
    }

    public void DifficultyButton(float diff)
    {
        blackOutSquare.gameObject.SetActive(true);
        StartCoroutine(StartGame(diff));
    }
    public void ReturnButton()
    {
        Time.timeScale = 1f;
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
        blackOutSquare.gameObject.SetActive(true);
        StartCoroutine(ReturnToTitle());
    }

    public IEnumerator Retry()
    {
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
        StartCoroutine(blackOutSquare.FadeBlackOutSquare());
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public IEnumerator StartGame(float diff)
    {
        foreach (GameObject obj in titleUI)
        {
            obj.SetActive(false);
        }
        PlayerPrefs.SetFloat("DamageMult", diff);
        StartCoroutine(blackOutSquare.FadeBlackOutSquare());
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }
    public IEnumerator ReturnToTitle()
    {
        StartCoroutine(blackOutSquare.FadeBlackOutSquare());
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(0);
    }

    public void Pause()
    {
        if (!isPaused)
        {
            pauseScreen.SetActive(true);
            isPaused = true;
            Time.timeScale = 0f;
        }
        else
        {
            pauseScreen.SetActive(false);
            isPaused = false;
            Time.timeScale = 1f;
        }
    }

    public void Controls()
    {
        if (!controls)
        {
            controls = true;
            controlsMenu.SetActive(true);
            foreach (GameObject obj in titleUI)
            {
                obj.SetActive(false);
            }
        }
        else
        {
            controls = false;
            controlsMenu.SetActive(false);
            foreach (GameObject obj in titleUI)
            {
                obj.SetActive(true);
            }
        }
    }
}
