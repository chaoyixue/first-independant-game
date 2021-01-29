using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager3 : MonoBehaviour
{
    
  

    // enter the scene pausemenu to pause the game
    void PauseGame()
    {

        Time.timeScale = 0; // THE WORLD!
        // load the scene 3 that is the pause menu scene
        SceneManager.LoadScene(sceneBuildIndex: 4, LoadSceneMode.Additive);
    }

    // resume game
    void ResumeGame()
    {
        Time.timeScale = 1; // time, commence to flow
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if we tap esc, will go to the pause mode
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }
}
