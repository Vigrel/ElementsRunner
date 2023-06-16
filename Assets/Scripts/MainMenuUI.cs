using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public GameObject startButton;
    public GameObject settingsPanel;
    public GameObject tutorialPanel;
    public GameObject mainScreen;

    // Settings panel variables
    public GameObject musicSoundtrack;
    public Button musicToggle;
    public Sprite musicOffImg;
    public Sprite musicOnImg;

    public Button SFXToggle;
    public Sprite SFXOffImg;
    public Sprite SFXOnImg;

    // private bool _musicEnabled = true;
    private bool _sfxEnabled = true;
    private MusicController _musicController;
    private Image _musicToggleImage;
    private Image _SFXToggleImage;

    // Tutorial panel variables


    // Start is called before the first frame update
    void Start()
    {
        startButton.SetActive(true);
        musicSoundtrack = GameObject.Find("MusicSoundtrack");
        _musicController = musicSoundtrack.GetComponent<MusicController>();
        _musicToggleImage = musicToggle.GetComponent<Image>();
        _SFXToggleImage = SFXToggle.GetComponent<Image>();
        if (_musicController.musicEnabled)
        {
            _musicToggleImage.sprite = musicOnImg;
        }
        else
        {
            _musicToggleImage.sprite = musicOffImg;
        }
    }

    public void playGame()
    {
        SceneManager.LoadScene("play_game");
    }

    public void openSettingsPanel()
    {
        mainScreen.SetActive(false);
        settingsPanel.SetActive(true);
    }

    // Functions inside Settings Panel

    public void closeSettingsPanel()
    {
        settingsPanel.SetActive(false);
        mainScreen.SetActive(true);
    }

    public void toggleMusic()
    {
        if (_musicController.musicEnabled)
        {
            // _musicEnabled = false;
            _musicController.StopMusic();
            _musicToggleImage.sprite = musicOffImg;
        }
        else
        {
            // _musicEnabled = true;
            _musicController.PlayMusic();
            _musicToggleImage.sprite = musicOnImg;
        }
    }

    public void toggleSFX()
    {
        if (_sfxEnabled)
        {
            _sfxEnabled = false;
            _SFXToggleImage.sprite = SFXOffImg;
        }
        else
        {
            _sfxEnabled = true;
            _SFXToggleImage.sprite = SFXOnImg;
        }
    }

    // Tutorial panel functions

    public void openTutorialPanel()
    {
        mainScreen.SetActive(false);
        tutorialPanel.SetActive(true);
    }

    public void closeTutorialPanel()
    {
        tutorialPanel.SetActive(false);
        mainScreen.SetActive(true);
    }
}
