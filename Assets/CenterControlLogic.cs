using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

public class CenterControlLogic : MonoBehaviour
{
    public GameObject allStartStop;
    public GameObject undoRedo;
    public GameObject tapTempo;
    public GameObject startStop;

    bool allStart = false;
    bool undo = false;
    bool tempo = false;
    bool startMetronom = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AllStart() {
        if (!allStart)
        {
            allStart = true;
            allStartStop.transform.GetChild(0).gameObject.SetActive(true);
            allStartStop.transform.GetChild(1).gameObject.SetActive(false);

        }
        else
        {
            allStart = false;
            allStartStop.transform.GetChild(0).gameObject.SetActive(false);
            allStartStop.transform.GetChild(1).gameObject.SetActive(true);

        }
    }
    public void UndoRedo()
    {
        if (!undo)
        {
            undo = true;
            undoRedo.transform.GetChild(0).gameObject.SetActive(true);
            undoRedo.transform.GetChild(1).gameObject.SetActive(false);

        }
        else
        {
            undo = false;
            undoRedo.transform.GetChild(0).gameObject.SetActive(false);
            undoRedo.transform.GetChild(1).gameObject.SetActive(true);

        }
    }
    public void TabTempo() {
        if (!tempo && !startMetronom)
        {
            tempo = true;
            tapTempo.GetComponent<ProgressBar>().Func_PlayUIAnim();

        }
        else
        {
            if (!startMetronom)
            {
                tempo = false;
                tapTempo.GetComponent<ProgressBar>().Func_StopUIAnim();
            }
        }
    }
    public void StartStop()
    {
        if (!startMetronom)
        {
            startMetronom = true;
            startStop.transform.GetChild(0).gameObject.SetActive(true);
            startStop.transform.GetChild(1).gameObject.SetActive(false);
            if (!tempo)
            {
                tapTempo.GetComponent<ProgressBar>().Func_PlayUIAnim();

            }

        }
        else
        {
            startMetronom = false;
            tempo = false;
            startStop.transform.GetChild(0).gameObject.SetActive(false);
            startStop.transform.GetChild(1).gameObject.SetActive(true);
            tapTempo.GetComponent<ProgressBar>().Func_StopUIAnim();

        }
    }
}
