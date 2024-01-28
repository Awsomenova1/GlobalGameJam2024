using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEditor;

//holds functions of the main menu and sub menus
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject PlayButton, InstructionsPanel, SettingsPanel, CreditsPanel;
    [SerializeField] private ScreenWipe screenWipe;
    [SerializeField] private AK.Wwise.Event MenuBack, MenuSelect, MenuNav, MenuAdjust;
    private bool playing = false, quitting = false;
    [SerializeField] private Slider musicSlider, soundSlider;
    private GameObject currentSelection;
    public static bool firstopen = false;

    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(PlayButton.gameObject);
        firstopen = true;
        screenWipe.gameObject.SetActive(true);
        screenWipe.WipeOut();
    }

    public void UpdateMusicVolume()
    {
        SettingsManager.currentSettings.musicVolume = musicSlider.value;
        MenuAdjust.Post(gameObject);
        AkSoundEngine.SetRTPCValue("musicVolume", SettingsManager.currentSettings.musicVolume);
    }

    public void UpdateSoundVolume()
    {
        SettingsManager.currentSettings.soundVolume = soundSlider.value;
        MenuAdjust.Post(gameObject);
        AkSoundEngine.SetRTPCValue("soundVolume", SettingsManager.currentSettings.soundVolume);
    }

    // Update is called once per frame
    void Update()
    {
        if (!PopupPanel.open)
        {
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                currentSelection = EventSystem.current.currentSelectedGameObject;
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(currentSelection);
            }
        }
    }

    public void Play()
    {
        SettingsManager.SaveSettings();
        if (playing) return;
        playing = true;
        MenuSelect.Post(gameObject);
        screenWipe.WipeIn();
        screenWipe.PostWipe += LoadGame;
    }

    public void LoadGame()
    {
        screenWipe.PostWipe -= LoadGame;
        firstopen = false;
        SceneManager.LoadScene("Game Scene");
    }

    public void Instructions()
    {
        if (!PopupPanel.open && !playing && !quitting)
        {
            InstructionsPanel.SetActive(true);
            MenuSelect.Post(gameObject);
        }
    }

    public void Settings()
    {
        if (!PopupPanel.open && !playing && !quitting)
        {
            SettingsPanel.SetActive(true);
            musicSlider.value = SettingsManager.currentSettings.musicVolume;
            soundSlider.value = SettingsManager.currentSettings.soundVolume;
            MenuSelect.Post(gameObject);
        }
    }

    public void ToggleFullscreen()
    {
        SettingsManager.currentSettings.fullscreen = !SettingsManager.currentSettings.fullscreen;
        if (SettingsManager.currentSettings.fullscreen)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        MenuAdjust.Post(gameObject);
    }

    public void Credits()
    {
        if (!PopupPanel.open && !playing && !quitting)
        {
            CreditsPanel.SetActive(true);
            MenuSelect.Post(gameObject);
        }
    }

    public void Quit()
    {
        if (quitting) return;
        quitting = true;
        MenuSelect.Post(gameObject);
        screenWipe.WipeIn();
        screenWipe.PostWipe += ExitGame;
    }

    public void ExitGame()
    {
        screenWipe.PostWipe -= ExitGame;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void OnApplicationQuit()
    {
        SettingsManager.SaveSettings();
    }
}
