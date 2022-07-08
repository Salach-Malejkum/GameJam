using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class PauseMenu : MonoBehaviour
{

    public GameObject pasuePanel;

    public void ButtonPause()
    {
        pasuePanel.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void ButtonResume()
    {
        pasuePanel.SetActive(false);
        Time.timeScale = 1.0f;

    }

    public void ButtonRestart()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ButtonOptions()
    {

    }

    public void ButtonMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ButtonExitToDesktop()
    {
        if (EditorApplication.isPlaying)
            UnityEditor.EditorApplication.isPlaying = false;
        else
            Application.Quit();
    }

}
