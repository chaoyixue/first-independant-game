using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    // get informations of the player
    public GameObject myPlayer;

    // get the transform of the player
    private Transform transformPlayer;

    // check the distance between the player and the NPC
    private bool CheckDistance(Vector3 PositionPlyaer)
    {
        if(Vector3.Distance(PositionPlyaer, this.GetComponent<Transform>().position) <= 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }


    // Start is called before the first frame update
    void Start()
    {
        // get the transform of the player
        transformPlayer = myPlayer.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = transformPlayer.position;
        bool NearThePlayer = CheckDistance(playerPosition);
        if (NearThePlayer) // the player is nearby the NPC
        {
            if(Input.GetKeyDown(KeyCode.U)) // tape key U to communicate with the NPC
            {
                TriggerDialogue();
            }
            if(Input.GetKeyDown(KeyCode.Y))
            {
                FindObjectOfType<DialogueManager>().DisplayNextSentence();
            }
                   
        }

          
    }
}
