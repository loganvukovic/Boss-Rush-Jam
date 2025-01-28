using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class BossActions : MonoBehaviour
{
    public Transform tf;
    public PlayerMovement playerMovement;
    public GameObject stage;
    public bool wontShoot;

    public bool canRotate;
    public bool rotating;
    public bool stageRotating;

    public float rotateTimer;
    public float rotateCooldown;
    public float timeElapsed;
    public float rotationDuration;

    private Quaternion startRotation;
    private Quaternion targetRotation;

    public bool canClone;
    public GameObject[] spawnPoints;
    //public float cloneTimer;
    //public float cloneCooldown;
    public int hitCounter;
    public int reqHits;
    public string spawnSide;
    public string[] sides;
    public GameObject[] fakePuppets;
    public GameObject empty;
    public bool isRotating;
    public bool curSpots;
    public int prevSpot;
    public bool moving;

    private int previousAttack = -1;
    public int phase1Range;
    public BulletSpawner[] spawners;
    public int sideDependentSpawners = 0;
    public BulletSpawner[] northSpawners;
    public BulletSpawner[] westSpawners;
    public BulletSpawner[] southSpawners;
    public BulletSpawner[] eastSpawners;
    public string playerSide;
    public float attackTimer;
    public float attackCooldown;

    public bool healing;
    public bool dying = false;

    public string curSide;
    public int curPhase;
    public bool fakeBoss;

    // Start is called before the first frame update
    void Start()
    {
        previousAttack = -1;
        rotateTimer = 0;
        curSide = "North";

        if(canClone)
        {
            ChooseSpot(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (fakeBoss)
        {
            return;
        }

        curPhase = GetComponentInChildren<BossScript>().curPhase;
        playerSide = playerMovement.curSide;

        if (sideDependentSpawners > 0)
        {
            for (int i = 0; i < sideDependentSpawners; i++)
            {
                if (playerSide == "North")
                {
                    spawners[i] = northSpawners[i];
                }
                if (playerSide == "South")
                {
                    spawners[i] = southSpawners[i];
                }
                if (playerSide == "West")
                {
                    spawners[i] = westSpawners[i];
                }
                if (playerSide == "East")
                {
                    spawners[i] = eastSpawners[i];
                }
            }
        }

        if (GetComponentInChildren<BossScript>().healing)
        {
            healing = true;
            return;
        }
        else healing = false;

        stageRotating = playerMovement.rotating;

        if (canRotate)
        {
            rotateTimer += Time.deltaTime;
            if (rotateTimer > rotateCooldown)
            {
                if (Random.Range(0f, 1f) < 0.5f)
                {
                    Rotate(90);
                }
                else
                {
                    Rotate(-90);
                }
            }
            if (rotating && timeElapsed < rotationDuration)
            {
                timeElapsed += Time.deltaTime;
                transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / rotationDuration);

                if (Quaternion.Angle(transform.localRotation, targetRotation) < 0.1f)
                {
                    timeElapsed = 0;
                    rotating = false;
                }
            }
        }

        //Round rotation to nearest multiple of 90
        /*if (!rotating && !stageRotating && tf.rotation.y % 90 != 0)
        {
            Debug.Log((tf.eulerAngles.y / 90f) * 90f);
            tf.rotation = Quaternion.Euler(0, Mathf.Round(tf.eulerAngles.y / 90f) * 90f, 0);
        }*/

        if (stageRotating)
        {
            return;
        }

        if(canClone)
        {
            if (hitCounter >= reqHits)
            {
                foreach (GameObject spawn in spawnPoints)
                {
                    spawn.GetComponent<PuppetSpawn>().SwitchSpots();
                }
                foreach (GameObject puppet in fakePuppets)
                {
                    foreach (GameObject spawn in puppet.GetComponent<CloneScript>().spawnPoints)
                    {
                        spawn.GetComponent<PuppetSpawn>().SwitchSpots();
                    }
                }
                ChooseSpot(Random.Range(0, spawnPoints.Length));
            }
        }

        attackTimer += Time.deltaTime;
        if (attackTimer > attackCooldown && !wontShoot)
        {
            if (curPhase == 1)
            {
                PickAttack(Random.Range(0, phase1Range));
            }
            else
            {
                PickAttack(Random.Range(0, spawners.Length));
            }
        }
    }

    private void Rotate(float angle)
    {
        if (curSide == "North")
        {
            if (angle == 90) curSide = "West";
            else curSide = "East";
        }
        else if (curSide == "West")
        {
            if (angle == 90) curSide = "South";
            else curSide = "North";
        }
        else if (curSide == "South")
        {
            if (angle == 90) curSide = "East";
            else curSide = "West";
        }
        else if (curSide == "East")
        {
            if (angle == 90) curSide = "North";
            else curSide = "South";
        }

        rotateTimer = 0f;
        startRotation = transform.localRotation;
        targetRotation = startRotation * Quaternion.Euler(0, angle, 0);
        rotating = true;
    }

    private void ChooseSpot(int spot)
    {
        if (spot != prevSpot)
        {
            prevSpot = spot;
            hitCounter = 0;
            StartCoroutine(UpdatePosition(spot));
            spawnSide = sides[spot];
            CreateClones(spot);
        }
        else ChooseSpot(Random.Range(0, spawnPoints.Length));
    }

    private void CreateClones(int spot)
    {
        GameObject spawnedClone;
        int curPuppet = 0;
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (i != spot)
            {
                spawnedClone = fakePuppets[curPuppet];
                curPuppet++;
                CloneScript cloneScript = spawnedClone.GetComponent<CloneScript>();
                cloneScript.side = sides[i];
                cloneScript.spawnPoint = cloneScript.spawnPoints[i];
                StartCoroutine(cloneScript.UpdatePosition());
            }
        }
        RandomizeSpawnOrder(fakePuppets);
    }

    private void RandomizeSpawnOrder(GameObject[] puppets)
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject tempSpawn = spawnPoints[i];
            string tempSide = sides[i];
            int r = Random.Range(i, spawnPoints.Length);
            spawnPoints[i] = spawnPoints[r];
            spawnPoints[r] = tempSpawn;
            sides[i] = sides[r];
            sides[r] = tempSide;

            foreach (GameObject puppet in puppets)
            {
                CloneScript cloneScript = puppet.GetComponent<CloneScript>();
                GameObject tempCloneSpawn = cloneScript.spawnPoints[i];
                cloneScript.spawnPoints[i] = cloneScript.spawnPoints[r];
                cloneScript.spawnPoints[r] = tempCloneSpawn;
            }
        }
    }

    private void PickAttack(int attack)
    {
        if(attack != previousAttack)
        {
            previousAttack = attack;
            attackTimer = 0;
            spawners[attack].Fire();
            Debug.Log(spawners[attack].gameObject);
            attackCooldown = spawners[attack].cooldown;

            BulletSpawner[] linkedSpawners = spawners[attack].linkedSpawners;
            if (linkedSpawners.Length != 0)
            {
                foreach (BulletSpawner spawner in linkedSpawners)
                {
                    spawner.Fire();
                }
            }
        }
    }
    public void IncreaseSpeed(float speed)
    {
        foreach (BulletSpawner spawner in spawners)
        {
            spawner.cooldown = spawner.cooldown / speed;
        }
    }

    public IEnumerator UpdatePosition(int spot)
    {
        moving = true;
        GameObject tempObject = Instantiate(empty, spawnPoints[spot].transform.position, transform.rotation, stage.transform);
        transform.localRotation = CalcNewAngle(spot);
        while (transform.position != tempObject.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, tempObject.transform.position, 0.1f);
            yield return new WaitForSeconds(0.01f);
        }
        moving = false;
        transform.position = tempObject.transform.position;
        Destroy(tempObject);
        //loat westRNG = Random.Range(0f, 1f);
        //float eastRNG = Random.Range(0f, 1f);      
    }
    public Quaternion CalcNewAngle(int spot)
    {
        string side = sides[spot];
        Quaternion angle;
        if (side == "North")
        {
            angle = Quaternion.Euler(0, 0, 0);
        }
        else if (side == "South")
        {
            angle = Quaternion.Euler(0, 180, 0);
        }
        else if (side == "West")
        {
            angle = Quaternion.Euler(0, 90, 0);
        }
        else
        {
            angle = Quaternion.Euler(0, 270, 0);
        }

        return angle;
    }
}
