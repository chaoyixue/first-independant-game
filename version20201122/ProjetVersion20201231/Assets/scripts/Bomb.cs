using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float speed;
    private Rigidbody bombRB;
    public Vector3 direct;
    private Transform bombBody;

    // get the main camera
    private Camera mainCamera;
    private CameraShaker cameraShaker;
    

    private bool startFlash = false; // if or not start flash  
    public float flashLength; // the duration of the flash
    public float timeBetweenFlash; // the step between flash
    private float flashCounter1; // timer for the step 
    private float flashCounter2; // timer for the duration of the flash
    public int flashTime; // the number of flash

    private Renderer rend;
    private Color storedColor;

    public float damageRange = 2.0f; // the damage range of the bomb
    public Player player; // get the player

   
    public float damage; // the damage of the bomb

    // the animation of explosion
    public Exposion exposion;
    private bool exposionFinish = false;



    // Start is called before the first frame update
    void Start()
    {
        bombRB = this.GetComponent<Rigidbody>();
        GameObject gameObject = GameObject.FindWithTag("MainCamera");
        mainCamera = gameObject.GetComponent<Camera>();
        cameraShaker = mainCamera.GetComponent<CameraShaker>();
        //bombBody = GetComponentsInChildren<Transform>()[1];
        //bombBody = GameObject.FindGameObjectWithTag("GrenadeBody");
        //rend = bombBody.GetComponent<Renderer>();
        //storedColor = rend.material.GetColor("_Color");
        //playerObj = player.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        bombRB.AddForce(direct * speed * Time.deltaTime, ForceMode.VelocityChange); // give the bomb a horizontal force
        if (startFlash) // check if the flash starts
        {
            exposion.start = true;
            //Exposion newExposion = Instantiate(exposion, transform.position, Quaternion.identity) as Exposion;
            Flashing(); // start the flash 
            if (flashTime == 0) // check if the flash is end
            {
                exposion.finish = true;
                Destroy(this.gameObject);
                //StartCoroutine(cameraShaker.Shake(0.12f, 0.3f));
                getDamage(player, damage);
            }
        }
        else
        {
            Destroy(this.gameObject, 10);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        bombRB.constraints = RigidbodyConstraints.FreezePositionZ;
        startFlash = true;
    }


    private void Flashing()
    {
        flashCounter1 -= Time.deltaTime;
        if (flashCounter1 <= 0)
        {
            if (flashCounter2 > 0)
            {
                //Debug.Log("flashCounter2:  " + flashCounter2);
                flashCounter2 -= Time.deltaTime;
                //rend.material.SetColor("_Color", Color.red);
            }
            else
            {
                //Debug.Log("hhhhhhhhh");
                flashTime--;
                flashCounter1 = timeBetweenFlash;
                flashCounter2 = flashLength;
                //rend.material.SetColor("_Color", storedColor);
            }
        }
        //*** debug ***//
        //
        Color lineColor;
        if (Vector3.Distance(player.gameObject.transform.position, this.transform.position) <= damageRange)
        {
            lineColor = Color.red;
        }
        else
        {
            lineColor = Color.green;
        }
        Debug.DrawLine(this.transform.position, player.gameObject.transform.position, lineColor);
        //
    }

    public void getDamage(Player player, float damage)
    {
        if (Vector3.Distance(player.gameObject.transform.position, this.transform.position) <= damageRange)
        {
            Debug.Log("Damage:" + damage);
            player.TakeDamage(damage);
        }
    }

}
