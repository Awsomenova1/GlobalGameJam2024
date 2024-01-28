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

    //icons for speed/heat and laugh meters
    public Image speedIcon;
    public Image laughIcon;

    //images for laugh bar face icon
    public Sprite emote1;
    public Sprite emote2;
    public Sprite emote3;
    public Sprite emote4;
    //images for speed/heat bar face icon
    public Sprite emote5;
    public Sprite emote6;
    public Sprite emote7;

    public static bool paused;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject WinScreen;
    [SerializeField] private TextMeshProUGUI WinText;
    [SerializeField] private GameObject LoseScreen;

    //value to adjust speed/heat meter to
    private float targetSpeed;
    [SerializeField] private GameObject globalWwise;
    [SerializeField] private AK.Wwise.Event PauseMusic, ResumeMusic, StopMusic, StartMusic;
    [SerializeField] private AK.Wwise.State calm, mediate, intense, silent, none;
    private enum MusicState { CALM, MEDIATE, INTENSE };
    private MusicState currentState;
    public bool won = false, lost = false, playingAgain = false, quit = false;

    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        meter.laughSpeed = 0;
        IntroSequence();
        screenWipe.gameObject.SetActive(true);
        screenWipe.WipeOut();
    }

    // Update is called once per frame
    void Update()
    {
        //pause/unpause the game when pause button pressed, but only if screen wipe is over
        if (Input.GetKeyDown(KeyCode.Escape) && ScreenWipe.over)
        {
            if (!paused && !PopupPanel.open && !won && !lost)
                Pause();
            else if (paused && PopupPanel.open)
                UnPause();
        }

        if (suspendSequence)
            return;

        if (startedSequence && gameSequence.GetCurrentAnimatorStateInfo(0).IsName("NoCurrentSequence"))
            FinishedSequence();

        if (!startedSequence && !dialog.reading && Input.GetKeyDown(KeyCode.Q))
            StartSequence();

        targetSpeed = meter.laughSpeed * 5;

        //updates speed/heat meter visuals
        speedMeter.value = Mathf.Lerp(speedMeter.value, targetSpeed, Time.deltaTime);
        if(speedMeter.value <= speedMeter.maxValue / 3){
            speedIcon.sprite = emote5;
            if (currentState != MusicState.CALM)
            {
                calm.SetValue();
                currentState = MusicState.CALM;
            }
            
        }
        else if(speedMeter.value <= speedMeter.maxValue * 2 / 3){
            speedIcon.sprite = emote7;
            if (currentState != MusicState.MEDIATE)
            {
                mediate.SetValue();
                currentState = MusicState.MEDIATE;
            }
                
        }
        else{
            speedIcon.sprite = emote6;
            if (currentState != MusicState.INTENSE)
            {
                intense.SetValue();
                currentState = MusicState.INTENSE;
            }
                
        }

        //checks if the player has lost the game, ends play if so
        if(meter.checkLose())
        {
            dialog.setSource(new DialogSource("Laughter"));

            dialog.reading = true;
            suspendSequence = true;
            while (!lost)
                Lose();
            button.stopInputs = true;
        }

        UpdateSliderIcons();

    }

    public void IntroSequence()
    {
        dialog.setSource(new DialogSource("[ss, .025]This guys about to talk to you. [exit]"));
        dialog.reading = true;
    }

    public void Pause()
    {
        PauseMusic.Post(globalWwise);
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
        paused = true;
    }

    public void UnPause()
    {
        if (!PopupPanel.open) return;
        ResumeMusic.Post(globalWwise);
        Time.timeScale = 1;
        paused = false;
        PauseMenu.GetComponent<PopupPanel>().Close();
    }

    public void QuitToMenu()
    {
        if (quit) return;
        quit = true;
        Time.timeScale = 1;
        paused = false;
        StopMusic.Post(globalWwise);
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
        if (paused) return;
        won = true;
        StopMusic.Post(globalWwise);
        WinScreen.SetActive(true);
        WinText.SetText("You made it through without laughing!\n\nFinal Grade: " + meter.calculateGrade());
        button.stopInputs = true;
    }

    public void Lose()
    {
        if (paused) return;
        lost = true;
        StopMusic.Post(globalWwise);
        LoseScreen.SetActive(true);
        button.stopInputs = true;
    }

    //resets game
    public void PlayAgain()
    {
        if (playingAgain) return;
        playingAgain = true;

        if (PopupPanel.open){
            if(PauseMenu.activeSelf == true){
                PauseMenu.GetComponent<PopupPanel>().Close();
            }
            else if(WinScreen.activeSelf == true){
                WinScreen.GetComponent<PopupPanel>().Close();
            }
            else if(LoseScreen.activeSelf == true){
                LoseScreen.GetComponent<PopupPanel>().Close();
            }
        }
        won = false;
        lost = false;

        Time.timeScale = 1;
        paused = false;
        screenWipe.WipeIn();
        StopMusic.Post(globalWwise);
        screenWipe.PostWipe += ReloadGame;
    }
    //syncs clocks for start of gameplay
    public void StartSequence()
    {
        startedSequence = true;


        //dialog.setSource(new DialogSource("[c] Blah blah blah."));
        dialog.setSource(new DialogSource("[lf,WormMartEmployee.txt]"));
        dialog.reading = true;
        gameSequence.Play("24hrEmployee");
        

        button.stopInputs = false;
    }

    public void FinishedSequence()
    {
        startedSequence = false;
        Debug.Log("Finished sequence");
        while (!won)
        {
            Win();
        }
    }

    //updates slider icon for laughter bar
    public void UpdateSliderIcons()
    {
        if (Mathf.Abs(LaughMeter.laughter - 5000) <= 1000)
        {
            laughIcon.sprite = emote1;
        }
        else if (Mathf.Abs(LaughMeter.laughter - 5000) <= 2600)
        {
            laughIcon.sprite = emote2;
        }
        else if (Mathf.Abs(LaughMeter.laughter - 5000) <= 3800)
        {
            laughIcon.sprite = emote3;
        }
        else if (Mathf.Abs(LaughMeter.laughter - 5000) <= 5000)
        {
            laughIcon.sprite = emote4;
        }
    }
}
