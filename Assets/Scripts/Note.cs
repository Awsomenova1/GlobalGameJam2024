using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public bool canBePressed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Button")
        {
            canBePressed = true;
        }

        //have another collider that changes buttonToPress based on this markers letter
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Button")
        {
            canBePressed = false;
        }
    }

}
