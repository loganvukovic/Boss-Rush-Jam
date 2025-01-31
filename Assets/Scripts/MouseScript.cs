using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScript : MonoBehaviour
{
    public PlayerMovement playerMovement; 
    public BossActions bossActions;
    public GameObject explosion;
    public float speed;
    public float chasingTime;
    public float stillTime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChaseAndExplode());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator ChaseAndExplode()
    {
        float chasingTimer = 0;
        while (chasingTimer < chasingTime)
        {
            if (playerMovement.curSide == bossActions.spawnSide && !playerMovement.rotating)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerMovement.transform.position.x, transform.position.y, transform.position.z), speed * Time.deltaTime);

                if (playerMovement.transform.position.x < transform.position.x)
                {
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, -90, 0);
                }
            }
            chasingTimer += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(stillTime);
        StartCoroutine(Explode());
    }

    public IEnumerator Explode()
    {
        GameObject spawnedExplosion = Instantiate(explosion, transform.position, transform.rotation, transform);
        yield return new WaitForSeconds(0.7f);
        Destroy(spawnedExplosion);
        Destroy(gameObject);
    }
}
