using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            Scene pauseScene = SceneManager.GetSceneByName("Paused");

            if (!pauseScene.isLoaded)
                PauseGame();
            else
                ResumeGame();
        }
    }
    void PauseGame()
    {
        SceneManager.LoadScene("Paused", LoadSceneMode.Additive);

        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void ResumeGame()
    {
        Scene pauseScene = SceneManager.GetSceneByName("Paused");

        if (pauseScene.isLoaded)
        {
            SceneManager.UnloadSceneAsync("Paused");
        }

        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OpenSettings()
    {
        Time.timeScale = 1f;
        SceneTracker.previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Settings");
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}