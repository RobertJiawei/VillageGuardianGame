using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManagement : MonoBehaviour
{
    [SerializeField] Slider VolumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))//check if there is a player preference for music volume
        {
            PlayerPrefs.SetFloat("musicVolume", 0.5f);//if no player preference, create one
            Load();
        }
        else
        {
            Load();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Save();
    }

    public void ChangeVolume()
    {
        AudioListener.volume = VolumeSlider.value;//change the audio volume when slider change
        Save();
    }

    private void Load()
    {
        VolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");//when game loaded the audio volume will be the player
        //player preference volume
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", VolumeSlider.value);
        //save the slider value to player preference when quit game
    }
}
