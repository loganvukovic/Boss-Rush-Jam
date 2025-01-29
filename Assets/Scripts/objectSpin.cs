using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectSpin : MonoBehaviour
{




    public float rotSpeed = 50f;
    public int rotDirection = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (rotDirection)
        {
            case 1:

                transform.Rotate(0, 0, rotSpeed * Time.deltaTime);


                break;

            case 2:

                transform.Rotate(0, rotSpeed * Time.deltaTime, 0);

                break;


            case 3:

                transform.Rotate(rotSpeed * Time.deltaTime, 0, 0);

                break;

        } 
        
    }
}
