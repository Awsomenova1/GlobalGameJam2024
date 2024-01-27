using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{

    public Animator gameSequence;
    public LaughMeter meter;
    public DialogController dialog;

    public bool startedSequence = false;
    // Start is called before the first frame update
    void Start()
    {
        meter.laughSpeed = 0;
        IntroSequence();
    }

    // Update is called once per frame
    void Update()
    {
        if (startedSequence && gameSequence.GetCurrentAnimatorStateInfo(0).IsName("NoCurrentSequence"))
            FinishedSequence();

        if (!startedSequence && !dialog.reading && Input.GetKeyDown(KeyCode.Q))
            StartSequence();

        
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
        gameSequence.Play("24HrEmployee");
    }

    public void FinishedSequence()
    {
        startedSequence = false;
        Debug.Log("Finished sequence");
    }
}
