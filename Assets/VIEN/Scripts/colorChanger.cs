using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorChanger : MonoBehaviour
{
    const string COLOR_CHANGER_MSG = "Press [E] to change into displayed color\n";

    private Renderer m_renderer;
    private ParticleSystem m_paticles;
    private Animator m_animHower;

    private GameObject m_player;

    private Color m_startMaterialColor;

    void Start()
    {
        m_renderer = GetComponent<Renderer>();
        m_startMaterialColor = m_renderer.material.color;
        m_paticles = transform.GetChild(0).GetComponentInChildren<ParticleSystem>();
        m_animHower = GetComponent<Animator>();
        m_paticles.Stop();
        //m_animHower.enabled = false;
        m_player = GameObject.Find("Player");
    }

    void Update()
    {

    }
    private void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "Player")
        {
            m_paticles.Play();
            //m_animHower.enabled = true;
            col.gameObject.SendMessage("DisplayMessage", COLOR_CHANGER_MSG);
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            //col.gameObject.SendMessage("ColorChange", gameObject); //slow?
            m_player.GetComponent<character>().ColorChange(gameObject);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            m_paticles.Stop();
            //m_animHower.enabled = false;
            col.gameObject.SendMessage("ClearMessage");
        }
    }

    public void ResetColor() {
        m_renderer.material.color = new Color(m_startMaterialColor.r, m_startMaterialColor.g, m_startMaterialColor.b);
        tag = transform.parent.tag;
    }
}
