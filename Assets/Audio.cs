using UnityEngine;
using UnityEngine.UI;
using System.IO;
using NAudio.Wave;
using System;
using System.Net;

public class Audio : MonoBehaviour
{
    WaveInEvent waveIn;
    WaveFileWriter writer;
    bool isRecording = false;

    public Button startRecordingButton;
    public Button stopRecordingButton;
    public AudioSource audioSource;

    MemoryStream recordedAudioStream;

    public void Start()
    {
        // Set up the buttons
        startRecordingButton.onClick.AddListener(StartRecording);
        stopRecordingButton.onClick.AddListener(StopRecording);
    }

    public void StartRecording()
    {
        if (!isRecording)
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
            AudioClip recordedClip = AudioClip.Create("RecordedAudio", buffer.Length / 2, 1, waveIn.WaveFormat.SampleRate, false);
            recordedClip.SetData(BytesToFloat(buffer), 0);

            // Play the recorded audio clip
            audioSource.clip = recordedClip;
            audioSource.Play();

            isRecording = false;
        }
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
