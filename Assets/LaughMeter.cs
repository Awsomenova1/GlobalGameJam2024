using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaughMeter : MonoBehaviour
{
    //current laughter meter value
    public int laughter;
    //minimum and maximum laugh bar values, set to 0-10000 range for high customizability via speed
    private int minLaughter = 0;
    private int maxLaughter = 10000;
    private int startLaughter = 0;
    //the how many points the laughter bar decreases by each frame (1/60 second)
    public int laughSpeed;

    public Slider laughBarFill;

    // Start is called before the first frame update
    void Start()
    {
        startLaughter = maxLaughter / 2;
        laughBarFill.minValue = minLaughter;
        laughBarFill.maxValue = maxLaughter;
        laughter = startLaughter;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(laughter > minLaughter && laughter < maxLaughter){
            laughter -= laughSpeed;
        }
        laughBarFill.value = laughter;
    }


}
