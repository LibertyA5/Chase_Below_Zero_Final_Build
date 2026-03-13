using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsUI : MonoBehaviour
{
    public void Back()
    {
        if (!string.IsNullOrEmpty(SceneTracker.previousScene))
        {
            SceneManager.LoadScene(SceneTracker.previousScene);
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}

