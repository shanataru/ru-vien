using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrayCatGarden : MonoBehaviour
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
        if (dt.m_dialog.talkedTo == true) {
            if (dt.m_dialog.karma == true)
            {
                
            }
        }

    }
}
