using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class TextEffect
{
    public int start = 0;
    //End of -1 means no end
    public int end = -1;

    public string type;

    public abstract void ApplyEffectToMesh(TMP_TextInfo textMesh);

    public static TextEffect GetEffect(string name)
    {
        switch(name)
        {
            case "Wave" or "wave":
                return new TextWave();
            case "Shake" or "shake":
                return new TextShake();
            case "Wiggle" or "wiggle":
                return new TextWiggle();
            default:
                Debug.LogError("Failed to find a text effect of name " + name);
                return null;
        }
    }

    public static TextEffect MakeEffect(string[] name, int start = 0)
    {
        TextEffect returnEffect = null;
        if (name[0][0] == ' ')
            name[0] = name[0].Substring(1);

        switch (name[0])
        {
            case "Wave" or "wave":
                if(name.Length == 4)
                    returnEffect = new TextWave(float.Parse(name[1]), float.Parse(name[2]), float.Parse(name[3]));
                else
                    Debug.LogError("Not enough parameters for Wave effect!");
                break;
            case "Shake" or "shake":
                if (name.Length == 2)
                    returnEffect = new TextShake(float.Parse(name[1]));
                else
                    Debug.LogError("Not enough parameters for shake effect!");
                break;
            case "Wiggle" or "wiggle":
                if (name.Length == 3)
                    returnEffect = new TextWiggle(float.Parse(name[1]), float.Parse(name[2]));
                else
                    Debug.LogError("Not enough parameters for wiggle effect! " + name.Length);
                break;
            default:
                Debug.LogError("Failed to find a text effect of name " + name[0]);
                break;
        }
        if(returnEffect != null)
            returnEffect.start = start;
        return returnEffect;

    }
}
