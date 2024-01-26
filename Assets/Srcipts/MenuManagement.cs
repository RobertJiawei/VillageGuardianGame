using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuManagement : MonoBehaviour
{
    //Show cursor when menu shows up
    private void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);// Load game1 scene
    }

    public void QuitGame()
    {
        Application.Quit();// Quit Game
    }

    public void BackToMain()
    {
        SceneManager.LoadScene(0);//Load main menu scene
    }
}
