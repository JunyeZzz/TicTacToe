using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    public Slider sfxVolumeSlider;

    void Start()
    {
        // Load saved volume or set default to 1
        float savedVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        sfxVolumeSlider.value = savedVolume;
        AudioListener.volume = savedVolume;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game!");
    }

    public void SetSFXVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}