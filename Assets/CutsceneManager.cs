using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public GameObject returnText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShowReturnText());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator ShowReturnText()
    {
        yield return new WaitForSeconds(25f);
        returnText.SetActive(true);
    }
}
