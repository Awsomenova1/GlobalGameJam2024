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
    private GameObject currentSelection;

    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(PlayButton.gameObject);
        screenWipe.gameObject.SetActive(true);
        screenWipe.WipeOut();
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
        if (playing) return;
        playing = true;
        // MenuSelect.Post(gameObject);
        screenWipe.WipeIn();
        screenWipe.PostWipe += LoadGame;
    }

    public void LoadGame()
    {
        screenWipe.PostWipe -= LoadGame;
        SceneManager.LoadScene("Game Scene");
    }

    public void Instructions()
    {
        if (!PopupPanel.open && !playing && !quitting)
        {
            InstructionsPanel.SetActive(true);
            // MenuSelect.Post(gameObject);
        }
    }

    public void Settings()
    {
        if (!PopupPanel.open && !playing && !quitting)
        {
            SettingsPanel.SetActive(true);
            // MenuSelect.Post(gameObject);
        }
    }

    public void Credits()
    {
        if (!PopupPanel.open && !playing && !quitting)
        {
            CreditsPanel.SetActive(true);
            // MenuSelect.Post(gameObject);
        }
    }

    public void Quit()
    {
        if (quitting) return;
        quitting = true;
        // MenuSelect.Post(gameObject);
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
}
