using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydraManager : MonoBehaviour
{
    public int hydraDeaths;

    // Start is called before the first frame update
    void Start()
    {
        hydraDeaths = 0;
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
        Debug.Log("Game Beaten!");
        yield return null;
    }
}
