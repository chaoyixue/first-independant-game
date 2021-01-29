using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    // get to myDialogueBox
    public GameObject myDialogueBox;
    // name of the NPC
    public Text nameText;

    // the speech of the NPC
    public Text dialogueText;

    public Animator animatorDialogue;

    // keep track all the sentences in the current dialogue FIFO
    private Queue<string> sentences;

    public void StartDialogue(Dialogue dialogue)
    {

        animatorDialogue.SetBool("IsOpen", true); // playe the animation start dialogue
        //Debug.Log("starting conversation with " + dialogue.name);
        nameText.text = dialogue.name;
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            // end the dialogue
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        //Debug.Log(sentence);
        dialogueText.text = sentence;
    }

    // end the dialogue when all the sentences passed
    void EndDialogue()
    {
        //Debug.Log("the sentences are over.");
        animatorDialogue.SetBool("IsOpen", false);
        myDialogueBox.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
        // initialize
        sentences = new Queue<string>();
    }

    
}
