using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MyGameManager : MonoBehaviour
{
    public bool intro;
    public GameObject m_player;
    public GameObject m_npcCatGarden;
    public GameObject m_npcCatTutorial;
    public GameObject m_startPlace;
    public GameObject m_HUD;


    public void Start()
    {
        Application.targetFrameRate = 60;

        if (intro)
            return;
        m_player.transform.GetChild(1).GetComponent<character>().TeleportPlayer(m_startPlace.transform.position);
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //m_player.transform.GetChild(1).GetComponent<character>().TeleportPlayer(m_startPlace.transform.position);
    }

    public void GoToIntroScene() {
        SceneManager.LoadScene("Intro");
    }

    public void StartGame() {
        SceneManager.LoadScene("fp_vien");
    }

    public void ExitGame() {
        Application.Quit();
    }


    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    ExitGame();
        //}

        if (intro)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            m_HUD.GetComponent<headsUpDisplay>().m_collected += 10;
        }

        if (m_npcCatGarden.activeInHierarchy) {
            if (m_npcCatGarden.GetComponent<DialogTrigger>().m_dialog.karma == true)
            {
                m_player.transform.GetChild(1).GetComponent<character>().TeleportPlayer(new Vector3(2.08f, -2.45f, -37.22f));
                //m_npcCatGarden.GetComponent<DialogTrigger>().m_dialog.karma = false;
                m_npcCatGarden.SetActive(false);
                m_npcCatTutorial.SetActive(true);
            }
        }

    }
}
