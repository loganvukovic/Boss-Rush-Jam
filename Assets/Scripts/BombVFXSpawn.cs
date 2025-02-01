using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombVFXSpawn : MonoBehaviour
{

    public GameObject VFX;


    private void OnDestroy()
    {

        Instantiate(VFX, this.gameObject.transform.position, gameObject.transform.rotation);

        
    }

}
