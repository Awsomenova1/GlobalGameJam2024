using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogController : MonoBehaviour
{
    public static DialogController main;

    public TextMeshProUGUI textDisplay;

    public DialogSource source;

    public bool reading = false;

    public List<TextEffect> textEffects = new List<TextEffect>();

    public AK.Wwise.Event NPC_Talk, NPC_Stop, NPC_Tired, NPC_Angry;

    public bool talking = false;
    public LaughMeter meter;

    public string text
    {
        get
        {
            return textDisplay.text;
        }
        set
        {
            textDisplay.text = value;
        }
    }

    public DialogSource.ReadMode readMode = DialogSource.ReadMode.DEFAULT;

    //Animator of NPC
    public Animator anim;

    bool collected = false;

    void Awake()
    {
        main = this;
        textDisplay.ForceMeshUpdate();
        NPC_Tired.Post(gameObject);

        textDisplay.OnPreRenderText += applyTextEffects;
    }

    private void Start()
    {
        //setSource(new DialogSource("This[w, 1] is a[ss, .2] [TFX,Wave,5,5,50]tester[/TFX,Wave]! [w, 7] [c][ss, .05] And again to test wrapping! [exit]"));
        //reading = true;
    }

    // Update is called once per frame
    void Update()
    {
        OtherUpdate();
    }
    void OtherUpdate()
    {
        if (reading)
        {
            if (!collected)
            {
                textDisplay.maxVisibleCharacters = 0;
                textDisplay.text = source.collect();
                collected = true;
            }
            source.read(DialogSource.ReadMode.TYPEWRITE);
            textDisplay.maxVisibleCharacters = source.charCount;
        }
        if (collected)
            textDisplay.ForceMeshUpdate();

        if (reading && source.NotWaiting && !meter.inResponseWindow)
        {
            if (!talking)
            {
                NPC_Talk.Post(gameObject);
                talking = true;
            }
        }
        else
        {
            if (talking)
            {
                NPC_Stop.Post(gameObject);
                talking = false;
            }
        }
    }


    public void setSource(DialogSource newSource)
    {
        if (source != null)
        {
            source.clear -= OutputCleared;
            source.addEffect -= AddEffect;
            source.removeEffect -= RemoveEffect;
            source.exit -= close;
            source.playAnimation -= playAnimation;
            source.ps -= PlaySound;
            source.setEmot -= SetEmotion;

        }
        source = newSource;
        text = "";
        //headerDisplay.text = "";
        textEffects.Clear();
        newSource.clear += OutputCleared;
        newSource.addEffect += AddEffect;
        newSource.removeEffect += RemoveEffect;
        newSource.exit += close;
        source.playAnimation += playAnimation;
        source.ps += PlaySound;
        source.setEmot += SetEmotion;

    }

    public void applyTextEffects(TMP_TextInfo info)
    {
        for (int i = 0; i < textEffects.Count; i++)
        {
            textEffects[i].ApplyEffectToMesh(info);
        }
    }

    public void RemoveEffect(string type)
    {
        if (type[0] == ' ')
            type = type.Substring(1);
        for(int i = textEffects.Count - 1; i >= 0; i--)
        {
            if(textEffects[i].type.ToUpperInvariant().Contains(type.ToUpperInvariant()))
            {
                textEffects[i].end = GetLengthNoCommandsRealTime();
                return;
            }
        }
        Debug.LogWarning("There was no effect of type " + type + " to end!");
    }

    public void AddEffect(string[] input)
    {
        string[] newInput = new string[input.Length - 1];
        for(int i = 1; i < input.Length; i++)
        {
            newInput[i - 1] = input[i];
        }
        TextEffect effect = TextEffect.MakeEffect(newInput, GetLengthNoCommandsRealTime());
        if(effect != null)
            textEffects.Add(effect);
    }

    public void OutputCleared()
    {
        textEffects.Clear();
        collected = false;
    }

    public int GetLengthNoCommands()
    {
        int depth = 0;
        int length = 0;
        for(int i = 0; i < text.Length; i++)
        {
            if (text[i] == '<')
                depth++;
            if (depth == 0)
                length++;

            if (text[i] == '>')
                depth--;
        }
        return length;
    }

    /// <summary>
    /// Reads the next part of the string that hasnt been "pushed" yet instead of what is already out
    /// </summary>
    /// <returns></returns>
    public int GetLengthNoCommandsRealTime()
    {
        int depth = 0;
        int length = 0;
        for (int i = 0; i < source.outString.Length; i++)
        {
            if (source.outString[i] == '<')
                depth++;
            if (depth == 0)
                length++;

            if (source.outString[i] == '>')
                depth--;
        }
        return length;
    }

    public void close()
    {
        reading = false;
        collected = false;
        textDisplay.maxVisibleCharacters = int.MaxValue;
    }

    public void playAnimation(string name)
    {
        anim.Play("name");
    }

    public void PlaySound(string name, float volume, bool loop)
    {

    }

    public void SetEmotion(int emotionId)
    {
        anim.SetFloat("Emotion", emotionId);
    }
}
