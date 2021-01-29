using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTriggered : MonoBehaviour
{
    public GameObject TextObject;
    // variable to control the duration of the text to be showed
    [SerializeField] float duration_Text = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        TextObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // debug
        // Debug.Log("the player triggered");

        // if the player is nearby, show the message on the screen
        if(other.gameObject.tag == "Player")
        {
            TextObject.SetActive(true);
            StartCoroutine(waitForSec());
        }
    }

    IEnumerator waitForSec()
    {
        yield return new WaitForSeconds(duration_Text);
        Debug.Log("we will destroy the object");
        TextObject.SetActive(false);
    }
}
