using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionVisible : MonoBehaviour
{
    // this class used to control the invisibility of a text message when pressed tab

    // bool to control the visualbility 
    private bool visible = true;
    public GameObject TextObject;

    // Start is called before the first frame update
    void Start()
    {
        TextObject.SetActive(visible);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            visible = !visible;
            // Debug.Log("visible : " + visible);
        }
        TextObject.SetActive(visible);
    }
}
