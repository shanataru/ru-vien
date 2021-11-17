using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * Feeds conversation text into canvas, disables player move, save player choices
 * 
 */
public class DialogManager : MonoBehaviour
{
    //for state editing - disable camera and player movement
    public GameObject playerSetup; 

    //for the dialog
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textDialog;
    public GameObject m_ChoiceButtons;
    public GameObject m_tradeButtons;

    public GameObject continueButton;

    //animation and fading of the dialog
    public Animator anim;
    public Canvas canvasFade;
    public Canvas canvasFadeUI;

    public AudioSource ambient;
    public AudioSource game_music;

    //misc
    private CDialog m_NPCDialog;
    private bool m_playerAnswered = false;

    private GameObject m_player;
    private GameObject m_followCamera;

    //from CDialog
    private Queue<string> m_sentences;
    private Queue<string> m_questions;
    private Queue<string> m_trueAnswers;
    private Queue<string> m_falseAnswers;
    private Queue<string> m_trueReactions;
    private Queue<string> m_falseReactions;
    private Queue<string> m_deathMessages;
    private Queue<string> m_proceedMessages;

    private Queue<string> m_tradeQuestions;

    void Start() {
        // fill in player data
        m_player = playerSetup.transform.Find("Player").gameObject;
        m_followCamera = playerSetup.transform.Find("FollowCamera").gameObject;

        //fill up queues from CDialog class
        m_sentences = new Queue<string>();
        m_questions = new Queue<string>();
        m_trueAnswers = new Queue<string>();
        m_falseAnswers = new Queue<string>();
        m_trueReactions = new Queue<string>();
        m_falseReactions = new Queue<string>();
        m_deathMessages = new Queue<string>();
        m_proceedMessages = new Queue<string>();

        m_tradeQuestions = new Queue<string>();
    }

    public void StartDialog(CDialog d) {
        m_sentences.Clear();
        m_questions.Clear();
        m_trueAnswers.Clear();
        m_falseAnswers.Clear();
        m_trueReactions.Clear();
        m_falseReactions.Clear();
        m_deathMessages.Clear();
        m_proceedMessages.Clear();
        m_tradeQuestions.Clear();

        m_player.SendMessage("ToggleConversationState", true);
        m_followCamera.SendMessage("ToggleConversationState", true);
        anim.SetBool("isOpen", true);
        m_NPCDialog = d;
        textName.text = d.m_NPCName;

        //enqueue sentences
        foreach (string s in d.m_sentences)
        {
            m_sentences.Enqueue(s);
        }

        //enqueue questions?
        foreach (string q in d.m_questions)
        {
            m_questions.Enqueue(q);
        }

        //enqueue player answer possibilities
        foreach (string ga in d.m_trueAnswers)
        {
            m_trueAnswers.Enqueue(ga);
        }

        foreach (string ba in d.m_falseAnswers)
        {
            m_falseAnswers.Enqueue(ba);
        }

        //enqueue possible NPC reaction
        foreach (string tr in d.m_trueReactions)
        {
            m_trueReactions.Enqueue(tr);
        }

        foreach (string fr in d.m_falseReactions)
        {
            m_falseReactions.Enqueue(fr);
        }

        foreach (string dm in d.m_deathMessages)
        {
            m_deathMessages.Enqueue(dm);
        }

        foreach (string pm in d.m_proceedMessages)
        {
            m_proceedMessages.Enqueue(pm);
        }


        foreach (string tq in d.m_tradeQuestions)
        {
            m_tradeQuestions.Enqueue(tq);
        }

        //start by normal conversation
        continueButton.SetActive(true);
        m_ChoiceButtons.gameObject.SetActive(false);
        m_tradeButtons.gameObject.SetActive(false);

        StartCoroutine(WaitForDialogAnimation(1));
    }

    IEnumerator WaitForDialogAnimation(float t) {
        yield return new WaitForSecondsRealtime(t);
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        //nothing to say
        if (m_sentences.Count == 0) 
        {
            if (m_NPCDialog.tradeableNPC == false)
            {
                //nothing to ask
                if (m_questions.Count == 0)
                {
                    EndDialog();
                    return;
                }
                //start asking life-decision questions
                DisplayNextQuestion();
                return;
            }
            else {
                //nothing to trade
                if (m_tradeQuestions.Count == 0)
                {
                    EndDialog();
                    return;
                }
                DisplayNextTradeQuestion();
                return;
            }
        }

        string s = m_sentences.Dequeue();
        StopAllCoroutines(); //Stop if user press continue
        StartCoroutine(TypeSentence(s, textDialog));
    }

    void EndDialog() {
        anim.SetBool("isOpen", false);
        m_player.SendMessage("ToggleConversationState", false);
        m_followCamera.SendMessage("ToggleConversationState", false);
        textDialog.text = "";
        m_NPCDialog.talkedTo2 = true;
    }

    void DisplayNextQuestion() {
        if (m_questions.Count == 0) {
            EndDialog();
            return;
        }

        string q = m_questions.Dequeue();
        
        //for every question there should be a choice for the player!
        string ga = m_trueAnswers.Dequeue();
        string ba = m_falseAnswers.Dequeue();

        //set buttons for player answer
        m_ChoiceButtons.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = ga;
        m_ChoiceButtons.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = ba;

        m_ChoiceButtons.gameObject.SetActive(true);
        continueButton.SetActive(false);

        StopAllCoroutines(); //Stop if user press continue
        StartCoroutine(TypeSentence(q, textDialog));

        //display until player answers
        StartCoroutine(WaitForPlayerAnswer());
    }

