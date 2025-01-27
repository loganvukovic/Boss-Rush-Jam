using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScript : MonoBehaviour
{
    public PlayerMovement playerMovement; 
    public BossActions bossActions;
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
            if (playerMovement.curSide == bossActions.spawnSide)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerMovement.transform.position.x, transform.position.y, transform.position.z), speed * Time.deltaTime);
            }
            chasingTimer += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(stillTime);
        Explode();
    }

    public void Explode()
    {
        //Create explosion here
        Destroy(gameObject);
    }
}
