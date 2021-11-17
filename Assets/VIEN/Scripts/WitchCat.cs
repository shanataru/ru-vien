using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using TMPro;

public class WitchCat : MonoBehaviour
{
    const int TRADE_AMOUNT = 10;
    public headsUpDisplay hud;
    public GameObject tradeButton;
    private DialogTrigger dt;

    void Start()
    {
        dt = GetComponent<DialogTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dt.m_dialog.karma == true)
        {
            dt.m_dialog.ResetDialog();
            dt.m_dialog.m_sentences = new string[] { ". . ." };
            dt.m_dialog.talkedTo = false;
        }
        if (dt.m_dialog.traded == true) {
            Traded();
            dt.m_dialog.traded = false;
        }
    }

    public void CheckFishboneCount() {
        if (hud.m_collected < TRADE_AMOUNT)
        {
            tradeButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            tradeButton.GetComponent<Button>().interactable = true;
        }
    }

    public void Traded() {
        //Debug.Log("Take fish, give key");
        hud.m_collected -= TRADE_AMOUNT;
        hud.m_key = true;
    }
}
