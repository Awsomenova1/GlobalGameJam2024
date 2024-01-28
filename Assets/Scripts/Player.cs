using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//controls when particular animations play for the player character
public class Player : MonoBehaviour
{
    public Animator anim;
    int laughLevel = 0;

    private void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        //change the player's sprite based on the current laugh level
        if(Mathf.Abs(LaughMeter.laughter - 5000) <= 1000)
        {
            laughLevel = 0;
        }
        else if (Mathf.Abs(LaughMeter.laughter - 5000) <= 2600)
        {
            laughLevel = 1;
        }
        else if (Mathf.Abs(LaughMeter.laughter - 5000) <= 3800)
        {
            laughLevel = 2;
        }
        else if (Mathf.Abs(LaughMeter.laughter - 5000) <= 5000)
        {
            laughLevel = 3;
        }
        anim.SetInteger("LaughLevel", laughLevel);
    }
}
