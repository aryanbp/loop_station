using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

public class CenterControlLogic : MonoBehaviour
{
    public GameObject allStartStop;
    public GameObject undoRedo;
    public GameObject tapTempo;
    public GameObject startStop;
    public GameObject loop0;
    public GameObject loop1;

    public bool allStart = false;
    public bool undo = false;
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
            loop0.GetComponent<LogicManagerScript>().Pause();
            loop0.GetComponent<Audio>().PausePlayRecording();
            loop1.GetComponent<LogicManagerScript>().Pause();
            loop1.GetComponent<Audio>().PausePlayRecording();
        }
        else
        {
            allStart = false;
            allStartStop.transform.GetChild(0).gameObject.SetActive(false);
            allStartStop.transform.GetChild(1).gameObject.SetActive(true);
            loop0.GetComponent<LogicManagerScript>().Pause();
            loop0.GetComponent<Audio>().PausePlayRecording();
            loop1.GetComponent<LogicManagerScript>().Pause();
            loop1.GetComponent<Audio>().PausePlayRecording();
        }
    }
    public void UndoRedo()
    {
        if (!undo && (loop0.GetComponent<LogicManagerScript>().looping || loop1.GetComponent<LogicManagerScript>().looping))
        {
            undo = true;
            undoRedo.transform.GetChild(0).gameObject.SetActive(true);
            undoRedo.transform.GetChild(1).gameObject.SetActive(false);
            loop0.GetComponent<LogicManagerScript>().undoBar.SetActive(true);
            loop0.GetComponent<LogicManagerScript>().undoBar.GetComponent<ProgressBar>().Func_PlayUIAnim();
            loop1.GetComponent<LogicManagerScript>().undoBar.SetActive(true);
            loop1.GetComponent<LogicManagerScript>().undoBar.GetComponent<ProgressBar>().Func_PlayUIAnim();

        }
        else
        {
            undo = false;
            undoRedo.transform.GetChild(0).gameObject.SetActive(false);
            undoRedo.transform.GetChild(1).gameObject.SetActive(true);
            loop0.GetComponent<LogicManagerScript>().undoBar.GetComponent<ProgressBar>().Func_StopUIAnim();
            loop0.GetComponent<LogicManagerScript>().undoBar.SetActive(false);
            loop0.GetComponent<LogicManagerScript>().undoOn=false;
            loop0.GetComponent<LogicManagerScript>().undoBar.GetComponent<ProgressBar>().m_SpriteArray[0] = loop0.GetComponent<LogicManagerScript>().unselect;

            loop1.GetComponent<LogicManagerScript>().undoBar.GetComponent<ProgressBar>().Func_StopUIAnim();
            loop1.GetComponent<LogicManagerScript>().undoBar.SetActive(false);
            loop1.GetComponent<LogicManagerScript>().undoOn = false;
            loop1.GetComponent<LogicManagerScript>().undoBar.GetComponent<ProgressBar>().m_SpriteArray[0] = loop1.GetComponent<LogicManagerScript>().unselect;
            GameObject[] audioObjects0 = GameObject.FindGameObjectsWithTag(loop0.GetComponent<Audio>().name_tag);
            GameObject[] audioObjects1 = GameObject.FindGameObjectsWithTag(loop1.GetComponent<Audio>().name_tag);
            
            if (audioObjects0.Length > 1)
            {
                if (audioObjects0[audioObjects0.Length - 1].GetComponent<AudioSource>().name == "Undo")
                {
                    loop0.GetComponent<Audio>().undoRecording();
                    Destroy(audioObjects0[audioObjects0.Length - 1]);
                }
            }
            if (audioObjects1.Length > 1)
            {
                if (audioObjects1[audioObjects1.Length - 1].GetComponent<AudioSource>().name == "Undo")
                {
                    loop1.GetComponent<Audio>().undoRecording();
                    Destroy(audioObjects1[audioObjects1.Length - 1]);
                }
            }
        }
    }
    public void TabTempo() {
        if (!tempo && !startMetronom)
        {
            tempo = true;
/*            tapTempo.GetComponent<ProgressBar>().Func_PlayUIAnim();
*/
        }
        else
        {
            if (!startMetronom)
            {
                tempo = false;
/*                tapTempo.GetComponent<ProgressBar>().Func_StopUIAnim();
*/            }
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
/*                tapTempo.GetComponent<ProgressBar>().Func_PlayUIAnim();
*/
            }
            else
            {
/*                tapTempo.GetComponent <ProgressBar>().Func_RestartUIAnim();
*/            }

        }
        else
        {
            startMetronom = false;
            tempo = false;
            startStop.transform.GetChild(0).gameObject.SetActive(false);
            startStop.transform.GetChild(1).gameObject.SetActive(true);
/*            tapTempo.GetComponent<ProgressBar>().Func_StopUIAnim();
*/
        }
    }
}
