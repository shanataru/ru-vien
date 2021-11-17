using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platform : MonoBehaviour
{
    private Collider m_collider;
    public GameObject player;
    public Material m_opaque;
    public Material m_transparent;
    private string m_tag;
    private Color m_materialColor;

    void Start()
    {
        m_collider = GetComponent<Collider>();
        player = GameObject.FindWithTag("Player");
        m_tag = gameObject.tag;
        m_materialColor = gameObject.GetComponent<Renderer>().material.color;
    }

    void Update()
    {
        if (m_tag == player.GetComponent<character>().m_playerColorTag) //script not attached to grey platforms
        {
            m_collider.enabled = true;
            //m_materialColor.a = 1.0f;
            gameObject.GetComponent<Renderer>().material = m_opaque;
        }
        else {
            m_collider.enabled = false;
            //m_materialColor.a = 0.5f;
            gameObject.GetComponent<Renderer>().material = m_transparent;
        }
    }
}
