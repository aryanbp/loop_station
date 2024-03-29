using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using static Unity.VisualScripting.Metadata;

public class SettingsPanelScript : MonoBehaviour
{
    public TextMeshProUGUI Label;
    public TextMeshProUGUI State;
    public AudioMixerGroup loop1;
    public AudioMixerGroup track;
    string loopVolume = "VolumeLoop1";
    public bool oneShot = false;

    public Dictionary<string,List<string>> buffer = new Dictionary<string, List<string>>() {
        { "Memory: Level", new List<string>()},
        { "Memory: Comp",   new List<string>() },
        { "Memory: Reverb",new List < string >() },
        {"Memory: Name",   new List < string >{"ON", "0.1" } },
         { "Sys: Auto Off", new List < string >{"ON", "0.1" }},
        {"Sys: LineOut Level", new List < string >() },
         { "Track 1: Reverse", new List < string >{ "OFF", "250" } },
        { "Track 1: PlayLevel",new List < string >()},
        { "Track 1: 1Short", new List < string >{ "OFF", "250" } },
        { "Track 1: Track FX", new List < string >{ "ON", "0.1" } },
        { "Track 1: Play Mode", new List < string >{ "MULTI", "0.1" } },
        { "Track 1: Measure", new List < string >{ "FREE", "0.1" } },
        { "Track 1: Loop Sync", new List < string >{ "OFF", "250" } },
        { "Track 1: Tempo Sync", new List < string >{ "OFF", "250" } },
        {"Rhythm: Level",new List < string >()},
        { "Rhythm: Beat",new List < string >{ "4/4", "0.4" } },
        { "Rhythm: Line Out", new List < string >{ "ON", "0.1" } },
        { "Rhythm: PlayCount", new List < string >{ "ON", "0.1" } }
    };
    Dictionary<string, List<string>> memorySettings = new Dictionary<string, List<string>>(){
        { "Memory: Level", new List<string>()},
        { "Memory: Comp",  new List<string>() },
        { "Memory: Reverb",new List<string>() },
        {"Memory: Name",   new List<string>{"ON", "OFF" } }
    };
    Dictionary<string, List<string>> systemSettings = new Dictionary<string, List<string>>(){
        { "Sys: Auto Off", new List<string>{"ON", "OFF" } },
        {"Sys: LineOut Level", new List<string>() }
    };

    Dictionary<string, List<string>> editSettings = new Dictionary<string, List<string>>(){
        { "Track 1: Reverse", new List<string>{"ON", "OFF" } },
        { "Track 1: PlayLevel",new List<string>()},
        { "Track 1: 1Short", new List<string>{"ON", "OFF" } },
        { "Track 1: Track FX", new List<string>{"ON", "OFF" } },
        { "Track 1: Play Mode", new List < string > { "MULTI", "SINGLE" } },
        { "Track 1: Measure", new List < string > { "FREE", "AUTO" } },
        { "Track 1: Loop Sync", new List < string > { "ON", "OFF" } },
        { "Track 1: Tempo Sync", new List < string > { "ON", "OFF" } }
    };
    
    Dictionary<string, List<string>> rhythmSettings = new Dictionary<string, List<string>>(){ 
        {"Rhythm: Level",new List<string>()}, 
        { "Rhythm: Beat", new List < string > { "2/4", "3/4", "4/4", "5/4", "6/4","7/4" } },
        { "Rhythm: Line Out", new List < string > { "ON", "OFF" } },
        { "Rhythm: PlayCount", new List < string > { "ON", "OFF" } } 
    };

