using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LockedDoor : MonoBehaviour
{
    public bool sameScene;
    public int sceneToLoad;
    public BlackoutSquare blackoutSquare;
    public GameObject[] objectsToDisable;
    public GameObject[] objectsToEnable;
    public GameObject stage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!other.GetComponentInParent<PlayerMovement>().rotating && other.GetComponentInParent<PlayerMovement>().heldKeys >= 1)
            {
                if (sameScene)
                {
                    StartCoroutine(LoadRoom(other.transform.parent.gameObject));
                }
                else
                {
                    StartCoroutine(LoadScene());
                }
            }
        }
    }

    public IEnumerator LoadRoom(GameObject player)
    {
        StartCoroutine(blackoutSquare.FadeBlackOutSquare());
        player.GetComponent<PlayerMovement>().heldKeys--;
        yield return new WaitForSeconds(2f);
        StartCoroutine(blackoutSquare.FadeFromBlack());
        stage.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        player.transform.position = new Vector3(5.67f, 6.04f, 7);
        foreach (var obj in objectsToDisable)
        {
            if (obj != null)
            obj.SetActive(false);
        }
        foreach (var obj in objectsToEnable)
        {
            obj.SetActive(true);
        }
        yield return new WaitForSeconds(1.7f);
        transform.parent.gameObject.SetActive(false);
    }

    public IEnumerator LoadScene()
    {
        StartCoroutine(blackoutSquare.FadeBlackOutSquare());
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneToLoad);
    }
}