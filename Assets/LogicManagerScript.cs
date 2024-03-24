using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LogicManagerScript : MonoBehaviour
{
    public GameObject controler;
    public GameObject playBar;
    public GameObject recorddubBar;
    public GameObject pauseBar;
    public GameObject undoBar;
    public GameObject editButton;
    public GameObject settingPanel;
    public Sprite select;
    public Sprite unselect;

    public TextMeshProUGUI Label;
    public TextMeshProUGUI SettingsLabel;
    public bool on=false;
    public bool off=false;
    public bool looping=false;
    public bool editOn = false;
    public bool sync = false;
    public bool undoOn= false;
    public List<Transform> children = new List<Transform>();
    AudioSource recordedAudio;
    private float clipLength;
    public static bool memory = false;
    public static bool system = false;
    public static bool edit = false;
    public static bool rhythm = false;


    // Start is called before the first frame update
    void Start()
    {
        //AudioSource audioSource = GetComponent<AudioSource>();
        //audioSource.clip = Microphone.Start(Microphone.devices[0], true, 10, 44100);
        //audioSource.Play();

        /*for (int i = 0; i < startRecordingButtons.Length; i++)
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
        foreach (var device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (looping)
        {
            recordedAudio=GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();
            clipLength = recordedAudio.clip.length;
            if (recordedAudio.time>=0 && recordedAudio.time<=0.005)
            {
                // Perform your function here when the audio clip finishes
                //Debug.Log(recordedAudio.time+"Audio clip finished playing.");
                sync = true;
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
            GetComponent<CenterControlLogic>().UndoRedo();
        }
}
    public void Click() 
    {
        if (GetComponent<CenterControlLogic>().undo && on && looping)
        {
            GameObject[] audioObjects = GameObject.FindGameObjectsWithTag("Loop1");

            if (audioObjects.Length > 1)
            {
                audioObjects[audioObjects.Length - 1].GetComponent<AudioSource>().name="Undo?";
                if (undoOn)
                {
                    undoBar.GetComponent<ProgressBar>().m_SpriteArray[0] = unselect;
                    undoOn= false; 
                }
                else
                {
                    undoBar.GetComponent<ProgressBar>().m_SpriteArray[0] = select;
                    undoOn = true;
                }

            }
        }

        else if (on)
        {

            if (looping & off)
            {
                off = false;
                children[2].gameObject.SetActive(true);
                Label.text = "Looping";
                pauseBar.SetActive(false);
                playBar.SetActive(true);
                playBar.GetComponent<ProgressBar>().Func_PlayUIAnim();
                children[5].gameObject.transform.GetChild(0).gameObject.SetActive(false);
                children[5].gameObject.transform.GetChild(1).gameObject.SetActive(true);
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
                EditAnim();

            }
            else
            {
                children[1].gameObject.SetActive(!on); 
                children[3].gameObject.SetActive(false);
                children[2].gameObject.SetActive(true);
                if (children[5].gameObject.transform.GetChild(0).gameObject)
                {
                    children[5].gameObject.transform.GetChild(0).gameObject.SetActive(false);
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
            on= true;
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
            Label.text = "Looping";
            pauseBar.SetActive(false);
            playBar.SetActive(true);
            playBar.GetComponent<ProgressBar>().Func_PlayUIAnim();

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
                children[5].gameObject.transform.GetChild(1).gameObject.GetComponent<ProgressBar>().Func_StopUIAnim();
                children[5].gameObject.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }
    public void EditAnim() {
        if (!editOn && on && looping)
        {
            editOn = true;
            children[5].gameObject.transform.GetChild(1).gameObject.GetComponent<ProgressBar>().Func_PlayUIAnim();
        }
        else if(editOn)
        {
            editOn = false;
            children[5].gameObject.transform.GetChild(1).gameObject.GetComponent<ProgressBar>().Func_StopUIAnim();
            children[5].gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }
    }
    public void EditButtonClick() {
        if(!editOn)
        {
            edit = true;
            memory = false;
            system = false;
            rhythm = false;
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
        settingPanel.GetComponent<SettingsPanelScript>().index = -1;
        SettingsLabel.text = "RHYTHM SETTINGS";
        Label.text = "";
    }
}
