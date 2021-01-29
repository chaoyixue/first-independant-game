//攻击型飞行器


using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UIElements;

public class aircraft02 : MonoBehaviour
{

    /* the attribute of the aircraft */
    public float moveSpeed; // the speed of the aircraft when monitoring
    public float tracingSpeed; // the speed of the aircraft when tracing the player
    public float rotateSpeed; // the rotation speed when monitoring
    public float tracingRotateSpeed; // the rotation speed when tracing the player
    public float distanceSeuil; // the distance to trigger the player

    /* the status of the aircraft*/
    private Vector3 startPosition; // the start position
    public float monitorRange; // the monitor range
   
    private Vector3 stopPosition; // the stop position
    private Vector3 startRotation; // the start rotation
    private Vector3 stopRotation; // the stop rotation


    private bool direction_stop = true; // the direction from the start position to the end position
    private bool start_turning = false; // check if the aircraft commences to turning around

    public float stayTime; // the duration when the aircraft is at the endposition

    private bool tracingMode = false; // check if the aircraft is in the tracing mode

    public Player player; // get the player
    private GameObject playerObj; 

    private bool tracing_turning = false; // check if in the mode tracing turning

    private bool waitToBack = false; //  check if need to return to the default state



    /***** fire control ******/
    public float fireDistanceSeuil; // the distance seuil for firing
    public Transform firePoint; // the position of the bomb being dropped
    private bool fireing; //  if or not throwing the bomb
    private float shotCounter; // timer to control the rate of shotting
    public float timeBetweenShots; // the time between two shots
    public Bomb bomb; // get the bomb
    public float bombSpeed; // the horizontal speed of the bomb
    private Vector3 bombDirect; // the direction of the bomb



    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        stopPosition = startPosition + new Vector3(0f, 0f, monitorRange);
        if (startPosition.z > stopPosition.z) //make sure startPosition smaller than stopPosition 
        {
            Vector3 temp = startPosition;
            startPosition = stopPosition;
            stopPosition = temp;
        }
        startRotation = transform.localEulerAngles;
        stopRotation = startRotation + new Vector3(0.0f, 180.0f, 0.0f); 
        playerObj = player.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        /**** move control ****/
        Switch(playerObj);
        if (!tracingMode)
        {
            if (waitToBack == true)
            {
                Vector3 pointTo = CalDirect(startPosition);
                if (!SameDirectOrNot(startPosition) && pointTo.z > 0)
                {
                    InTurning(startRotation, rotateSpeed);
                    if (transform.localEulerAngles == startRotation)
                    {
                        waitToBack = false;
                        start_turning = true;
                    }
                }
                if (!SameDirectOrNot(startPosition) && pointTo.z < 0)
                {
                    InTurning(stopRotation, rotateSpeed);
                    if (transform.localEulerAngles == stopRotation)
                    {
                        waitToBack = false;
                        start_turning = true;
                    }
                }
                if (SameDirectOrNot(startPosition))
                {
                    waitToBack = false;
                    start_turning = false;
                }
            }
            else
            {
                testMove();
            }
        }
        else
        {
            testTrace(playerObj);
        }

