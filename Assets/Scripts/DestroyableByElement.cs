using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableByElement : MonoBehaviour
{
    public string weakness;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "PlayerHB")
        {
            if(other.GetComponentInParent<PlayerAttack>().curElement == weakness)
            {
                Destroy(gameObject);
            }
        }
        if (other.tag == "Bubble")
        {
            if (other.GetComponent<Bullet>().element == weakness)
            {
                Destroy(gameObject);
            }
        }
    }
}
