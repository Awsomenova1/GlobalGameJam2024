using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject PlayButton;
    [SerializeField] private GameObject InstructionsPanel, SettingsPanel, CreditsPanel;
    private GameObject currentSelection;

    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(PlayButton.gameObject);
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
        // TODO change to gameplay scene
        // maybe add a fancy transition or something idk :eyes:
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
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
