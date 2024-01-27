using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public SpriteRenderer button;
    public Sprite not_pressed;
    public Sprite pressed;
    public KeyCode buttonToPress;
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
            //show the red circle (player pressing down on button)
            print("Q");
            button.sprite = pressed;
        }

        if (Input.GetKeyUp(buttonToPress))
        {
            button.sprite = not_pressed;
            //back to white square (player not pressing the button)
        }
    }
}
