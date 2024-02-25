using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image m_Image;

    public Sprite[] m_SpriteArray;
    public float m_Speed = .04f;

    private int m_IndexSprite;
    private Coroutine m_CorotineAnim;
    private bool IsDone;

    public void Func_PlayUIAnim()
    {
        IsDone = false;
        StartCoroutine(Func_PlayAnimUI());
    }

    public void Func_StopUIAnim()
    {
        IsDone = true;
        if (m_CorotineAnim != null)
            StopCoroutine(m_CorotineAnim);
    }

    public void Func_RestartUIAnim()
    {
        Func_StopUIAnim();
        m_IndexSprite = 0;
        Func_PlayUIAnim();
    }

    IEnumerator Func_PlayAnimUI()
    {
        yield return new WaitForSeconds(m_Speed);
        if (m_IndexSprite >= m_SpriteArray.Length)
        {
            m_IndexSprite = 0;
        }
        m_Image.sprite = m_SpriteArray[m_IndexSprite];
        m_IndexSprite += 1;
        if (!IsDone)
            m_CorotineAnim = StartCoroutine(Func_PlayAnimUI());
    }
}
