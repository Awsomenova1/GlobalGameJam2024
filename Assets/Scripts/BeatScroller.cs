using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatScroller : MonoBehaviour
{
    public float beatTempo;

    public bool hasStarted;

    public List<ButtonPrompt> InputQueue = new List<ButtonPrompt>();

    public float startTime;

    public float TimeToDistanceMult = 5;

    public Vector2 basePosition;

    public GameObject keyPrefab;
    public Animator hitEffects;

    public float timeInSequence
    {
        get
        {
            return Time.time - startTime;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        beatTempo = beatTempo / 60f;
        basePosition = transform.position;
        string keysToHit = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        for(float i = 1; i <= 60; i += .5f)
        {
            KeyCode toHit = (KeyCode)System.Enum.Parse(typeof(KeyCode), "" + keysToHit[(int)(i / .5f) % keysToHit.Length]);
            InputQueue.Add(new ButtonPrompt(i, toHit));
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePositions();
        DetectInputs();
        //if (!hasStarted)
        //{
        //    if (Input.anyKeyDown)
        //    {
        //        hasStarted = true;
        //    }
        //}
        //else
        //{
        //    transform.position -= new Vector3(beatTempo * Time.deltaTime, 0f, 0f);
        //}
    }

    public void DetectInputs()
    {
        bool hitSomething = false;


        for(int i = 0; i < InputQueue.Count; i++)
        {
            if(Input.GetKeyDown(InputQueue[i].keyToHit) && Mathf.Abs(timeInSequence -InputQueue[i].TimeToHit ) < .2f)
            {
                if (InputQueue[i].associatedObj != null)
                {
                    Destroy(InputQueue[i].associatedObj);
                    InputQueue[i].hit = true;

                    if (Mathf.Abs(timeInSequence - InputQueue[i].TimeToHit) < .1f)
                        hitEffects.Play("clickHit");
                    else
                        hitEffects.Play("click");

                    hitSomething = true;

                }
            }

            if (timeInSequence - InputQueue[i].TimeToHit > .5f && !InputQueue[i].missed && !InputQueue[i].hit)
            {
                hitEffects.Play("clickMiss");
                InputQueue[i].missed = true;
            }
        }

        if (!hitSomething && Input.anyKeyDown)
            hitEffects.Play("clickWrong");

        
    }

    public void UpdatePositions()
    {
        for(int i = 0; i < InputQueue.Count; i++)
        {
            if (Mathf.Abs(InputQueue[i].TimeToHit - timeInSequence) < 10)
            {
                if (InputQueue[i].associatedObj == null && !InputQueue[i].hit)
                {
                    InputQueue[i].associatedObj = Instantiate(keyPrefab, transform);
                    InputQueue[i].associatedObj.GetComponentInChildren<TMPro.TextMeshPro>().text = InputQueue[i].keyToHit.ToString();
                }
                
                if(InputQueue[i].associatedObj != null)
                    InputQueue[i].associatedObj.transform.localPosition= Vector2.right * (InputQueue[i].TimeToHit - timeInSequence) * TimeToDistanceMult;

            }
            else if (InputQueue[i].associatedObj != null)
            {
                Destroy(InputQueue[i].associatedObj);
                InputQueue[i].associatedObj = null;

            }

        }
    }

    public void BeginScroll()
    {
        InputQueue.Sort();
        startTime = Time.time;
    }


}

public class ButtonPrompt
{
    public float TimeToHit;
    public KeyCode keyToHit;
    public GameObject associatedObj;
    public bool hit = false;
    public bool missed = false;


    public ButtonPrompt(float time)
    {
        TimeToHit = time;
        keyToHit = KeyCode.Space;
    }

    public ButtonPrompt(float time, KeyCode key)
    {
        TimeToHit = time;
        keyToHit = key;
    }

    public static bool operator <(ButtonPrompt a, ButtonPrompt b)
    {
        return a.TimeToHit < b.TimeToHit;
    }
    
    public static bool operator >(ButtonPrompt a, ButtonPrompt b)
    {
        return a.TimeToHit > b.TimeToHit;
    }

    public static bool operator <=(ButtonPrompt a, ButtonPrompt b)
    {
        return a.TimeToHit <= b.TimeToHit;
    }

    public static bool operator >=(ButtonPrompt a, ButtonPrompt b)
    {
        return a.TimeToHit >= b.TimeToHit;
    }

    public static bool operator ==(ButtonPrompt a, ButtonPrompt b)
    {
        return a.TimeToHit == b.TimeToHit;
    }

    public static bool operator !=(ButtonPrompt a, ButtonPrompt b)
    {
        return a.TimeToHit != b.TimeToHit;
    }

    public override bool Equals(object obj)
    {
        return (TimeToHit == ((ButtonPrompt)obj).TimeToHit && keyToHit == ((ButtonPrompt)obj).keyToHit);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
