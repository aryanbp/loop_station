using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicManagerScript : MonoBehaviour
{
    public GameObject controler;
    public bool on=false;
    public bool off=false;
    public bool looping=false;
    public List<Transform> children = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform c in controler.transform)
        { 
            children.Add(c);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Click() 
    {
        if (on)
        {
            if (looping)
            {
                children[2].gameObject.SetActive(false);
                children[1].gameObject.SetActive(true);
                looping = false; 

            }
            else
            {
                children[1].gameObject.SetActive(false);
                children[3].gameObject.SetActive(false);
                children[2].gameObject.SetActive(true);
                looping = true;
            }

        }
        else 
        {
            on= true;
            children[3].gameObject.SetActive(true);
        }
    }
    public void Pause() 
    {
        if(off && looping)
        {
            children[2].gameObject.SetActive(true);
            off = false;

        }
        else if(looping)
        {
            children[2].gameObject.SetActive(false);
            off = true;
        }
    }
}
