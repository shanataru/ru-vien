using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//behaviour for door and BRAZIER too ...
public class Door : MonoBehaviour
{
    private DialogTrigger dt;
    public headsUpDisplay hud;
    public GameObject m_brazier;
    string white = "white";

    void Start()
    {
        dt = GetComponent<DialogTrigger>();
        m_brazier.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        m_brazier.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
        m_brazier.transform.GetChild(2).GetComponent<ParticleSystem>().Stop();
        m_brazier.GetComponent<DialogTrigger>().m_dialog.talkedTo = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (hud.m_key == true)
        {
            dt.m_dialog.ResetDialog();
            dt.m_dialog.m_sentences = new string[] { "(Cats can apparently unlock doors now...)", "(You are thrilled about this new skill and quickly discard the absurdity of this fact.)" };
            dt.m_dialog.m_questions = new string[] { "(What will you do now?)" };
            dt.m_dialog.m_trueAnswers = new string[] { "Stay" };
            dt.m_dialog.m_falseAnswers = new string[] { "Go inside" };
            dt.m_dialog.m_trueReactions = new string[] { ". . ." };
            dt.m_dialog.m_falseReactions = new string[] { "(The key fits the keyhole perfectly.)" };
            dt.m_dialog.m_deathMessages = new string[] { "You have entered the black tower. It's dark and the wind feels cold against your fur. Your cat eyes slowly adjust to the complete darkness. You're ready to advance in your journey.\n\n <color=white> To be continued ..." };
            dt.m_dialog.m_proceedMessages = new string[] { "You have decided not to go yet." };
            hud.m_key = false;

            //enable brazier
            m_brazier.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            m_brazier.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
            m_brazier.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
            m_brazier.GetComponent<DialogTrigger>().m_dialog.talkedTo = false;
        }
    }
}
