using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaughMeter : MonoBehaviour
{
    //current laughter meter value
    public static int laughter;
    //minimum and maximum laugh bar values, set to 0-10000 range for high customizability via speed
    private int minLaughter = 0;
    private int maxLaughter = 10000;
    private int startLaughter = 0;
    //the how many points the laughter bar decreases by each frame (1/60 second)
    public int laughSpeed;

    public Slider laughBarFill;

    //total distance past the threashold player was throught the game;
    [SerializeField]
    public int totalDistance;
    //times distance from center sampled
    public int countDistance;
    //distance away from start value that is considered within perfect accuracy range
    [SerializeField]
    private int threashold;
    //number of frames (1/60 sec) between when distances are recorded
    [SerializeField]
    private int timeToCount = 60;
    //number of frames since the last time distance recorded
    private int timeSinceCount;

    public static float difficultyScalar = 2;

    public List<(string resp1, string resp2, string resp3)> responseQueue = new List<(string resp1, string resp2, string resp3)>();

    public TMPro.TextMeshProUGUI response1;
    public TMPro.TextMeshProUGUI response2;
    public TMPro.TextMeshProUGUI response3;

    //Messy, :(
    public Image responseButton1;
    public Image responseButton2;
    public Image responseButton3;

    public Sprite buttonSprite;
    public Sprite buttonSelectedSprite;

    public bool inResponseWindow = false;
    private bool inResponseWindowLastFrame = false;

    public int currResponse;

    public Player playerAnim;

    public CanvasGroup responseGroup;
    public CanvasGroup dialogGroup;

    public int selectedResponse = 0;

    // Start is called before the first frame update
    void Start()
    {
        startLaughter = maxLaughter / 2;
        laughBarFill.minValue = minLaughter;
        laughBarFill.maxValue = maxLaughter;
        laughter = startLaughter;

        timeSinceCount = 0;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(laughter > minLaughter && laughter < maxLaughter){
            laughter -= (int)(laughSpeed * difficultyScalar);
        }
        laughBarFill.value = laughter;

        //once per second, add current distance to running total to average later
        timeSinceCount += 1;
        if(timeSinceCount >= timeToCount){
            addDistance();
            timeSinceCount = 0;
        }
        //reqhits/second = (60 * laughSpeed)/180
        //decayRateForBpm = (180 * bpm/60)/60 = 180 * bpm/360, 180 is recovery rate

        AkSoundEngine.SetRTPCValue("laughter", laughter);
    }

    //check if the player has exceeded loss threasholds
    public bool checkLose(){
        if(laughter >= maxLaughter || laughter <= minLaughter){
            return true;
        }
        else{
            return false;
        }
    }
    private void Update()
    {
        ResponseUpdates();
    }

    //add current distance from center of bar to running total to average later
    //with threashold > 0, distance is from edge of range around center
    void addDistance(){
        if(laughter > startLaughter + threashold || laughter < startLaughter + threashold){
            totalDistance += System.Math.Abs(laughter - (startLaughter + threashold));
        }
        else{
            totalDistance += 0;
        }
        countDistance += 1;
    }

    //calculate and report the average distance from center
    public float calculateAvgDistance(){
        try {
            return totalDistance / countDistance;
        }
        catch (DivideByZeroException) // amazing
        {
            return 0;
        }
        
    }


    public void ResponseUpdates()
    {
        if (inResponseWindow && !inResponseWindowLastFrame)
        {
            response1.text = responseQueue[currResponse].resp1;
            response2.text = responseQueue[currResponse].resp2;
            response3.text = responseQueue[currResponse].resp3;



            //Open response windows
            responseGroup.alpha = 1;
            responseGroup.interactable = true;
            responseGroup.blocksRaycasts = true;

            dialogGroup.alpha = 0;
            dialogGroup.interactable = false;
            dialogGroup.blocksRaycasts = false;
            playerAnim.SetResponding(true);
        }
        else if (!inResponseWindow && inResponseWindowLastFrame)
        {
            //Close dialog windows
            responseGroup.alpha = 0;
            responseGroup.interactable = false;
            responseGroup.blocksRaycasts = false;

            dialogGroup.alpha = 1;
            dialogGroup.interactable = true;
            dialogGroup.blocksRaycasts = true;

            playerAnim.SetResponding(false);

            responseButton1.sprite = buttonSprite;
            responseButton2.sprite = buttonSprite;
            responseButton3.sprite = buttonSprite;

            currResponse++;
        }
        else if (inResponseWindow)
        {

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                responseButton1.sprite = buttonSprite;
                responseButton2.sprite = buttonSprite;
                responseButton3.sprite = buttonSprite;

                if (Input.GetKeyDown(KeyCode.A))
                {
                    responseButton1.sprite = buttonSelectedSprite;
                    selectedResponse = 1;
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    responseButton2.sprite = buttonSelectedSprite;
                    selectedResponse = 2;
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    responseButton3.sprite = buttonSelectedSprite;
                    selectedResponse = 3;
                }
            }
        }



        inResponseWindowLastFrame = inResponseWindow;
    }

    //calculate a grade for the player based on how well they kept in the center
    public string calculateGrade(){
        float avgDist = calculateAvgDistance();

        if(avgDist == 0f){
            return "<color=\"pink\">S";
        }
        else if(avgDist <= 100){
            return "<color=#6d84ff>A";
        }
        else if(avgDist <= 500){
            return "<color=#6dd037>B";
        }
        else if(avgDist <= 1000){
            return "<color=#ebd000>C";
        }
        else if(avgDist <= 2000){
            return "<color=#ff9849>D";
        }
        else{
            return "<color=#FF0000>F";
        }
    }

}
