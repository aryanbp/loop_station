using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogicManagerScript : MonoBehaviour
{
    public GameObject controler;
    public TextMeshProUGUI Label;
    public bool on=false;
    public bool off=false;
    public bool looping=false;
    public List<Transform> children = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        //AudioSource audioSource = GetComponent<AudioSource>();
        //audioSource.clip = Microphone.Start(Microphone.devices[0], true, 10, 44100);
        //audioSource.Play();

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
        
    }
    public void reset()
    {
        if (looping)
        {
            children[1].gameObject.SetActive(false);
            children[2].gameObject.SetActive(false);
            children[3].gameObject.SetActive(false);
            on = false;
            off = false;
            looping = false;
            Label.text = "";
        }
}
    public void Click() 
    {

        if (on)
        {

            if (looping & off)
            {
                off = false;
                children[2].gameObject.SetActive(true);
                Label.text = "Looping";
            }
            else if (looping)
            {
                children[2].gameObject.SetActive(false);
                children[1].gameObject.SetActive(true);
                looping = false;
                Label.text = "Overdubbing";

            }
            else
            {
                children[1].gameObject.SetActive(!on); 
                children[3].gameObject.SetActive(false);
                children[2].gameObject.SetActive(true);
                Label.text = "Looping";
                looping = true;
            }

        }
        else 
        {
            on= true;
            children[3].gameObject.SetActive(true);
            Label.text = "Recording";
        }
    }
    public void Pause() 
    {
        if(off && looping)
        {
            children[2].gameObject.SetActive(true);
            off = false;
            Label.text = "Looping";

        }
        else if(looping)
        {
            children[2].gameObject.SetActive(false);
            off = true;
            Label.text = "Pause";
        }
    }
}
