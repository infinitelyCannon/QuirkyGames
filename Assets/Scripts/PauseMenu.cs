using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    
    public GameObject pauseMenuUI;
    public GameObject pauseScreen;
    public GameObject HowToPlay;
    public bool isPaused;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
        }

        if (isPaused)
        {
            ActivateMenu();
        }
        else
        {
            DeactivateMenu();
        }
    }


    void ActivateMenu()
    {
        Time.timeScale = 0.0f;
        AudioListener.pause = true;
        pauseMenuUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

   public void DeactivateMenu()
    {
        Time.timeScale = 1.0f;
        AudioListener.pause = false;
        pauseMenuUI.SetActive(false);
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Return();
    }
    public void HowTo()
    {
        HowToPlay.SetActive(true);
        pauseScreen.SetActive(false);
        
    }
    public void Return()
    {
        HowToPlay.SetActive(false);
        pauseScreen.SetActive(true);
        
    }
}
