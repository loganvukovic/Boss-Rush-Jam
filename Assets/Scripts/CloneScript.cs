using System.Collections;
using System.Collections.Generic;
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

    public void UpdatePosition()
    {
        transform.position = spawnPoint;
    }
}
