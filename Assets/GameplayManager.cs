using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameplayManager : MonoBehaviour
{

    public Animator gameSequence;
    public LaughMeter meter;
    public DialogController dialog;
    public Slider speedMeter;

    public ScreenWipe screenWipe;

    public bool startedSequence = false;
    public bool suspendSequence = false;

    public Image speedIcon;
    public Image laughIcon;

    public Sprite emote1;
    public Sprite emote2;
    public Sprite emote3;
    public Sprite emote4;

    public static bool paused;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject WinScreen;
    [SerializeField] private GameObject LoseScreen;

    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        meter.laughSpeed = 0;
        IntroSequence();
    }

    // Update is called once per frame
    void Update()
    {
        if (suspendSequence)
            return;

        if (startedSequence && gameSequence.GetCurrentAnimatorStateInfo(0).IsName("NoCurrentSequence"))
            FinishedSequence();

        if (!startedSequence && !dialog.reading && Input.GetKeyDown(KeyCode.Q))
            StartSequence();

        speedMeter.value = meter.laughSpeed * 5;

        if(meter.checkLose())
        {
            dialog.setSource(new DialogSource("Laughter"));
            dialog.reading = true;
            suspendSequence = true;
            Lose();
            button.stopInputs = true;
        }

        UpdateSliderIcons();


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

    public void IntroSequence()
    {
        dialog.setSource(new DialogSource("This guys about to talk to you. [exit]"));
        dialog.reading = true;
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
        screenWipe.WipeIn();
        screenWipe.PostWipe += LoadMenu;
    }

    public void LoadMenu()
    {
        screenWipe.PostWipe -= LoadMenu;
        SceneManager.LoadScene("Main Menu");
    }

    public void ReloadGame()
    {
        screenWipe.PostWipe -= ReloadGame;
        SceneManager.LoadScene("Game Scene");
    }

    public void Win()
    {
        WinScreen.SetActive(true);
        WinScreen.GetComponentInChildren<TextMeshProUGUI>().SetText("<b><size=+200%><align=center>YOU WIN!</size></b>\nYou're a great listener!\nAverage Distance from Center: " + meter.calculateAvgDistance());
    }

    public void Lose()
    {
        LoseScreen.SetActive(true);
    }

    public void PlayAgain()
    {
        PopupPanel.open = false; // TODO this is really crappy and temporary
        screenWipe.WipeIn();
        screenWipe.PostWipe += ReloadGame;
    }
    public void StartSequence()
    {
        startedSequence = true;


        dialog.setSource(new DialogSource("[c] Blah blah blah."));
        dialog.reading = true;
        gameSequence.Play("24hrEmployee");

        button.stopInputs = false;
    }

    public void FinishedSequence()
    {
        startedSequence = false;
        Debug.Log("Finished sequence");
        Win();
        button.stopInputs = false;
    }

    public void UpdateSliderIcons()
    {
        if (Mathf.Abs(LaughMeter.laughter - 5000) > -1 && Mathf.Abs(LaughMeter.laughter - 5000) <= 1000)
        {
            speedIcon.sprite = emote1;
            laughIcon.sprite = emote1;
        }
        else if (Mathf.Abs(LaughMeter.laughter - 5000) <= 2600)
        {
            speedIcon.sprite = emote2;
            laughIcon.sprite = emote2;
        }
        else if (Mathf.Abs(LaughMeter.laughter - 5000) <= 3800)
        {
            speedIcon.sprite = emote3;
            laughIcon.sprite = emote3;
        }
        else if (Mathf.Abs(LaughMeter.laughter - 5000) <= 5000)
        {
            speedIcon.sprite = emote4;
            laughIcon.sprite = emote4;
        }
    }
}
