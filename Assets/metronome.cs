using UnityEngine;
using UnityEngine.UI;

public class Metronome : MonoBehaviour
{
    public float beatsPerMinute = 120f; // Initial BPM
    public AudioClip metronomeSound; // Sound for the metronome
    public GameObject tapTempo;
    private AudioSource audioSource;
    private float beatInterval;
    private float beatTimer;
    private bool isRunning = false; // Flag to track if the metronome is running

    void Start()
    {
        // Calculate the interval between beats based on BPM
        beatInterval = 60f / beatsPerMinute;

        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();
        // Set the AudioClip for the AudioSource
        audioSource.clip = metronomeSound;
    }

    void Update()
    {
        if (isRunning)
        {
            // Update the beat timer
            beatTimer -= Time.deltaTime;
            // If it's time for a beat
            if (beatTimer <= 0f)
            {
                // Play the metronome sound
                audioSource.Play();

                // Reset the beat timer
                beatTimer += beatInterval;

                // Enable tap tempo image when the sound starts
                tapTempo.SetActive(true);

                // Schedule disabling tap tempo image after the sound ends
                Invoke("DisableTapTempo", metronomeSound.length);
            }
        }

        // Recalculate the interval between beats based on the current BPM
        beatInterval = 60f / beatsPerMinute;
    }

    // Function to start or stop the metronome
    public void OnMetronome()
    {
        isRunning = !isRunning;
        if (isRunning)
        {
            // Start the metronome
            beatTimer = beatInterval;
            // Play the metronome sound immediately when starting
            audioSource.Play();
        }
        else
        {
            // Stop the metronome sound
            audioSource.Stop();
            // Disable tap tempo image
            tapTempo.SetActive(false);
        }
    }

    // Method to disable tap tempo image
    private void DisableTapTempo()
    {
        tapTempo.SetActive(false);
    }
}
