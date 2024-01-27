using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{

    public Animator gameSequence;
    public LaughMeter meter;
    public DialogController dialog;
    public Slider speedMeter;

    public bool startedSequence = false;
    public bool suspendSequence = false;

    public Image speedIcon;
    public Image laughIcon;

    public Sprite emote1;
    public Sprite emote2;
    public Sprite emote3;
    public Sprite emote4;

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
            GameManager.main.Lose();
            button.stopInputs = true;
        }

        UpdateSliderIcons();
        
    }

    public void IntroSequence()
    {
        dialog.setSource(new DialogSource("This guys about to talk to you. [exit]"));
        dialog.reading = true;
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
        GameManager.main.Win();
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
