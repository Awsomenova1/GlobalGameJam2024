using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator anim;
    int laughLevel = 0;

    //for placeholder pourposes
   // public SpriteRenderer sr;
   // public Sprite laughLevel0;
   // public Sprite laughLevel1;
   // public Sprite laughLevel2;
  //  public Sprite laughLevel3;
    private void Start()
    {
        anim = this.GetComponent<Animator>();
        //sr = this.GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if(Mathf.Abs(LaughMeter.laughter - 5000) > -1 && Mathf.Abs(LaughMeter.laughter - 5000) <= 1000)
        {
            laughLevel = 0;
            //sr.sprite = laughLevel0;
        }
        else if (Mathf.Abs(LaughMeter.laughter - 5000) > 1000 && Mathf.Abs(LaughMeter.laughter - 5000) <= 2600)
        {
            laughLevel = 1;
           // sr.sprite = laughLevel1;
        }
        else if (Mathf.Abs(LaughMeter.laughter - 5000) > 2600 && Mathf.Abs(LaughMeter.laughter - 5000) <= 3800)
        {
            laughLevel = 2;
            //sr.sprite = laughLevel2;
        }
        else if (Mathf.Abs(LaughMeter.laughter - 5000) > 3800 && Mathf.Abs(LaughMeter.laughter - 5000) <= 5000)
        {
            laughLevel = 3;
            //sr.sprite = laughLevel3;
        }
        anim.SetInteger("LaughLevel", laughLevel);
    }
}
