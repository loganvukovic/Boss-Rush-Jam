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

    public IEnumerator Retry()
    {
        gameOverScreen.SetActive(false);
        StartCoroutine(blackOutSquare.FadeBlackOutSquare());
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