    List<string> settings;
    string selectedSetting;
    public int index = -1;
    public bool knobReset=false;
    public bool knobSet = false;
    public float z;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(settings);
    }

    // Update is called once per frame
    void Update()
    {
        if(LogicManagerScript.memory || LogicManagerScript.system || LogicManagerScript.edit || LogicManagerScript.rhythm)
        {
            if (LogicManagerScript.memory)
            {
                selectedSetting = "memory";
                settings = memorySettings.Keys.ToList();
            }
            else if (LogicManagerScript.system)
            {
                selectedSetting = "system";
                settings = systemSettings.Keys.ToList();
            }
            else if (LogicManagerScript.edit)
            {
                selectedSetting = "track";
                settings = editSettings.Keys.ToList();
            }
            else if (LogicManagerScript.rhythm)
            {
                selectedSetting = "rhythm";
                settings = rhythmSettings.Keys.ToList();
            }
        }

    }
    public void next_option() {
        if (index < settings.Count-1)
        {
            State.text = "";
            index++;
            Label.text = settings[index];
            if (buffer[settings[index]].Count>0 && buffer[settings[index]][0] != "")
            {
                State.text = buffer[settings[index]][0];
                z = float.Parse(buffer[settings[index]][1]);
                knobSet = true;
            }
            else
            {
                knobReset = true;
            }
        }
    }
    public void prev_option()
    {
        if (index>0)
        {
            State.text = "";
            index--;
            Label.text = settings[index];
            if (buffer[settings[index]].Count > 0  && buffer[settings[index]][0] != "")
            {
                State.text = buffer[settings[index]][0];
                z = float.Parse(buffer[settings[index]][1]);
                knobSet = true;
            }
            else
            {
                knobReset = true;
            }
        }
    }
    public void exit_option() {
        knobReset = true;
        index = -1;
        Label.text = "01 INIT MEMORY";
        State.text = "";
        settings =new List<string>();
    }
    public void FunctionSelected(float value, float z)
    {
        if (index > -1)
        {
            if(selectedSetting == "memory") {
                List<string> options = memorySettings[settings[index]];
                ValueHandeler(settings[index], options, value,z);
            }
            else if (selectedSetting == "system")
            {
                List<string> options = systemSettings[settings[index]];
                ValueHandeler(settings[index], options, value,z);
            }
            if (selectedSetting == "track")
            {
                List<string> options = editSettings[settings[index]];
                ValueHandeler(settings[index], options, value,z);
            }
            if (selectedSetting == "rhythm")
            {
                List<string> options = rhythmSettings[settings[index]];
                ValueHandeler(settings[index], options, value,z);
            }

        }
    }
    public void ValueHandeler(string function, List<string> options, float value, float z)
    {
        if (buffer[Label.text].Count > 2)
        {
            buffer[Label.text].Clear();
        }
        //buffer[Label.text].Clear();
        if (options.Count > 0)
        {
            if (options.Count == 2) {
                if (value < 50)
                {
                    State.text = options[0];
                    buffer[Label.text].Add(options[0]);
                    buffer[Label.text].Add(z.ToString());
;                }
                else { 
                    State.text = options[1];
                    buffer[Label.text].Add(options[1]);
                    buffer[Label.text].Add(z.ToString());
                }
            }
            else
            {
                int index = Mathf.FloorToInt(value / 100f * (options.Count - 1)); // Calculate index based on slider value and list length
                index = Mathf.Clamp(index, 0,options.Count - 1); // Clamp index to valid range (0 to list.Count-1)
                State.text = options[index];
                buffer[Label.text].Add(options[index]);
                buffer[Label.text].Add(z.ToString());
            }
        }
        else
        {
            State.text = value.ToString();
            buffer[Label.text].Add(value.ToString());
            buffer[Label.text].Add(z.ToString());
        }
        FunctionHandler(function, buffer[Label.text][0]);
    }
    public void FunctionHandler(string func, string value)
    {
        GameObject[] audioObjects = GameObject.FindGameObjectsWithTag("Loop1");
        if (func== "Track 1: Reverse")
        {
            if(audioObjects.Length > 0)
            {
                if (value == "ON" && audioObjects[0].GetComponent<AudioSource>().pitch == 1)
                {
                    foreach (GameObject obj in audioObjects)
                    {
                        obj.GetComponent<AudioSource>().pitch = -1;
                    }
                }
                else if (value == "OFF" && audioObjects[0].GetComponent<AudioSource>().pitch == -1)
                {
                    foreach (GameObject obj in audioObjects)
                    {
                        obj.GetComponent<AudioSource>().pitch = 1;
                    }
                }
            }
        }
        if (func== "Track 1: PlayLevel") {
            loop1.audioMixer.SetFloat(loopVolume, (int.Parse(value) - 1) * (20 - (-80)) / (100 - 1) - 80);
        }
        if (func== "Track 1: 1Short")
        {
            if (audioObjects.Length > 0)
            {
                if (value == "ON" && !oneShot)
                {
                    oneShot = true;
                    foreach (GameObject obj in audioObjects)
                    {
                        obj.GetComponent<AudioSource>().loop=false;
                        obj.GetComponent<AudioSource>().Pause();
                    }
                    foreach (GameObject obj in audioObjects)
                    {
                        obj.GetComponent<AudioSource>().Play();
                    }
                }
                else if (value == "OFF" && oneShot)
                {
                    oneShot = false;
                    foreach (GameObject obj in audioObjects)
                    {
                        obj.GetComponent<AudioSource>().loop = true;
                        obj.GetComponent<AudioSource>().Play();
                    }
                }
            }
        }
    }
}
