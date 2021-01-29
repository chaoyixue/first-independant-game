using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    /*attributes of the aircraft*/
    public float moveSpeed; // speed when monitoring
    public float tracingSpeed; // speed when tracing
    public float rotateSpeed; // turning speed when monitoring
    public float tracingRotateSpeed; // turning speed when tracing
    public float distanceSeuil; // distance seuil to commence the tracing

    /*status of the aircraft*/
    private Vector3 startPosition; // the start position of the aircraft
    public Vector3 stopPosition; // the stop position of the aircraft
    private Vector3 startRotation; // the start rotation of the aircraft
    public Vector3 stopRotation; // the stop rotation of the aircraft

    /* mode of the aircraft*/
    private bool monitoring; // monitor mode
    private bool tracing; // tracing mode
    private bool returning; // returning mode
    private bool turning; // rotation mode
    private bool moving; // moving mode

    /*player*/
    public Player player; 
    private GameObject playerObj; 



    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        if(System.Math.Abs(startPosition.z) > System.Math.Abs(stopPosition.z)) //make sure the start position is larger than the stop position
        {
            Vector3 temp = startPosition;
            startPosition = stopPosition;
            stopPosition = temp;
        }
        startRotation = transform.localEulerAngles; // start rotation（0，0，0）
        stopRotation = startRotation + new Vector3(0.0f, 180.0f, 0.0f); // stop inverse rotation（0，180，0）
        GameObject playerObj = player.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*functions*/
    private void AircraftMonitor() {
        /*
        if ()
        {

        }
        */
    }


    /*tools*/
    /*check if in the tracing mode*/
    bool InDistanceOrNot(GameObject playerObj) { 
        if(Vector3.Distance(this.transform.position, playerObj.transform.position) <= distanceSeuil)
        {
            return true;
        }
        return false;
    }

    /*check if on the same direction*/
    bool SameDirectOrNot(Vector3 towards) {
        Vector3 pointTo = CalDirect(towards);
        if(pointTo == this.transform.forward)
        {
            return true;
        }
        return false;
    }

    /*calculate the orientation of the target*/
    Vector3 CalDirect(Vector3 towards) { 
        return Vector3.Normalize(new Vector3(0f,0f,towards.z) - this.transform.position);
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
        if(towards.y == 0f)
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
}
