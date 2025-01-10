using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletLife = 1f;
    public float rotation = 0f;
    public float speed = 1f;
    public float damage = 5f;

    public float timer = 0f;
    private Vector3 spawnPoint;
    public bool isBomb;
    public bool destroyOnHit;
    public int spot;
    public GameObject bombBox;
    public Transform spawner;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        if (isBomb)
        {
            for (int i = -2; i < 3; i++)
            {
                if (spot != i)
                {
                    Instantiate(bombBox, new Vector3(spawner.position.x, spawner.position.y + (i * 2), spawner.position.z), transform.rotation, transform);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > bulletLife)
            Destroy(this.gameObject);
        timer += Time.deltaTime;
        transform.position = Movement(timer);
    }

    private Vector3 Movement(float timer)
    {
        float x, y, z;

        x = timer * speed * transform.right.x;
        y = timer * speed * transform.right.y;
        z = timer * speed * transform.right.z;

        return new Vector3(x + spawnPoint.x, y + spawnPoint.y, z + spawnPoint.z);
    }
}
