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


    void Awake()
    {
        main = this;
        textDisplay.ForceMeshUpdate();

        textDisplay.OnPreRenderText += applyTextEffects;

    }

    private void Start()
    {
        setSource(new DialogSource("This is a [TFX,Wave,5,5,50]tester[/TFX,Wave]! [w, 7] [c]"));
        reading = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (reading)
        {
            textDisplay.text = source.read(readMode);
            textDisplay.ForceMeshUpdate();
            
        }
        
        if (main == null || main != this)
            main = this;
    }


    public void setSource(DialogSource newSource)
    {
        if (source != null)
        {
            source.clear -= OutputCleared;
            source.addEffect -= AddEffect;
            source.removeEffect -= RemoveEffect;
        }
        source = newSource;
        text = "";
        //headerDisplay.text = "";
        textEffects.Clear();
        newSource.clear += OutputCleared;
        newSource.addEffect += AddEffect;
        newSource.removeEffect += RemoveEffect;

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

}
