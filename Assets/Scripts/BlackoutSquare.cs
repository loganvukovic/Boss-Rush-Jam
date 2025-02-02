using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlackoutSquare : MonoBehaviour
{
    public bool blackFromStart;
    public Animator cutsceneManager;
    public GameObject knight;

    // Start is called before the first frame update
    void Start()
    {
        if (blackFromStart)
        {
            Color objectColor = GetComponent<UnityEngine.UI.Image>().color;
            GetComponent<UnityEngine.UI.Image>().color = new Color(objectColor.r, objectColor.g, objectColor.b, 1);
            StartCoroutine(FadeFromBlack());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator FadeBlackOutSquare(int fadeSpeed = 1)
    {
        Debug.Log("aaaa");
        gameObject.SetActive(true);
        Color objectColor = GetComponent<UnityEngine.UI.Image>().color;
        float fadeAmount;

        while (GetComponent<UnityEngine.UI.Image>().color.a < 1)
        {
            fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            GetComponent<UnityEngine.UI.Image>().color = objectColor;
            yield return null;
        }
        /*yield return new WaitForSeconds(1);
        StartCoroutine(FadeFromBlack());*/
    }

    public IEnumerator FadeFromBlack(int fadeSpeed = 1)
    {
        if (blackFromStart)
        {
            yield return new WaitForSeconds(1f);
        }
        Color objectColor = GetComponent<UnityEngine.UI.Image>().color;
        float fadeAmount;

        while (GetComponent<UnityEngine.UI.Image>().color.a > 0)
        {
            fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            GetComponent<UnityEngine.UI.Image>().color = objectColor;
            yield return null;
        }
        if (cutsceneManager) cutsceneManager.enabled = true;
        if (knight) knight.SetActive(true);
        gameObject.SetActive(false);
    }
}
