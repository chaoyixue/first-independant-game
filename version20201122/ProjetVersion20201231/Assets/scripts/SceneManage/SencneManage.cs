using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SencneManage : MonoBehaviour
{
    // get the gameobject player to have an access to its public variables
    GameObject thePlayer;
    Player myplayer;

    void PauseGame()
    {
        // change the public variable which determines the status of pause
        myplayer.TfPaused = !myplayer.TfPaused;
        Time.timeScale = 0; // THE WORLD!
        // load the scene 3 that is the pause menu scene
        SceneManager.LoadScene(sceneBuildIndex: 4, LoadSceneMode.Additive);
    }

    // resume game
    void ResumeGame()
    {
        Time.timeScale = 1; // time, commence to flow
    }

    // get the transform of our player
    private Transform my_player_transform;

    // variable to save the  name and the id of the scene activated now
    private string sceneName;
    private int sceneIndex;

    // Get the name and the id of the scene activated now
    void GetScene()
    {
        sceneName = SceneManager.GetActiveScene().name;
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }


    // Start is called before the first frame update
    void Start()
    {
        GetScene();
        
        // get to the public variable
        thePlayer = GameObject.Find("DogPBR");
        myplayer = thePlayer.GetComponent<Player>();
        // get the transform of our player
        my_player_transform = thePlayer.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // if the player is walking to the right corner of the scene
        if(my_player_transform.position.z >= 20)
        {
            // load the next thing
            SceneManager.LoadScene(sceneIndex+1);
        }

        // if we tap esc, will go to the pause mode
        else if (Input.GetKeyDown(KeyCode.Escape) && !myplayer.TfPaused)
        {
            PauseGame();
        }
    }
}
