using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CDialog 
{
    public string m_NPCName;
    public bool karma; //karma = true (good) or false (bad) answer; helps to proceed with the "story"
    public bool talkedTo = false; //broken, used for brazier...
    public bool talkedTo2 = false;
    public bool tradeableNPC;
    public bool traded = false;

    [TextArea(3,10)]
    public string[] m_sentences;

    //---------------------------------------------------------------------------------------------

    [TextArea(3, 10)]
    public string[] m_questions;

    [TextArea(3, 10)]
    public string[] m_trueAnswers;

    [TextArea(3, 10)]
    public string[] m_falseAnswers;

    [TextArea(3, 10)]
    public string[] m_trueReactions;

    [TextArea(3, 10)]
    public string[] m_falseReactions;

    [TextArea(3, 10)]
    public string[] m_deathMessages;

    [TextArea(3, 10)]
    public string[] m_proceedMessages;

    //---------------------------------------------------------------------------------------------

    [TextArea(3, 10)]
    public string[] m_tradeQuestions;

    //---------------------------------------------------------------------------------------------

    public void ResetDialog() {
        m_sentences = new string[0];
        m_questions = new string[0]; 
        m_trueAnswers = new string[0];
        m_falseAnswers = new string[0];
        m_trueReactions = new string[0];
        m_falseReactions = new string[0];
        m_deathMessages = new string[0];
        m_proceedMessages = new string[0];
        m_tradeQuestions = new string[0];
    }
}
