using UnityEngine;
using UnityEngine.UI;
using System.IO;
using NAudio.Wave;
using System;
using System.Net;
using UnityEditor;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.Assertions.Must;

public class Audio : MonoBehaviour
{
    WaveInEvent waveIn;
    WaveFileWriter writer;
    bool isRecording = false;
    public bool isMute=false;
    public GameObject PanelSettings;
    public GameObject CenterControl;

    public Button[] startRecordingButtons;
    public Button[] stopRecordingButtons;
    public List<AudioSource> audioSources = new List<AudioSource>();
    public Slider slider;
    public LogicManagerScript logicUI;
    public AudioMixerGroup loop;
    public AudioMixerGroup track;
    public string name_tag;

    MemoryStream recordedAudioStream;

    public void liveAudio()
    {
        
    }
    public void Start()
    {
        // Set up the buttons
        for(int  i = 0; i < startRecordingButtons.Length; i++) { 
            startRecordingButtons[i].onClick.AddListener(StartRecording); 
        }
        for (int i = 0; i < startRecordingButtons.Length; i++)
        {
            stopRecordingButtons[i].onClick.AddListener(StopRecording);
        }
        slider.onValueChanged.AddListener(SetVolumne);
    }
    public void SetVolumne(float value) {
        foreach (AudioSource audioSource in audioSources)
        { 
            audioSource.volume = value/100;
        }
    }

    public void StartRecording()
    {
        if (!isRecording && !isMute && !CenterControl.GetComponent<CenterControlLogic>().undo && !PanelSettings.GetComponent<SettingsPanelScript>().oneShot)
        {
            // Set up WaveInEvent to capture audio from the default microphone
            waveIn = new WaveInEvent();
            waveIn.DeviceNumber = 0; // Change this number if you want to use a different microphone device
            waveIn.WaveFormat = new WaveFormat(44100, 1); // Adjust format as needed

            // Set up a MemoryStream to hold the recorded audio
            recordedAudioStream = new MemoryStream();

            // Set up event handlers for capturing audio
            waveIn.DataAvailable += OnDataAvailable;
            waveIn.StartRecording();

            isRecording = true;
        }
        else
        {
            PausePlayRecording();
        }
    }

    public void StopRecording()
    {
        if (isRecording)
        {
            // Stop recording and clean up resources
            waveIn.StopRecording();
            waveIn.Dispose();


              // Set the position of the MemoryStream back to the beginning
            recordedAudioStream.Position = 0;

            // Load the recorded audio into an AudioClip
            var waveStream = new RawSourceWaveStream(recordedAudioStream, waveIn.WaveFormat);
            byte[] buffer = new byte[recordedAudioStream.Length];
            waveStream.Read(buffer, 0, buffer.Length);
            AudioClip recordedClip = AudioClip.Create(name_tag+"RecordedAudio", buffer.Length / 2, 1, waveIn.WaveFormat.SampleRate, false);
            recordedClip.SetData(BytesToFloat(buffer), 0);

            // Play the recorded audio clip
            // Instantiate a new GameObject to hold the audio source
            GameObject newAudioObject = new GameObject(name_tag+"AudioSource");
            newAudioObject.tag = name_tag;

            // Add an AudioSource component to the new GameObject
            AudioSource audioSource = newAudioObject.AddComponent<AudioSource>();
            GameObject[] audioObjects = GameObject.FindGameObjectsWithTag(name_tag);

            if (audioObjects.Length > 1)
            {
                audioObjects[audioObjects.Length - 2].GetComponent<AudioSource>().outputAudioMixerGroup=loop;
            }
            audioSource.clip = recordedClip;
            audioSource.time = audioSource.clip.length - 0.01f;
            if (GetComponent<LogicManagerScript>().playBar.GetComponent<ProgressBar>().m_Speed == .04f) {
                GetComponent<LogicManagerScript>().playBar.GetComponent<ProgressBar>().m_Speed = audioSource.clip.length / 9;
            }
            audioSource.tag = name_tag;
            audioSource.outputAudioMixerGroup = track;
            audioSource.Play();
            if (PanelSettings.GetComponent<SettingsPanelScript>().buffer["Track 1: Measure"][0] == "FREE")
            {
                audioSource.loop = true;
            }  

            audioSources.Add(audioSource);
            isRecording = false;
        }
    }
    public void PausePlayRecording()
    {
        if (GetComponent<LogicManagerScript>().off & GetComponent<LogicManagerScript>().on)
        {
            foreach (AudioSource audioSource in audioSources) {
                audioSource.mute = true;
                isMute = true;
            }
                
        }
        else
        {
            foreach (AudioSource audioSource in audioSources)
            {
                audioSource.Play();
                audioSource.mute = false;
                isMute = false;
            }
        }
    }
    public void undoRecording() {
        int index = audioSources.Count - 1;
        audioSources[index].Stop();
        audioSources.RemoveAt(index);

    }
    public void ClearRecording()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (!audioSource.IsUnityNull())
            {
                Destroy(audioSource.gameObject);
            }
        }
        audioSources.Clear();
        isRecording = false;
        isMute = false;
    }

    void OnDataAvailable(object sender, WaveInEventArgs e)
    {
        // Write audio data to the MemoryStream
        recordedAudioStream.Write(e.Buffer, 0, e.BytesRecorded);
    }

    float[] BytesToFloat(byte[] bytes)
    {
        float[] floats = new float[bytes.Length / 2];
        for (int i = 0; i < bytes.Length; i += 2)
        {
            short sample = (short)((bytes[i + 1] << 8) | bytes[i]);
            floats[i / 2] = sample / 32768.0f;
        }
        return floats;
    }

}
