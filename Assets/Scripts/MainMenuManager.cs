using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button PlayButton;

    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(PlayButton.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        // TODO change to gameplay scene
        // maybe add a fancy transition or something idk :eyes:
        Debug.Log("Play");
    }

    public void Instructions()
    {
        // TODO open separate popup or something for game instructions
        Debug.Log("Instructions");
    }

    public void Settings()
    {
        // TODO open separate popup or something for game settings
        Debug.Log("Settings");
    }

    public void Credits()
    {
        // TODO open separate popup or something for game credits
        Debug.Log("Credits");
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
