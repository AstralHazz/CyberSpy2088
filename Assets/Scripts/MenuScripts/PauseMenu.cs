using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseScreenUI;

    public static bool gameIsPaused;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
                Resume();
            else
                Pause();
        }
    }

    private void Pause()
    {
        pauseScreenUI.SetActive(true);
        gameIsPaused = true;

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
    }
    private void Resume()
    {
        pauseScreenUI.SetActive(false);
        gameIsPaused = false;

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("Gwit The Game!");
        Application.Quit();
    }
}
