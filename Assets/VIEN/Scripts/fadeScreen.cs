using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadeScreen : MonoBehaviour
{
    private CanvasGroup m_cg;
    public void FadeToBlack() {
        m_cg = GetComponent<CanvasGroup>();
        StartCoroutine(FadeOut());
    }
    public void FadeToInvisible()
    {
        m_cg = GetComponent<CanvasGroup>();
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeOut() {
        while (m_cg.alpha < 1) { //slowly set to 1
            m_cg.alpha += Time.deltaTime / 2.0f;
            yield return null;
        }
        m_cg.interactable = false;
        yield return null;
    }
    IEnumerator FadeIn()
    {
        while (m_cg.alpha > 0) { //slowly set to 0
            m_cg.alpha -= Time.deltaTime / 2.0f;
            yield return null;
        }
        m_cg.interactable = false;
        yield return null;
    }
}
