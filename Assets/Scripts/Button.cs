using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    public SpriteRenderer button;
    public Sprite not_pressed;
    public Sprite pressed;
    public KeyCode buttonToPress;
    public LaughMeter laughMeter;
    // Start is called before the first frame update
    void Start()
    {
        button = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(buttonToPress))
        {
            //show the circle (player pressing down on button)
            button.sprite = pressed;
            laughMeter.laughter += 100;
        }

        if (Input.GetKeyUp(buttonToPress))
        {
            button.sprite = not_pressed;
            //back to square (player not pressing the button)
        }
    }
}
