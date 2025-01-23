using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CloneScript : MonoBehaviour
{
    public GameObject spawnPoint;
    public string side;
    public BossActions bossActions;
    public GameObject empty;
    public Transform stage;
    public bool moving;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator UpdatePosition()
    {
        moving = true;
        GameObject tempObject = Instantiate(empty, spawnPoint.transform.position, transform.rotation, stage.transform);
        while (transform.position != tempObject.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, tempObject.transform.position, 0.1f);
            yield return new WaitForSeconds(0.01f);
        }
        transform.position = tempObject.transform.position;
        Destroy(tempObject);
        moving = false;
    }
}
