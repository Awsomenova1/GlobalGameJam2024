using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//reads and reacts to button presses that are part of gameplay
public class Button : MonoBehaviour
{
    public SpriteRenderer button;
    public Sprite not_pressed;
    public Sprite pressed;
    //keyboard key that will be listened for
    public KeyCode buttonToPress;
    public LaughMeter laughMeter;
    //ammount to laugh bar that each keypress will add
    public int laughRecovery = 40;
    //decides if inputs should be read, true when game play is paused
    public bool stopInputs = true;

    // Start is called before the first frame update
    void Start()
    {
        button = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stopInputs || GameplayManager.paused)
            return;

        //when selected key pressed down, add to laughter meter
        if (Input.GetKeyDown(buttonToPress))
        {
            //show the circle (player pressing down on button)
            button.sprite = pressed;

            LaughMeter.laughter += (int)(laughRecovery * LaughMeter.difficultyScalar);
        }

        if (Input.GetKeyUp(buttonToPress))
        {
            //back to square (player not pressing the button)
            button.sprite = not_pressed;
        }
    }
}
