using UnityEngine;
using UnityEngine.UI;

public class Metronome : MonoBehaviour
{
    public float tempo = 120; // Beats per minute
    public AudioClip tickSound; // Sound to play on each tick
    private bool isPlaying = false;
    private AudioSource audioSource;

    void Start()
    {
        // Get the AudioSource component attached to the same GameObject or add one if not present
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Set the audio clip
        audioSource.clip = tickSound;
        audioSource.loop = true;

    }

    public void OnMetronome()
    {
        if (!isPlaying)
        {
            // Calculate the playback speed based on tempo
            float speed = tempo / 60f;
            ChangeAudioSpeed(audioSource, speed);
        }
    }

    void ChangeAudioSpeed(AudioSource audioSource, float newSpeed)
    {
        // Get the audio data
        float[] data = new float[audioSource.clip.samples * audioSource.clip.channels];
        audioSource.clip.GetData(data, 0);

        // Set the playback speed
        audioSource.pitch = 1f;

        // Create a new audio clip with adjusted speed
        AudioClip newClip = AudioClip.Create("TempClip", data.Length, audioSource.clip.channels, audioSource.clip.frequency, false);
        newClip.SetData(data, 0);
        audioSource.clip = newClip;

        // Set the new playback speed
        audioSource.pitch = newSpeed;
        audioSource.Play();
    }
}
