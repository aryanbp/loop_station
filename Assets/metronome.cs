using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Metronome : MonoBehaviour
{
    public float beatsPerMinute = 120f; // Initial BPM
    public GameObject Settings;
    public int measure = 4;
    public AudioClip metronomeSound; // Sound for the metronome
    public GameObject tapTempo;
    public GameObject tapHit;
    private AudioSource audioSource;
    private float beatInterval;
    private float beatTimer;
    private bool isRunning = false; // Flag to track if the metronome is running
    int repet = 0;
    bool ok = false;

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
                if (repet == measure-1)
                {
                    tapHit.SetActive(true);
                    ok = true;
                    repet = 0;
                }
                else
                {
                    // Enable tap tempo image when the sound starts
                    tapTempo.SetActive(true);
                    repet++;
                }

                // Play the metronome sound
                audioSource.Play();

                // Reset the beat timer
                beatTimer += beatInterval;


                // Schedule disabling tap tempo image after the sound ends
                Invoke("DisableTapTempo", metronomeSound.length);
            }
            // Making Mapping of Function and their values to work....
            if (Settings.GetComponent<SettingsPanelScript>().buffer["Rhythm: Level"].Count > 0)
            {
                GetComponent<AudioSource>().volume = float.Parse(Settings.GetComponent<SettingsPanelScript>().buffer["Rhythm: Level"][0]) / 100;
            }
            if (Settings.GetComponent<SettingsPanelScript>().buffer["Rhythm: Line Out"].Count > 0)
            {
                if(Settings.GetComponent<SettingsPanelScript>().buffer["Rhythm: Line Out"][0] == "OFF")
                {
                    GetComponent<AudioSource>().mute=true;
                }
                else
                {
                    GetComponent<AudioSource>().mute=false;
                }

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
        if(ok)
        {
            tapHit.SetActive(false);
            ok = false;
        }
        else
        {
            tapTempo.SetActive(false);
        }
    }
}
