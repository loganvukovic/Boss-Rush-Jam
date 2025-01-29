using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuppetSpawn : MonoBehaviour
{
    public Transform altSpawn;
    private Vector3 tempPosition;
    public string spiderDirection;
    public GameObject[] throwPoints;

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
            GameObject[] tempThrowPoints = throwPoints;
            throwPoints = altSpawn.GetComponent<PuppetSpawn>().throwPoints;
            altSpawn.GetComponent<PuppetSpawn>().throwPoints = tempThrowPoints;
            string tempDirection = spiderDirection;
            spiderDirection = altSpawn.GetComponent<PuppetSpawn>().spiderDirection;
            altSpawn.GetComponent <PuppetSpawn>().spiderDirection = tempDirection;
        }
    }
}
