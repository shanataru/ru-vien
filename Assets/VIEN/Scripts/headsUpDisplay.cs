using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class headsUpDisplay : MonoBehaviour
{
    public TextMeshProUGUI m_systemInfo;
    public TextMeshProUGUI m_fishbonesCollectedMsg;
    public TextMeshProUGUI m_lifesMsg;
    public GameObject m_panelCount; 
    public GameObject m_panelInventory;
    public string m_messages;
    public GameObject m_KeyIcon;
    public GameObject m_HoneyIcon;
    public GameObject m_icon3;
    public GameObject m_icon4;

    public bool m_displayLifes = false;
    public GameObject m_lifeIcon;
    public GameObject m_lifeCount;

    public int m_collected = 0;
    public bool m_key = false;
    //private int m_maxToCollect;
    private int m_falls = 9;


    // Start is called before the first frame update
    void Start()
    {
        m_systemInfo.text = "";
        m_messages = "";
        //m_maxToCollect = GameObject.FindGameObjectsWithTag("Collectible").Length;
        m_KeyIcon.GetComponent<Image>().enabled = false;
        m_HoneyIcon.GetComponent<Image>().enabled = false;
        m_icon3.GetComponent<Image>().enabled = false;
        m_icon4.GetComponent<Image>().enabled = false;
        if (!m_displayLifes) {
            m_lifeIcon.GetComponent<Image>().enabled = false;
            m_lifeCount.GetComponent<TextMeshProUGUI>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_systemInfo.text = m_messages;
        m_fishbonesCollectedMsg.text = "x" + m_collected.ToString();
        m_lifesMsg.text = "x" + m_falls.ToString();

        if (m_key == true) {
            m_KeyIcon.GetComponent<Image>().enabled = true;
            //false is set by door after updating dialog....
        }
    }

    public void Collect()
    {
        m_collected += 1;
    }

    public void Death() {
        m_falls -= 1;
    }

    public void EnableCountInfo() {
        m_panelCount.SetActive(true);
        m_panelInventory.SetActive(true);
    }
}
