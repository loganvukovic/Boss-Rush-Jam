using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CloneScript : MonoBehaviour
{
    public Vector3 spawnPoint;
    public string side;
    public BossActions bossActions;

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
        while (transform.position != spawnPoint)
        {
            transform.position = Vector3.MoveTowards(transform.position, spawnPoint, 0.1f);
            yield return new WaitForSeconds(0.01f);
        }
        transform.position = spawnPoint;
    }
}
