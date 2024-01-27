using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWipe : MonoBehaviour
{
    public Action PostWipe;
    public static bool over = false;
    public void WipeIn()
    {
        over = false;
        GetComponent<Animator>().SetTrigger("WipeIn");
    }

    public void WipeOut()
    {
        GetComponent<Animator>().SetTrigger("WipeOut");
    }
    
    public void CallPostWipe()
    {
        PostWipe?.Invoke();
    }

    public void ScreenRevealed()
    {
        over = true;
    }
}
