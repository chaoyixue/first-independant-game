using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exposion : MonoBehaviour
{

    public float exposionSpeed; // speed of the explosion animation
    public bool finish = false;
    public bool start = false;


    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(0f,0f,0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            transform.localScale += new Vector3(exposionSpeed * Time.deltaTime, exposionSpeed * Time.deltaTime, exposionSpeed * Time.deltaTime);
        }
        if(transform.localScale.x > 1.5f && transform.localScale.y > 1.5f && transform.localScale.z > 1.5f)
        {
            exposionSpeed = 0f;
        }
        if(finish)
        {
            Destroy(this.gameObject);
        }
        
    }
}
