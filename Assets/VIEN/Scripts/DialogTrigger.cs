using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Attached to each NPC
 * 
 */
public class DialogTrigger : MonoBehaviour
{
    const string DIALOG_TRIGGER_MSG = "Press [F] to interact";
    public CDialog m_dialog;

    public void TriggerDialog()
    {
        DialogManager dm = FindObjectOfType<DialogManager>();

        if (m_dialog.tradeableNPC == true) {
            gameObject.GetComponent<WitchCat>().CheckFishboneCount();
        }

        if (m_dialog.talkedTo == true)
            return;

        dm.StartDialog(m_dialog);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (m_dialog.talkedTo == false) {
                col.gameObject.SendMessage("DisplayMessage", DIALOG_TRIGGER_MSG);
            }
        }
    }
    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                TriggerDialog();
            }
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.SendMessage("ClearMessage");
        }
    }
}
