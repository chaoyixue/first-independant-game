using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    // one class that passed to the Dialogue Manager whenever there is a new dialogue
    public string name;
    [TextArea(3,10)]
    public string[] sentences;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
