using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWipe : MonoBehaviour
{
    public Action PostWipe;
    public void WipeIn()
    {
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
}
