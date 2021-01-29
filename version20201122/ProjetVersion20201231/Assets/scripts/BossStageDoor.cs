using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStageDoor : MonoBehaviour
{
    public GameObject TextObject;
    // variable to control the duration of the text to be showed
    [SerializeField] float duration_Text = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // debug
        // Debug.Log("the player triggered");

        // if the player is nearby, show the message on the screen
        if (other.gameObject.tag == "Player")
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
