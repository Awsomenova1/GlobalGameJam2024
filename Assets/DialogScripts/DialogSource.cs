using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class DialogSource
{
    public string originalDialog;

    public string dialog;
    public int position;
    public int charCount;

    //Time to wait between words
    public float speed = 0.075f;
    float lastReadTime = 0;

    public bool skipSpaceWait = true;

    float waitTime = 0;
    float waitStart;


    public string outString = "";

    public static Dictionary<string, string> stringVariables = new Dictionary<string, string>();
    public static Dictionary<string, int> counterVariables = new Dictionary<string, int>();

    private bool waitFrameForChar = false;

    public event Action<String, float, bool> ps;
    public event Action<String> es;

    public event Action exit;
    public event Action speak, pauseSpeak, stopSpeak;

    //public event Action<string[]> requestOptionsStart;


    public event Action startWaitingForInput;

    public event Action clear;

    public event Action<string[]> addEffect;

    public event Action<string> removeEffect;

    //Is just used for things like DialogNpc to have reactions to certain events
    public event Action<string[]> callEvent;

    public bool waiting = false;
    public bool waitingForButtonInput = false;

    public Dictionary<string, string> responseOutputs = new Dictionary<string, string>();
    public List<string> responseOutputsNumeric = new List<string>();

    public Dictionary<string, string> dialogBlocks = new Dictionary<string, string>();

    public const string BLOCKS_SIGNATURE = "-Blocks";

    public bool skippingText = false;
    //number of characters/second
    /*Dialogue guide:
     * [] is escape
     * [b] is bark 
     * [b, a, v, c] is bark, acceleration, velocity, count
     * [w, t] is wait with time
     * [ss, s] is setspeed with speed
     * [abf] is auto bark frequency
     * [abf, min, max] sets the random range of abf, with max being exclusive
     * [j, pos] jumps position to new position
     * 
     * 
     * CONSTRAIN VAR SIZES SO THAT DIALOG ISNT OFFSET BY IT TOO MUCH
     * [svar, var, val] sets a var with a value. If it does not exist it is created
     * [gvar, var] reads a var and puts the value of it on the dialog out
     * [gvar, var, exists, else] //Outputs one option if a var exists, and the other if it does not
     * [rvar, var] //Removes a var. Gives a warning if the var does not exist.
     * [gvar, var, var2, true, false] //Checks if a var equals another var
     * 
     * 
     * [mcount, var, op, val] //Modify a counter variable or create one. operators can be +, -, /, *
     * [gcount, var, op, val, T] //Read counter, output based on it. operators can be >, <, <=, >=, =, !=. Output T if true
     * [gcount, var, op, val, T, F] 
     * [series, var, 1, 2, 3, 4, ...]// given a counter, output dialog that relates that that count % the entries
     * [lseries, var, 1, 2, 3, 4, ...] //Same as series but loops
     * 
     * [exit] exits dialog (closes the box, but is typically controlled by the thing holding it)
     * [sdb, x, y, x,y ] sets default bark settings
     * 
     * [c] clear output box
     * 
     * [sh, val] // sets header to a new string
     * [sh] //clears header
     * 
     * [prompt, 1, 1r, 2, 2r, ...] //Prompts with a number of options with their names and their results
     * 
     * [lf, path] //Sets output to be dialog from a text file at the path
     * [lf, path, block] //Loads a block from a specific path
     * [llf, block] //Loads a different block from within the current file
     * 
     * [wi] //Waits for user input to continue
     * 
     * [IA, text] //Instantly adds some text. Meant for use with tmppro styling
     * 
     * [CE, p1, p2, p3, ...] //Calls an event. Only works if the thing running it has a listener to the event. Parameters are parsed by the listener.
     *
     * [ps, sound] //Plays sound from a specified sound path as the speaker
     * [ps, sound, volume] //Plays sound with specified volume
     * [ps, sound, volume, loop] //Plays sound with specified volume and loopability (true/false)
     * 
     * [es] //Ends all sounds by this speaker
     * [es, sound] //Ends a particular sound by this speaker
     *
     * [bgm, music] //Plays background music from a specified sound path
     * [bgm, music, duration] //Plays background music, crossfading over specified duration
     * [bgm, music, duration, area] //Plays background music, crossfading over specified duration, while setting new music area
     *
     * [pbgm] //Pauses background music
     * [upbgm] //Unpauses background music
     *
     * [fibgm] //Fades background music in
     * [fibgm, duration] //Fades background music in over specified duration
     *
     * [fobgm] //Fades background music out
     * [fobgm, duration] //Fades background music out over specified duration
     * 
     * [ame, mixer, effect, value] //Applies effect to audio mixer
     * [ame, mixer, effect, value, duration] //Applies effect to audio mixer over specified duration
     * 
     * [GQC,id, true, false] //Gets if quest with id was completed or not. Outputs string in true or false section depending on it
     * [GQP,id, true, false] //Gets if quest with id has needed progression to complete or not. Outputs string in true or false section depending on it
     * [GQA,id, true, false] //Gets if quest with id was assigned or not. Outputs string in true or false section depending on it
     */
    public DialogSource(string dialog)
    {
        this.originalDialog = dialog;
        this.dialog = dialog;
    }

    public static Dictionary<string, string> getBlocks(string loadedText)
    {
        Dictionary<string, string> blocks = new Dictionary<string, string>();
        if (loadedText.Length > BLOCKS_SIGNATURE.Length && loadedText.Substring(0, 7) == BLOCKS_SIGNATURE)
        {
            int lastBlockStart = 0;
            for (int i = BLOCKS_SIGNATURE.Length; i < loadedText.Length; i++)
            {
                if (loadedText[i] == '{')
                {
                    
                    string titleChunk = loadedText.Substring(lastBlockStart, i - lastBlockStart);
                    //titleChunk = titleChunk.LastIndexOf('\n');
                    string blockName;
                    if (titleChunk.LastIndexOf('\n') != -1)
                        blockName = loadedText.Substring(titleChunk.LastIndexOf('\n') + lastBlockStart + 1, i - 1 - (titleChunk.LastIndexOf('\n') + lastBlockStart));
                    else
                        continue;
                    //Maybe change to use a short string series instead?
                    string blockText = loadedText.Substring(i + 3, getCommandEnd(loadedText, i, '{', '}') - (i + 3));
                    blocks.Add(blockName, blockText);
                    lastBlockStart = i + 1;
                }
            }
        }
        return blocks;
    }
    public static DialogSource LoadFromFile(string filePath)
    {
        string loadedText = LoadFile(filePath);
        Dictionary<string, string> blocks = getBlocks(loadedText);

        DialogSource returnDialog;
        if (blocks.ContainsKey("Default"))
            returnDialog = new DialogSource(blocks["Default"]);
        else
            returnDialog = new DialogSource(loadedText);
        returnDialog.dialogBlocks = blocks;
        return returnDialog;
    }

    public static string LoadFile(string filePath)
    {
        string gottenText = File.ReadAllText(Path.Combine(Application.streamingAssetsPath + @"\Dialog\", filePath));
        return gottenText;
    }

    public void changeToFile(string filePath)
    {
        string loadedText = LoadFile(filePath);
        Dictionary<string, string> blocks = getBlocks(loadedText);
        if (blocks.ContainsKey("Default"))
            dialog = blocks["Default"];
        else
            dialog = loadedText;
        dialogBlocks = blocks;
        position = 0;
        charCount = 0;
        waiting = false;
    }

    public void changeToBlock(string block)
    {
        if (dialogBlocks.ContainsKey(block))
        {
            //dialog = dialogBlocks[block];
            //position = 0;
            dialog = dialog.Insert(position, dialogBlocks[block]);
            waiting = false;
        }
        else
            Debug.LogError("Couldnt find a block called " + block + ". Make sure you are in the right file!");
    }

    public enum ReadMode
    {
        DEFAULT, COLLECT, TYPEWRITE
    }

    public string collect()
    {
        int originalPosition = position;
        if (waiting || waitingForButtonInput || dialog == null)
            return outString;
        
        skippingText = true;
        while (skippingText)
        {
            readDialog(ReadMode.COLLECT);

            if (position >= dialog.Length - 1)
            {
                skippingText = false;
                //Debug.LogWarning("Speed was left at 0, this could prevent anything else from running! Always return speed to non-zero once finishing");
            }
        }
        position = originalPosition;
        charCount = 0;
        return outString;
    }

    public string read(ReadMode mode = ReadMode.DEFAULT)
    {
        if (waiting || waitingForButtonInput || dialog == null)
        {
            pauseSpeak?.Invoke();
            return outString;
        }
        while ((lastReadTime + speed < Time.time) && (Time.time > waitStart + waitTime) || skippingText)
        {
            speak?.Invoke();
            lastReadTime = Time.time;
            readDialog(mode);

            if (position == dialog.Length - 1 && speed == 0)
            {
                skippingText = false;
                Debug.LogWarning("Speed was left at 0, this could prevent anything else from running! Always return speed to non-zero once finishing");
            }
        }
        return outString;
    }

    private static int getCommandEnd(string input, int startPosition, char openChar = '[', char closeChar = ']')
    {
        int depth = 0;
        for(int i = startPosition; i < input.Length; i++)
        {
            if (input[i] == openChar)
                depth++;
            if (input[i] == closeChar)
                depth--;

            if (depth == 0)
                return i;
        }
        return input.Length;
    }
    
    private void readDialog(ReadMode mode = ReadMode.DEFAULT)
    {
        if (waiting || waitingForButtonInput)
            return;
        ///TODO: calling loadfile seems to delay text appearing by like half a second, since the prompts always take half a second to appear.
        while (position < dialog.Length && dialog[position] == '[')
        {
            //int endPos = dialog.IndexOf(']', position);
            int endPos = getCommandEnd(dialog, position);
            List<string> parameters = new List<string>();
            int depth = 0;

            int lastProcessPosition = position;
            for (int i = lastProcessPosition; i <= endPos; i++)
            {
                if (dialog[i] == '[')
                    depth++;
                if (dialog[i] == ']')
                    depth--;
                if ((dialog[i] == ',' && depth == 1) || i == endPos)
                {
                    parameters.Add(dialog.Substring(lastProcessPosition + 1, i - lastProcessPosition - 1));
                    lastProcessPosition = i;
                }
            }
            position = endPos + 1;

            if ((mode == ReadMode.COLLECT && parameters[0] != "c") || mode != ReadMode.COLLECT)
            {
                if (mode != ReadMode.TYPEWRITE) // TODO this may need to be changed to account for effects that should not run in collect mode
                    processStringEffect(mode, parameters.ToArray());
                else if (parameters[0] != "TFX" && parameters[0] != "/TFX") // TODO this may need to be expanded to cover other effects that should not run in typewrite mode
                    processStringEffect(mode, parameters.ToArray());
            }

            if (mode == ReadMode.COLLECT && parameters[0] == "c")
                skippingText = false;
        }
        if (position >= dialog.Length)
            return;

        if (waitFrameForChar)
        {
            waitFrameForChar = false;
            return;
        }
        if (mode != ReadMode.TYPEWRITE)
            outString += dialog[position];
        position++;
        charCount++;


        while (skipSpaceWait && dialog.Length > position && (dialog[position] == ' ' || dialog[position] == '\n' || dialog[position] == '\t'))
        {
            if (mode != ReadMode.TYPEWRITE)
                outString += dialog[position];
            position++;
            charCount++;
        }

    }
    public void processStringEffect(ReadMode mode = ReadMode.DEFAULT, params string[] input)
    {
        if (input.Length == 0)
        {
            return;
        }
        //Whitelist so that typewrite only runs certain commands (ones that collect didn't run)
        if(mode == ReadMode.TYPEWRITE)
        {
            if (input[0] != "w" && input[0] != "c" && input[0] != "exit")
            {
                return;
            }
        }
        switch (input[0])
        {

            case "ss":
                if (input.Length == 2)
                {
                    speed = float.Parse(input[1]);
                }
                else
                    Debug.LogWarning("Invalid number of parameters for set speed(ss)!");
                break;

            case "w":
                if (input.Length == 2)
                {
                    if (!skippingText)
                    {
                        waitTime = float.Parse(input[1]);
                        waitStart = Time.time;
                        pauseSpeak?.Invoke();
                    }
                }
                else
                    Debug.LogWarning("Invalid number of parameters for wait(w)!");
                break;
            case "c":
                outString = "";
                skippingText = false;
                clear?.Invoke();
                break;
            case "j":
                if (input.Length == 2)
                {
                    position = int.Parse(input[1]);
                    waitFrameForChar = true;
                }
                else
                    Debug.LogWarning("Invalid number of parameters for jump (j)!");
                break;
            case "exit":
                skippingText = false;
                if (mode != ReadMode.COLLECT)
                {
                    stopSpeak?.Invoke();
                    exit?.Invoke();
                }
                break;
            case "lf":
                if (input.Length == 2)
                {
                    changeToFile(input[1]);
                }
                else if (input.Length == 3)
                {
                    changeToFile(input[1]);
                    changeToBlock(input[2]);
                }
                else
                    Debug.LogWarning("Invalid number of arguments for loadFile [lf]!");
                break;
            case "llf":
                if (input.Length == 2)
                {
                    changeToBlock(input[1]);
                }
                else
                    Debug.LogError("Invalid number of arguments for loadLocalFile [llf]!");
                break;
            case "wi":
                if (input.Length == 1)
                {
                    waitingForButtonInput = true;
                    pauseSpeak?.Invoke();
                    startWaitingForInput?.Invoke();
                    skippingText = false;
                }
                else
                    Debug.LogError("Invalid number or arguments for waitInput[wi]!");
                break;
            case "svar":
                if (input.Length == 3)
                {
                    if (!stringVariables.ContainsKey(input[1]))
                        stringVariables.Add(input[1], input[2]);
                    else
                        stringVariables[input[1]] = input[2];
                }
                else
                    Debug.LogError("Invalid number or arguments for set var [svar]!");
                break;
            case "gvar":
                if (input.Length == 2)
                {
                    if (stringVariables.ContainsKey(input[1]))
                        dialog = dialog.Insert(position, stringVariables[input[1]]);
                }
                else if (input.Length == 4)
                {
                    if (stringVariables.ContainsKey(input[1]))
                        dialog = dialog.Insert(position, input[2]);
                    else
                        dialog = dialog.Insert(position, input[3]);
                }
                else if (input.Length == 5)
                {
                    if (stringVariables.ContainsKey(input[1]))
                    {
                        if (stringVariables[input[1]] == input[2])
                            dialog = dialog.Insert(position, input[3]);
                        else
                            dialog = dialog.Insert(position, input[4]);
                    }
                }
                else
                    Debug.LogError("Invalid number or arguments for get var [gvar]!");
                break;
            case "rvar":
                if (input.Length == 2)
                {
                    if (stringVariables.ContainsKey(input[1]))
                        stringVariables.Remove(input[1]);
                    else
                        Debug.LogWarning("No variable called " + input[1] + " found, this may be intentional, but make sure that the referenced variable is named correctly " +
                            "(remember whitespace is counted)");
                }
                break;
            case "IA":
                if (input.Length == 2)
                {
                    outString += input[1];
                }
                else
                    Debug.LogWarning("Instant Add has too many parameters!");
                break;
            case "TFX":
                addEffect?.Invoke(input);
                break;
            case "/TFX":
                if (input.Length == 2)
                    removeEffect?.Invoke(input[1]);
                else
                    Debug.LogWarning("/TFX only takes one parameter!");
                break;
            case "CE":
                callEvent?.Invoke(input[1..]);
                break;
            case "mcount":
                if (input.Length == 4)
                {
                    if (!counterVariables.ContainsKey(input[1]))
                    {
                        counterVariables.Add(input[1], 0);
                    }

                    switch (input[2])
                    {
                        case "+":
                            counterVariables[input[1]] += int.Parse(input[3]);
                            break;
                        case "-":
                            counterVariables[input[1]] -= int.Parse(input[3]);
                            break;
                        case "/":
                            if (int.Parse(input[3]) == 0)
                            {
                                Debug.LogWarning("Counters: Cannot divide by 0!");
                                break;
                            }
                            counterVariables[input[1]] /= int.Parse(input[3]);
                            break;
                        case "*":
                            counterVariables[input[1]] *= int.Parse(input[3]);
                            break;
                        case "=":
                            counterVariables[input[1]] = int.Parse(input[3]);
                            break;
                        default:
                            Debug.LogWarning("No valid operation for counters " + input[1]);
                            break;
                    }

                }
                else
                    Debug.LogWarning("mcount takes 4 parameters!");
                break;

            case "gcount":
                if (input.Length == 5 || input.Length == 6)
                {
                    if(counterVariables.ContainsKey(input[1]))
                    {
                        bool output = false;
                        
                        switch(input[2])
                        {
                            case ">":
                                output = counterVariables[input[1]] > int.Parse(input[3]);
                                break;
                            case "<":
                                output = counterVariables[input[1]] < int.Parse(input[3]);
                                break;
                            case "<=":
                                output = counterVariables[input[1]] <= int.Parse(input[3]);
                                break;
                            case ">=":
                                output = counterVariables[input[1]] >= int.Parse(input[3]);
                                break;
                            case "=" or "==":
                                output = counterVariables[input[1]] == int.Parse(input[3]);
                                break;
                            case "!=":
                                output = counterVariables[input[1]] != int.Parse(input[3]);
                                break;
                            default:
                                Debug.LogWarning("No valid operation for counters compare " + input[1]);
                                break;
                        }
                        if (output)
                            dialog = dialog.Insert(position, input[4]);
                        else if (input.Length == 6)
                            dialog = dialog.Insert(position, input[5]);
                    }
                }
                else
                    Debug.LogWarning("gcount takes 5 or 6 parameters!"); 
                break;

            case "dcounter": //display counter
                if (input.Length == 2)
                {
                    if (counterVariables.ContainsKey(input[1]))
                        dialog = dialog.Insert(position, "" + counterVariables[input[2]]);
                    else
                        dialog = dialog.Insert(position, "" + 0);
                }
                else
                    Debug.LogWarning("dcounter takes 2 parameters!");
                break;

            case "series":
                if (input.Length > 3)
                {
                    if (counterVariables.ContainsKey(input[1]))
                    {
                        if(counterVariables[input[1]] + 2 < input.Length)
                            dialog = dialog.Insert(position, input[counterVariables[input[1]] + 2]);
                        else
                            dialog = dialog.Insert(position, input[input.Length - 1]);
                    }
                    else
                        dialog = dialog.Insert(position, input[2]);
                }
                else
                    Debug.LogWarning("series takes a series after the variable to read!");
                break;
            case "lseries":
                if (input.Length > 2)
                {
                    if (counterVariables.ContainsKey(input[1]))
                    {

                        dialog = dialog.Insert(position, input[2 + (counterVariables[input[1]] % (input.Length - 2))]);
                    }
                    else
                        dialog = dialog.Insert(position, input[2]);
                }
                else
                    Debug.LogWarning("series takes a series after the variable to read!");
                break;

            #region old audio stuff
            //case "ps":
            //    if (input.Length == 2)
            //    {
            //        playSound(input[1]);
            //    }
            //    else if (input.Length == 3)
            //    {
            //        playSound(input[1], float.Parse(input[2]));
            //    }
            //    else if (input.Length == 4)
            //    {
            //        bool loop = false;
            //        if (input[3].Trim().ToLower() == "true" || input[3].Trim().ToLower() == "t")
            //            loop = true;
            //        else if (input[3].Trim().ToLower() == "false" || input[3].Trim().ToLower() == "f")
            //            loop = false;
            //        else
            //            Debug.LogWarning("Invalid format for play sound [ps] loop parameter! Defaulting to false");
            //        playSound(input[1], float.Parse(input[2]), loop);
            //    }
            //    else
            //        Debug.LogWarning("Invalid number of parameters for play sound [ps]!");
            //    break;

            //case "es":
            //    if (input.Length == 1)
            //        endSound();
            //    else if (input.Length == 2)
            //        endSound(input[1]);
            //    else
            //        Debug.LogWarning("Invalid number of parameters for end sound [es]!");
            //    break;

            //case "bgm":
            //    if (input.Length == 2)
            //    {
            //        setBGM(input[1]);
            //    }
            //    else if (input.Length == 3)
            //    {
            //        setBGM(input[1], float.Parse(input[2]));
            //    }
            //    else if (input.Length == 4)
            //    {
            //        setBGM(input[1], float.Parse(input[2]), input[3]);
            //    }
            //    else
            //        Debug.LogWarning("Invalid number of parameters for background music [bgm]!");
            //    break;

            //case "pbgm":
            //    if (input.Length == 1)
            //        AudioManager.instance.PauseCurrent();
            //    else
            //        Debug.LogError("Invalid number or arguments for pause BGM [pbgm]!");
            //    break;

            //case "upbgm":
            //    if (input.Length == 1)
            //        AudioManager.instance.UnPauseCurrent();
            //    else
            //        Debug.LogError("Invalid number or arguments for unpause BGM [upbgm]!");
            //    break;

            //case "fibgm":
            //    if (input.Length == 1)
            //        AudioManager.instance.FadeInCurrent();
            //    else if (input.Length == 2)
            //        AudioManager.instance.FadeInCurrent(float.Parse(input[1]));
            //    else
            //        Debug.LogError("Invalid number or arguments for fade in BGM [fibgm]!");
            //    break;

            //case "fobgm":
            //    if (input.Length == 1)
            //        AudioManager.instance.FadeOutCurrent();
            //    else if (input.Length == 2)
            //        AudioManager.instance.FadeOutCurrent(float.Parse(input[1]));
            //    else
            //        Debug.LogError("Invalid number or arguments for fade out BGM [fibgm]!");
            //    break;

            //case "ame":
            //    if (input.Length == 4)
            //        applyMixerEffect(input[1], input[2], float.Parse(input[3]));
            //    else if (input.Length == 5)
            //        applyMixerEffect(input[1], input[2], float.Parse(input[3]), float.Parse(input[4]));
            //    else
            //        Debug.LogError("Invalid number or arguments for audio mixer effect [ame]!");
            //    break;
            #endregion
            default:
                Debug.LogWarning("Found empty or invalid dialog command " + input[0]);
                //Maybe make it just output the input (i.e. [tester]) if there is no command found and assume that it was not intended as a command call
                break;

        }
    }

    public void resetDialog()
    {
        dialog = originalDialog;
        outString = "";
        position = 0;
        charCount = 0;
        clear?.Invoke();
    }


    public void playSound(string sound, float volume = 1, bool loop = false)
    {
        ps?.Invoke(sound, volume, loop);
    }
    public void endSound(string sound = null)
    {
        es?.Invoke(sound);
    }

    //public void setBGM(string music, float duration = 1)
    //{
    //    AudioManager.instance.ChangeBGM(music, duration);
    //}
    //public void setBGM(string music, float duration, string area)
    //{
    //    AudioManager.instance.ChangeBGM(music, area, duration);
    //}
    //public void applyMixerEffect(string mixer, string effect, float value, float duration = 0)
    //{
    //    AudioManager.instance.ApplyMixerEffect(mixer, effect, value, duration);
    //}
}
