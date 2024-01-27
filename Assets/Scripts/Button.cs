using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public SpriteRenderer button;
    public Sprite not_pressed;
    public Sprite pressed;
    public KeyCode buttonToPress;
    public LaughMeter laughMeter;
    public int laughRecovery = 40;

    public bool stopInputs = true;

    // Start is called before the first frame update
    void Start()
    {
        button = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stopInputs)
            return;

        if (Input.GetKeyDown(buttonToPress))
        {
            //show the circle (player pressing down on button)
            button.sprite = pressed;
            LaughMeter.laughter += (int)(laughRecovery * LaughMeter.difficultyScalar);
        }

        if (Input.GetKeyUp(buttonToPress))
        {
            button.sprite = not_pressed;
            //back to square (player not pressing the button)
        }
    }
}