    public void PlayerChoseTrue() {
        m_playerAnswered = true;
        //continueButton.SetActive(true);

        string tr = m_trueReactions.Dequeue();
        m_falseReactions.Dequeue();

        m_ChoiceButtons.gameObject.SetActive(false);

        StopAllCoroutines(); //Stop if user press continue
        StartCoroutine(TypeSentence(tr, textDialog, true));
        StartCoroutine(DisplayProceedMessage());
    }
    public void PlayerChoseFalse()
    {
        m_playerAnswered = true;

        string fr = m_falseReactions.Dequeue();
        m_trueReactions.Dequeue();

        m_ChoiceButtons.gameObject.SetActive(false);

        StopAllCoroutines();
        StartCoroutine(TypeSentence(fr, textDialog, true));
        StartCoroutine(DisplayRestartScreen());
    }

    void DisplayNextTradeQuestion()
    {
        if (m_tradeQuestions.Count == 0)
        {
            EndDialog();
            return;
        }

        string tq = m_tradeQuestions.Dequeue();

        //for every question there should be a choice for the player!
        string ga = m_trueAnswers.Dequeue();
        string ba = m_falseAnswers.Dequeue();

        //set buttons for player answer
        m_tradeButtons.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = ga;
        m_tradeButtons.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = ba;

        m_tradeButtons.gameObject.SetActive(true);
        continueButton.SetActive(false);

        StopAllCoroutines(); //Stop if user press continue
        StartCoroutine(TypeSentence(tq, textDialog));

        //display until player answers
        StartCoroutine(WaitForPlayerAnswer());
    }

    public void PlayerTrade()
    {
        m_playerAnswered = true;
        m_NPCDialog.karma = true;
        m_NPCDialog.traded = true;

        string tr = m_trueReactions.Dequeue();
        m_falseReactions.Dequeue();

        m_tradeButtons.gameObject.SetActive(false);

        StopAllCoroutines(); //Stop if user press continue
        StartCoroutine(TypeSentence(tr, textDialog, true));
        StartCoroutine(EndDialogWithWait(3.0f));
    }

    public void PlayerNoTrade()
    {
        m_playerAnswered = true;
        string fr = m_falseReactions.Dequeue();
        m_trueReactions.Dequeue();

        m_tradeButtons.gameObject.SetActive(false);

        StopAllCoroutines(); //Stop if user press continue
        StartCoroutine(TypeSentence(fr, textDialog, true));
        StartCoroutine(EndDialogWithWait(0.7f));
    }

    IEnumerator TypeSentence(string sentence, TextMeshProUGUI textWindow, bool wait = false) {
        textWindow.text = "";
        foreach (char c in sentence.ToCharArray()) {
            textWindow.text += c;
            //yield return new WaitForSeconds(typingSpeed);
            yield return null;
        }
        if (wait) {
            yield return new WaitForSecondsRealtime(5);
        }
    }

    IEnumerator EndDialogWithWait(float t) {
        yield return new WaitForSecondsRealtime(t);
        EndDialog();
    }

    IEnumerator WaitForPlayerAnswer()
    {
        if (m_playerAnswered == false)
        {
            yield return null;
        }
    }

    //either fade to chnage scene or fade to restart game
    IEnumerator CutsceneFadeToBlack(float t) {
        canvasFade.gameObject.SetActive(true);
        canvasFade.GetComponent<fadeScreen>().FadeToBlack();
        yield return new WaitForSecondsRealtime(t);
    }

    IEnumerator CutsceneFadeToInvisible(float t)
    {
        canvasFade.gameObject.SetActive(true);
        canvasFade.GetComponent<fadeScreen>().FadeToInvisible();
        yield return new WaitForSecondsRealtime(t);
        canvasFade.gameObject.SetActive(false);
    }

    IEnumerator DisplayProceedMessage() {
        yield return new WaitForSecondsRealtime(1);
        yield return StartCoroutine(CutsceneFadeToBlack(2));
        canvasFadeUI.gameObject.SetActive(true); //enable canvas
        canvasFadeUI.transform.GetChild(1).gameObject.SetActive(false); //disable restart button
        TextMeshProUGUI text = canvasFadeUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>(); //write on fade screen info
        string pm = m_proceedMessages.Dequeue();
        StartCoroutine(TypeSentence(pm, text, true));
        m_NPCDialog.karma = true;
        EndDialog();
        yield return new WaitForSecondsRealtime(2);
        canvasFadeUI.gameObject.SetActive(false);
        StartCoroutine(CutsceneFadeToInvisible(2));
    }

    IEnumerator DisplayRestartScreen() {
        yield return new WaitForSecondsRealtime(1);
        yield return StartCoroutine(CutsceneFadeToBlack(2));
        TextMeshProUGUI text = canvasFadeUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        string dm = m_deathMessages.Dequeue();
        StartCoroutine(TypeSentence(dm, text, true));
        canvasFadeUI.gameObject.SetActive(true);
        canvasFadeUI.transform.GetChild(1).gameObject.SetActive(true); //enable restart button
        //m_player.GetComponent<AudioListener>().enabled = false;
        //ambient.volume = Mathf.Lerp(0.5f, 0.0f, Time.time);
        game_music.volume = Mathf.Lerp(0.02f, 0.0f, Time.time);

    }
}
