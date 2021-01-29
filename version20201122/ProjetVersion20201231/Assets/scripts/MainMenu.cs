using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    // function to resume the game
    private void ResumeGame()
    {
        // return from pause menu to the game screen
        SceneManager.UnloadSceneAsync(4);
        // change the value of the public variable TFPaused for the next pause 
        GameObject thePlayer = GameObject.Find("DogPBR");
        Player myplayer = thePlayer.GetComponent<Player>();
        myplayer.TfPaused = !myplayer.TfPaused;
        Time.timeScale = 1;
    }

    // variable to save the  name and the id of the scene activated now
    private string sceneName;
    private int sceneIndex;

    // Get the name and the id of the scene activated now
    void GetScene()
    {
        sceneName = SceneManager.GetActiveScene().name;
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // quit the game
    public void quitGame()
    {
        Application.Quit();
    }


    // press esc to resume the game
    private void EscResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResumeGame();
        }
    }

    private void Start()
    {
        GetScene();
    }

    private void Update()
    {
        EscResume();
        
    }
}