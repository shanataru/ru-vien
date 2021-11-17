using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrayCatTutorial : MonoBehaviour
{
    public headsUpDisplay hud;
    private DialogTrigger dt;
    void Start()
    {
        dt = GetComponent<DialogTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dt.m_dialog.talkedTo2 == true) {
            dt.m_dialog.m_NPCName = "Dev the Cat";
            hud.EnableCountInfo();
        }
    }
}
