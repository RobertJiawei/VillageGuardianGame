using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// This is a in game pause menu
/// </summary>
public class PauseMenu : MonoBehaviour
{
    //UI variables
    public static bool GameIsPause = false;
    public GameObject PausePanel;
    public GameObject Ui;
    public GameObject GameCursor;
    public AudioSource music;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    //When game is paused, enable pause menu and disable cursor, minimap and other UI
    public void Pause()
    {
        Ui.SetActive(false);
        PausePanel.SetActive(true);
        GameCursor.SetActive(false);
        Time.timeScale = 0;
        GameIsPause = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        music.Stop();
    }

    //When game reusmed, enable cursor, minimap and other UI, disable pause menu
    public void Resume()
    {
        Ui.SetActive(true);
        PausePanel.SetActive(false);
        GameCursor.SetActive(true);
        Time.timeScale = 1;
        GameIsPause = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        music.Play();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
