using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuppetSpawn : MonoBehaviour
{
    public Transform altSpawn;
    private Vector3 tempPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchSpots()
    {
        if (altSpawn != null)
        {
            tempPosition = transform.position;
            transform.position = altSpawn.position;
            altSpawn.position = tempPosition;
        }
    }
}
