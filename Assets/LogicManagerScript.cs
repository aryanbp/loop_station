using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LogicManagerScript : MonoBehaviour
{
    public GameObject controler;
    public GameObject playBar;
    public GameObject recorddubBar;
    public GameObject pauseBar;
    public GameObject undoBar;
    public GameObject editButton;
    public GameObject settingPanel;
    public GameObject Ifx_A;
    public GameObject Ifx_B;
    public GameObject Ifx_C;
    public Sprite select;
    public Sprite unselect;
    public GameObject centerControl;

    public TextMeshProUGUI Label;
    public TextMeshProUGUI SettingsLabel;
    public bool on=false;
    public bool off=false;
    public bool looping=false;
    public bool editOn = false;
    public bool sync = false;
    public bool undoOn= false;
    public List<Transform> children = new List<Transform>();
    GameObject[] recordedAudio;
    private float clipLength;
    public static bool memory = false;
    public static bool system = false;
    public static bool edit = false;
    public static bool rhythm = false;
    public static bool fx=false;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        /*//AudioSource audioSource = GetComponent<AudioSource>();
        //audioSource.clip = Microphone.Start(Microphone.devices[0], true, 10, 44100);
        //audioSource.Play();

        *//*for (int i = 0; i < startRecordingButtons.Length; i++)
        {
            startRecordingButtons[i].onClick.AddListener(audio.StartRecording);
        }
        for (int i = 0; i < startRecordingButtons.Length; i++)
        {
            stopRecordingButtons[i].onClick.AddListener(audio.StopRecording);
        }*/
        Label.text = "";
        foreach (Transform c in controler.transform)
        { 
            children.Add(c);
        }
        /*Debug.Log("Input Devices:");
        foreach (var device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }*/
        Debug.Log("Output Devices:");
       /* foreach (var device in DirectSoundOut.Devices)
        {
            Debug.Log(device.Description); // Display device names
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (looping && GetComponent<Audio>().PanelSettings.GetComponent<SettingsPanelScript>().buffer["Track 1: Measure"][0] == "AUTO")
        {
            recordedAudio=GameObject.FindGameObjectsWithTag("Loop1");
            if (!recordedAudio[0].GetComponent<AudioSource>().isPlaying)
            {
                foreach (GameObject audio in recordedAudio)
                {
                    audio.GetComponent<AudioSource>().Play();
                }
            }
            /*clipLength = recordedAudio.clip.length;
            if (recordedAudio.time>=0 && recordedAudio.time<=0.005)
            {
                // Perform your function here when the audio clip finishes
                //Debug.Log(recordedAudio.time+"Audio clip finished playing.");
                sync = true;
            }*/
        }
        if (!looping)
        {
            recordedAudio = GameObject.FindGameObjectsWithTag(GetComponent<Audio>().name_tag);
            if(recordedAudio.Length > 0)
            {
                if (!recordedAudio[0].GetComponent<AudioSource>().isPlaying)
                {
                    Click();
                    GetComponent<Audio>().StopRecording();
                }
            }
        }
    }
    public void reset()
    {
        if (looping)
        {
            children[1].gameObject.SetActive(false);
            children[2].gameObject.SetActive(false);
            children[3].gameObject.SetActive(false);
            children[5].gameObject.transform.GetChild(1).gameObject.GetComponent<ProgressBar>().Func_StopUIAnim();
            children[5].gameObject.transform.GetChild(0).gameObject.SetActive(true);
            children[5].gameObject.transform.GetChild(1).gameObject.SetActive(false);
            on = false;
            off = false;
            looping = false;
            Label.text = "";
            pauseBar.SetActive(false);
            playBar.SetActive(false);
            playBar.GetComponent<ProgressBar>().Func_StopUIAnim();
            if (centerControl.GetComponent<CenterControlLogic>().undo)
            {
                centerControl.GetComponent<CenterControlLogic>().UndoRedo();
            }
            settingPanel.GetComponent<SettingsPanelScript>().exit_option();
            playBar.GetComponent<ProgressBar>().m_Speed = .04f;
            if (centerControl.GetComponent<CenterControlLogic>().allStart)
            {
                centerControl.GetComponent<CenterControlLogic>().allStart = false;
                centerControl.GetComponent<CenterControlLogic>().allStartStop.transform.GetChild(0).gameObject.SetActive(false);
                centerControl.GetComponent<CenterControlLogic>().allStartStop.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
}
    public void Click() 
    {
        if (centerControl.GetComponent<CenterControlLogic>().undo && on && looping)
        {
            GameObject[] audioObjects = GameObject.FindGameObjectsWithTag(GetComponent<Audio>().name_tag);

            if (audioObjects.Length > 1)
            {
                if (undoOn)
                {
                    audioObjects[audioObjects.Length - 1].GetComponent<AudioSource>().name = "LoopAudioSource";
                    undoBar.GetComponent<ProgressBar>().m_SpriteArray[0] = unselect;
                    undoOn= false; 
                }
                else
                {
                    audioObjects[audioObjects.Length - 1].GetComponent<AudioSource>().name = "Undo";
                    undoBar.GetComponent<ProgressBar>().m_SpriteArray[0] = select;
                    undoOn = true;
                }

            }
        }
        else if (settingPanel.GetComponent<SettingsPanelScript>().oneShot)
        {
            playBar.GetComponent<ProgressBar>().Func_RestartUIAnim();
            GameObject[] audioObjects = GameObject.FindGameObjectsWithTag("Loop1");
            foreach(GameObject obj in audioObjects)
            {
                obj.GetComponent<AudioSource>().Play();
            }
        }
        else if (on)
        {
            if (editOn)
            {
                EditAnim();
            }
            if (looping & off)
            {
                off = false;
                children[2].gameObject.SetActive(true);
                Label.text = "Looping";
                pauseBar.SetActive(false);
                playBar.SetActive(true);
                playBar.GetComponent<ProgressBar>().Func_PlayUIAnim();
                children[5].gameObject.transform.GetChild(1).gameObject.SetActive(true);
                if (centerControl.GetComponent<CenterControlLogic>().allStart)
                {
                    centerControl.GetComponent<CenterControlLogic>().allStart = false;
                    centerControl.GetComponent<CenterControlLogic>().allStartStop.transform.GetChild(0).gameObject.SetActive(false);
                    centerControl.GetComponent<CenterControlLogic>().allStartStop.transform.GetChild(1).gameObject.SetActive(true);
                }
            }
            else if (looping)
            {
                children[2].gameObject.SetActive(false);
                children[1].gameObject.SetActive(true);
                looping = false;
                Label.text = "Overdubbing";
                playBar.SetActive(false);
                playBar.GetComponent<ProgressBar>().Func_StopUIAnim();
                recorddubBar.SetActive(true);
                recorddubBar.GetComponent<ProgressBar>().Func_PlayUIAnim();

            }
            else
            {
                children[1].gameObject.SetActive(!on); 
                children[3].gameObject.SetActive(false);
                children[2].gameObject.SetActive(true);
                if (children[5].gameObject.transform.GetChild(0).gameObject)
                {
                    children[5].gameObject.transform.GetChild(1).gameObject.SetActive(true);
                }
                Label.text = "Looping";
                looping = true;
                recorddubBar.SetActive(false);
                recorddubBar.GetComponent<ProgressBar>().Func_StopUIAnim();
                playBar.SetActive(true);
                playBar.GetComponent<ProgressBar>().Func_PlayUIAnim();
            }

        }
        else
        {
            if (editOn)
            {
                EditAnim();
            }
            on = true;
            children[3].gameObject.SetActive(true);
            Label.text = "Recording";
            recorddubBar.SetActive(true);
            recorddubBar.GetComponent<ProgressBar>().Func_PlayUIAnim();

        }
    }
    public void Pause() 
    {
        if(off && looping)
        {
            children[2].gameObject.SetActive(true);
            off = false;
            if (centerControl.GetComponent<CenterControlLogic>().allStart)
            {
                centerControl.GetComponent<CenterControlLogic>().allStart = false;
                centerControl.GetComponent<CenterControlLogic>().allStartStop.transform.GetChild(0).gameObject.SetActive(false);
                centerControl.GetComponent<CenterControlLogic>().allStartStop.transform.GetChild(1).gameObject.SetActive(true);
            }
            Label.text = "Looping";
            pauseBar.SetActive(false);
            playBar.SetActive(true);
            playBar.GetComponent<ProgressBar>().Func_RestartUIAnim();

        }
        else if(looping) 
        {
            children[2].gameObject.SetActive(false);
            off = true;
            Label.text = "Pause";
            playBar.SetActive(false);
            playBar.GetComponent<ProgressBar>().Func_StopUIAnim();
            pauseBar.SetActive(true); 
            if (editOn)
            {
                children[5].gameObject.transform.GetChild(0).gameObject.GetComponent<ProgressBar>().Func_StopUIAnim();
                children[5].gameObject.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }
    public void EditAnim() {
        if (!editOn && on && looping || !editOn)
        {
            EditButtonClick();
            editOn = true; 
            if (children[5].gameObject.transform.GetChild(1).gameObject)
            {
                children[5].gameObject.transform.GetChild(1).gameObject.SetActive(false);
            }
            children[5].gameObject.transform.GetChild(0).gameObject.GetComponent<ProgressBar>().Func_PlayUIAnim();
            
        }
        else if(editOn && on && looping)
        {
            editOn = false;
            children[5].gameObject.transform.GetChild(0).gameObject.GetComponent<ProgressBar>().Func_StopUIAnim();
            children[5].gameObject.transform.GetChild(1).gameObject.SetActive(true);
            settingPanel.GetComponent<SettingsPanelScript>().exit_option();
        }
        else if(editOn)
        {
            editOn = false;
            children[5].gameObject.transform.GetChild(0).gameObject.GetComponent<ProgressBar>().Func_StopUIAnim();
            settingPanel.GetComponent<SettingsPanelScript>().exit_option();
        }
    }
    public void EditButtonClick()
    {
        if (!editOn)
        {
            edit = true;
            memory = false;
            system = false;
            rhythm = false;
            fx = false;
            settingPanel.GetComponent<SettingsPanelScript>().index = -1;
            SettingsLabel.text = "TRACK SETTINGS";
            Label.text = "";
        }
    }
    public void MemoryButtonClick()
    {
        memory = true;
        system = false;
        rhythm = false;
        edit = false;
        fx = false;
        settingPanel.GetComponent<SettingsPanelScript>().index = -1;
        SettingsLabel.text = "MEMORY SETTINGS";
        Label.text = "";
    }
    public void SystemButtonClick()
    {
        system = true;
        memory = false;
        rhythm = false;
        edit = false;
        fx = false;
        settingPanel.GetComponent<SettingsPanelScript>().index = -1;
        SettingsLabel.text = "SYSTEM SETTINGS";
        Label.text = "";
    }
    public void RhythmButtonClick()
    {
        rhythm = true;
        memory = false;
        system = false;
        edit = false;
        fx = false;
        settingPanel.GetComponent<SettingsPanelScript>().index = -1;
        SettingsLabel.text = "RHYTHM SETTINGS";
        Label.text = "";
    }
    public void Ifx_Click(GameObject obj)
    {
        Dictionary<string,int> option = new Dictionary<string, int> {
            {"A",0 },
            {"B",1},
            {"C",2},
        };
        obj.transform.GetChild(0).gameObject.SetActive(!obj.transform.GetChild(0).gameObject.activeSelf);
        obj.transform.GetChild(1).gameObject.SetActive(!obj.transform.GetChild(1).gameObject.activeSelf);
        fx = true;
        memory = false;
        system = false;
        rhythm = false;
        edit = false;
        if (obj.transform.GetChild(0).gameObject.activeSelf)
        {
            Debug.Log(option[obj.name]);
            settingPanel.GetComponent<SettingsPanelScript>().index = option[obj.name];
            SettingsLabel.text = settingPanel.GetComponent<SettingsPanelScript>().fxSettings.Keys.ToList()[option[obj.name]];
            /*settingPanel.GetComponent<SettingsPanelScript>().next_option();*/
            settingPanel.GetComponent<SettingsPanelScript>().opt = obj.transform.GetChild(0).gameObject.activeSelf;
        }
        else
        {
            settingPanel.GetComponent<SettingsPanelScript>().exit_option();
        }
    }
}
