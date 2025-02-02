using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableByElement : MonoBehaviour
{
    public string weakness;
    public AudioSource AudioSource;

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "PlayerHB")
        {
            if(other.GetComponentInParent<PlayerAttack>().curElement == weakness)
            {
                Destroy(gameObject);
            }
            else
            {
                AudioSource.Play();
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
