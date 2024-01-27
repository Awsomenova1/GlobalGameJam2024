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

        for(float i = 1; i <= 60; i += .5f)
        {
            InputQueue.Add(new ButtonPrompt(i));
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePositions();
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
        for(int i = 0; i < InputQueue.Count; i++)
        {
            if(Input.GetKeyDown(InputQueue[i].keyToHit) && Mathf.Abs(InputQueue[i].TimeToHit - timeInSequence) < .2f)
            {
                
            }
        }
    }

    public void UpdatePositions()
    {
        for(int i = 0; i < InputQueue.Count; i++)
        {
            if (Mathf.Abs(InputQueue[i].TimeToHit - timeInSequence) < 10)
            {
                if (InputQueue[i].associatedObj == null)
                    InputQueue[i].associatedObj = Instantiate(keyPrefab, Vector2.right * (InputQueue[i].TimeToHit - timeInSequence) * TimeToDistanceMult, Quaternion.identity, transform);

                InputQueue[i].associatedObj.transform.position = Vector2.right * (InputQueue[i].TimeToHit - timeInSequence) * TimeToDistanceMult;

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


    public ButtonPrompt(float time)
    {
        TimeToHit = time;
        keyToHit = KeyCode.Space;
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
