using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // choose a player to follow
    public GameObject player;
    // variable for the depth of the camera
    public float cameraDepth = 8.0f;
    // the height wrt the player
    public float cameraHeight = 3.0f;


    // Update is called once per frame
    void Update()
    {
        Vector3 pos = player.transform.position;
        pos.x += cameraDepth;
        pos.y += cameraHeight;
        transform.position = pos;
    }
}
