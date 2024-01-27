using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool paused;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject WinScreen;
    [SerializeField] private GameObject LoseScreen;
    [SerializeField] private LaughMeter laughMeter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused && !PopupPanel.open)
                Pause();
            else if (paused && PopupPanel.open)
                UnPause();
        }

        // TODO TEMP DEBUG KEYBINDS
        if (Input.GetKeyDown(KeyCode.W) && !paused && !PopupPanel.open)
        {
            Win();
        }
        if (Input.GetKeyDown(KeyCode.L) && !paused && !PopupPanel.open)
        {
            Lose();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
        paused = true;
    }

    public void UnPause()
    {
        Time.timeScale = 1;
        paused = false;
        PauseMenu.GetComponent<PopupPanel>().Close();
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1;
        paused = false;
        PopupPanel.open = false; // TODO this is really crappy and temporary
        // TODO add fancy scene transition thingy
        SceneManager.LoadScene("Main Menu");
    }

    public void Win()
    {
        WinScreen.SetActive(true);
        WinScreen.GetComponentInChildren<TextMeshProUGUI>().SetText("<b><size=+200%><align=center>YOU WIN!</size></b>\nYou're a great listener!\nAverage Distance from Center: " + laughMeter.calculateAvgDistance());
    }

    public void Lose()
    {
        LoseScreen.SetActive(true);
    }

    public void PlayAgain()
    {
        PopupPanel.open = false; // TODO this is really crappy and temporary
        // TODO add fancy screen transition thingy
        SceneManager.LoadScene("Game Scene");
    }
}