        /**** fire control ****/
        fireControl(playerObj);
    }


    private void Switch(GameObject playerObj)
    {
        if (InDistanceOrNot(playerObj))
        {
            tracingMode = true;
        }
        else
        {
            tracingMode = false;
        }
    }

    private void testMove()
    {
        if (direction_stop && !start_turning) // from the start position to the end position
        {
            InMoving(stopPosition, moveSpeed); // move
            if (transform.position == stopPosition) // if already at the end position
            {
                //yield return new WaitForSeconds(stayTime); 
                direction_stop = false; // set the orientation to the start position
                start_turning = true;
            }
        }
        if (!direction_stop && start_turning)
        {
            InTurning(stopRotation, rotateSpeed);
            if (transform.localEulerAngles == stopRotation)
            {
                start_turning = false;
            }
        }
        if (!direction_stop && !start_turning)
        {
            InMoving(startPosition, moveSpeed);
            if (transform.position == startPosition) // already moved to the start position
            {
                //yield return new WaitForSeconds(stayTime); 
                direction_stop = true; // set the orientation to the start position
                start_turning = true;
            }
        }
        if (direction_stop && start_turning)
        {
            InTurning(startRotation, rotateSpeed);
            if (transform.localEulerAngles == startRotation)
            {
                start_turning = false;
            }
        }
    }


    private void testTrace(GameObject playerObj)
    {
        waitToBack = true;
        Vector3 pointTo = CalDirect(playerObj.transform.position);
        if (!start_turning)
        {
            if (!SameDirectOrNot(playerObj.transform.position) && pointTo.z > 0 && tracing_turning == true)
            {
                InTurning(startRotation, tracingRotateSpeed);
                if (transform.localEulerAngles == startRotation)
                {
                    tracing_turning = false;
                }
            }
            if (!SameDirectOrNot(playerObj.transform.position) && pointTo.z < 0 && tracing_turning == true)
            {
                InTurning(stopRotation, tracingRotateSpeed);
                if (transform.localEulerAngles == stopRotation)
                {
                    tracing_turning = false;
                }
            }
            if (SameDirectOrNot(playerObj.transform.position) && tracing_turning == false)
            {
                transform.Translate(new Vector3(0.0f, 0.0f, 1.0f) * tracingSpeed * Time.deltaTime);
                if (System.Math.Abs(transform.position.z - playerObj.transform.position.z) <= 1.0f)
                {
                    tracing_turning = true;
                }
            }
            if (SameDirectOrNot(playerObj.transform.position) && tracing_turning == true && System.Math.Abs(transform.position.z - playerObj.transform.position.z) > 1.0f)
            {
                tracing_turning = false;
            }
            if (!SameDirectOrNot(playerObj.transform.position) && tracing_turning == false)
            {
                tracing_turning = true;
            }
        }
        else
        {
            testMove();
        }
    }


    /*tools*/
    /* check if the player is in a short distance */
    bool InDistanceOrNot(GameObject playerObj)
    {
        if (Vector3.Distance(this.transform.position, playerObj.transform.position) <= distanceSeuil)
        {
            return true;
        }
        return false;
    }

    /*计算目标朝向*/
    Vector3 CalDirect(Vector3 towards)
    {
        return Vector3.Normalize(new Vector3(transform.position.x, transform.position.y, towards.z) - this.transform.position);
    }

    /* check if the aircraft is on the same direction of our player*/
    bool SameDirectOrNot(Vector3 towards)
    {
        Vector3 pointTo = CalDirect(towards);
        if (pointTo == this.transform.forward)
        {
            return true;
        }
        return false;
    }

    void InMoving(Vector3 towards, float speed)
    {
        Vector3 tempPosition = transform.position;
        tempPosition = Vector3.MoveTowards(tempPosition, towards, speed * Time.deltaTime);
        transform.position = tempPosition;
    }

    void InTurning(Vector3 towards, float speed)
    {
        float direct;
        if (towards.y == 0f)
        {
            direct = -1.0f;
        }
        else
        {
            direct = 1.0f;
        }
        Vector3 tempAngle = transform.localEulerAngles;
        tempAngle = Vector3.RotateTowards(tempAngle, towards, speed * Time.deltaTime, direct * speed * Time.deltaTime);
        transform.localEulerAngles = tempAngle;
    }

    /******* fire part ******/

    void fireControl(GameObject playerObj)
    {
        if (Vector3.Distance(playerObj.transform.position, this.transform.position) <= fireDistanceSeuil)
        {
            fireing = true;
        }
        else
        {
            fireing = false;
        }

        if (fireing)
        {
            // update de shot counter
            shotCounter -= Time.deltaTime;
            // control the aircraft not to shot all the time
            if (shotCounter <= 0)
            {
                shotCounter = timeBetweenShots;
                // instantiate a bomb at the firePoint position
                Bomb newBomb = Instantiate(bomb, firePoint.position, firePoint.rotation) as Bomb;  
                newBomb.speed = bombSpeed;
                if (Vector3.Dot(this.transform.forward, Vector3.forward) != 0f)
                {
                    bombDirect = this.transform.forward;
                }
                else
                {
                    bombDirect = new Vector3(0f, 0f, 0f);
                }
                newBomb.direct = bombDirect;
                newBomb.player = player;
                //newBomb.healthBar = healthBar;
                //Transform children = newBomb.GetComponentInChildren<Transform>();
                //children.tag = "GrenadeBody";
                /*
                GameObject bombBody = GameObject.FindGameObjectWithTag("GrenadeBody");
                Renderer rend = bombBody.GetComponent<Renderer>();
                Color storedColor = rend.material.GetColor("_Color");
                this.GetComponent<BombFlashCol>().Flashing();
                */
            }
        }
    }




}




