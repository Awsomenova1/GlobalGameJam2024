using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaughMeter : MonoBehaviour
{
    //current laughter meter value
    public int laughter;
    [SerializeField]
    private int maxLaughter = 10000;
    private int minLaughter = 0;
    //the how many points the laughter bar decreases by each frame (1/60 second)
    public int laughSpeed;

    public Slider laughBarFill;

    // Start is called before the first frame update
    void Start()
    {
        laughter = maxLaughter;
    }

    // Update is called once per frame
    void Update()
    {
        if(laughter > minLaughter){
            laughter -= laughSpeed;
        }
        laughBarFill.value = laughter;
    }


}
