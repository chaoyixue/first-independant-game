using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene2Manage : MonoBehaviour
{
    // get the gameobject player to have an access to its public variables
    GameObject thePlayer;
    Player myplayer;
    Transform myplayer_transform;

    // variable to save the  name and the id of the scene activated now
    private string sceneName;
    private int sceneIndex;

    // get the transform of the door
    [SerializeField] Transform door_transform;

    // enter the scene pausemenu to pause the game
    void PauseGame()
    {
        // change the public variable which determines the status of pause
        myplayer.TfPaused = !myplayer.TfPaused;
        Time.timeScale = 0; // THE WORLD!
        // load the scene 3 that is the pause menu scene
        SceneManager.LoadScene(sceneBuildIndex: 4, LoadSceneMode.Additive);
    }

    // Get the name and the id of the scene activated now
    void GetScene()
    {
        sceneName = SceneManager.GetActiveScene().name;
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // resume game
    void ResumeGame()
    {
        Time.timeScale = 1; // time, commence to flow
    }

    // Start is called before the first frame update
    void Start()
    {
        GetScene();
        // get to the public variable
        thePlayer = GameObject.Find("DogPBR");
        myplayer = thePlayer.GetComponent<Player>();
        myplayer_transform = thePlayer.GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        // if we tap esc, will go to the pause mode
        if (Input.GetKeyDown(KeyCode.Escape) && !myplayer.TfPaused)
        {
            PauseGame();
        }

        // if we are close to the door and we pressed U, we will pass to the third scene
        if (System.Math.Abs(myplayer_transform.position.z - door_transform.position.z) < 1 && Input.GetKeyDown(KeyCode.U))
        {
            // load the next scene
            SceneManager.LoadScene(sceneIndex+1);
        }

    }
}
