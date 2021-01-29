using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    // declaration and initialisation of variables
    public Transform myplayerTransform;
    // public bool isFlipped = false; // variable to control the flipping over of the boss towards the player


    // lookat the player
    public void LookAtPlayer()
    {
        // always look at the player and flip over the boss
        Vector3 target_position  = new Vector3(0, transform.position.y, myplayerTransform.position.z);
        transform.LookAt(target_position);
    }

    // Start is called before the first frame update
    void Start()
    {
        
        myplayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
