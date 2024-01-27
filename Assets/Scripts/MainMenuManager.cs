using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject PlayButton, InstructionsPanel, SettingsPanel, CreditsPanel;
    [SerializeField] private ScreenWipe screenWipe;
    private GameObject currentSelection;

    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(PlayButton.gameObject);
        screenWipe.gameObject.SetActive(true);
        Invoke("RevealScreen", 0.5f);
    }

    void RevealScreen()
    {
        screenWipe.WipeOut();
    }

    // Update is called once per frame
    void Update()
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

    public void Play()
    {
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
        if (!PopupPanel.open)
            InstructionsPanel.SetActive(true);
    }

    public void Settings()
    {
        if (!PopupPanel.open)
            SettingsPanel.SetActive(true);
    }

    public void Credits()
    {
        if (!PopupPanel.open)
            CreditsPanel.SetActive(true);
    }

    public void Quit()
    {
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
