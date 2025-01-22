using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockedDoor : MonoBehaviour
{
    public bool sameScene;
    public int sceneToLoad;
    public BlackoutSquare blackoutSquare;

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
        if (other.tag == "Player" && sameScene)
        {
            if (!other.GetComponentInParent<PlayerMovement>().rotating && other.GetComponentInParent<PlayerMovement>().heldKeys >= 1)
            {
                StartCoroutine(LoadRoom(other.transform.parent.gameObject));
            }
        }
    }

    public IEnumerator LoadRoom(GameObject player)
    {
        StartCoroutine(blackoutSquare.FadeBlackOutSquare());
        player.GetComponent<PlayerMovement>().heldKeys--;
        yield return new WaitForSeconds(1f);
        player.transform.position = new Vector3(5.67f, 6.04f, 7);
    }

    void LoadScene(int sceneToLoad)
    {

    }
}