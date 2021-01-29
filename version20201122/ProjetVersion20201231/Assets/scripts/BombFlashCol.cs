using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombFlashCol : MonoBehaviour
{

    private bool startFlash = false; //if or not start to flash
    public float flashLength; // duration of each flash
    public float timeBetweenFlash; // gap between two flashes
    private float flashCounter1; // counter for the gap
    private float flashCounter2; // counter for the duration
    public int flashTime; // times of the flash

    //private GameObject bombBody;
    //private Renderer rend;
    //private Color storedColor;


    // Start is called before the first frame update
    void Start()
    {
        //bombBody = GameObject.FindGameObjectWithTag("GrenadeBody");
        //rend = bombBody.GetComponent<Renderer>();
        //storedColor = rend.material.GetColor("_Color");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Flashing(Renderer rend, Color storedColor)
    {
        flashCounter1 -= Time.deltaTime;
        if (flashCounter1 <= 0)
        {
            flashCounter2 = flashLength;
            if (flashCounter2 > 0)
            {
                Debug.Log("flashCounter2" + flashCounter2);
                flashCounter2 -= Time.deltaTime;
                rend.material.SetColor("_Color", Color.red);
            }
            else
            {
                Debug.Log("hhhhhhhhh");
                flashTime--;
                if (flashTime == 0)
                {
                    Destroy(this.gameObject);
                }
                flashCounter1 = timeBetweenFlash;
                rend.material.SetColor("_Color", storedColor);
            }
        }
    }
}
