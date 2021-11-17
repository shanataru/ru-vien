using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character : MonoBehaviour
{

    public int m_playerColorID = 0;
    public string m_playerColorTag = "Grey";
    public GameObject allColorChangers;
    public Camera followCamera;

    public AudioClip changeColorSound;
    public AudioClip jumpSound;
    public AudioClip collectSound;

    public GameObject m_respawnPoint1;
    //public Transform pivot;

    private float movementSpeed = 6.0f;
    private float rotateSpeed = 120.0f;
    private float jumpSpeed = 10.0f;
    private float gravity = 20.0f;
    private float smoothFactor = 0.15f;
    private enum AllColors { Grey, Blue, Yellow, Green, Purple };
    private Vector3 m_moveDirection = new Vector3(0, 0, 0);
    private CharacterController m_controller;
    public GameObject m_HUD;
    private Renderer m_playerRenderer;
    private Color m_playerStartMaterialColor;
    private AudioSource m_audiosrc;

    private ParticleSystem m_footprints;
    private bool m_conversationState = false;


    void Start(){
        m_controller = GetComponent<CharacterController>();
        //m_HUD = GameObject.Find("HUDisp");
        //m_playerRenderer = transform.GetChild(0).gameObject.GetComponent<Renderer>();
        m_playerRenderer = transform.GetChild(0).transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        m_playerStartMaterialColor = m_playerRenderer.material.color;
        m_audiosrc = GetComponent<AudioSource>();
        m_footprints = transform.GetChild(4).GetComponentInChildren<ParticleSystem>();
    }

    void PlayerMove() {

        /*
        * 
        *  Source: https://www.youtube.com/watch?v=qwHJgYnoxEY&list=PLboXykqtm8dxhCV5SVn0N76jrdCo5HV2a&index=110
        */

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //disable movements while in conversation
        if (m_conversationState == true)
            return;

        transform.Rotate(0, horizontal * rotateSpeed * Time.deltaTime, 0);

        //MOVEMENT B
        //if (Input.GetKey(KeyCode.W))
        //{
        //    transform.rotation = Quaternion.Slerp(transform.rotation, pivot.rotation, smoothFactor);
        //}

        if (m_controller.isGrounded)
        {

            m_moveDirection = Vector3.forward * vertical;

            //MOVEMENT B
            //moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            m_moveDirection = transform.TransformDirection(m_moveDirection);
            m_moveDirection *= movementSpeed;

            if (Input.GetButton("Jump"))
            {
                m_moveDirection.y = jumpSpeed;
                m_audiosrc.PlayOneShot(jumpSound, 0.1f);
                m_footprints.Pause();
                m_footprints.Clear();
            }

        }
        else
        {
            m_footprints.Play();
            m_moveDirection = new Vector3(Input.GetAxis("Horizontal"), m_moveDirection.y, Input.GetAxis("Vertical"));
            m_moveDirection = transform.TransformDirection(m_moveDirection);
            m_moveDirection.x *= movementSpeed;
            m_moveDirection.z *= movementSpeed;
        }
        m_moveDirection.y -= gravity * Time.deltaTime;
        m_controller.Move(m_moveDirection * Time.deltaTime);


        /*
        * 
        *  Source: https://www.youtube.com/watch?v=GWbMQQAliVw
        */
        /*if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            //moveDirection = transform.TransformDirection(moveDirection);
            if (moveDirection != Vector3.zero) {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), smoothFactor);
            }

            moveDirection *= speed;
            if (Input.GetButton("Jump")){
                moveDirection.y = jumpSpeed;
            }
        }
        else {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), moveDirection.y, Input.GetAxis("Vertical"));

            //moveDirection = transform.TransformDirection(moveDirection);
            if (moveDirection != Vector3.zero) {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(moveDirection.x, 0.0f, moveDirection.z)), 0.15F);
            }

            moveDirection.x *= speed;
            moveDirection.z *= speed;
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
        */
    }

    public void ColorChange(GameObject colorChanger)
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            Renderer colorChangerRenderer = colorChanger.GetComponent<Renderer>();

            //switch color code
            switch (colorChanger.tag) {
                case "Grey":
                    m_playerColorID = (int)AllColors.Grey;
                    break;
                case "Blue":
                    m_playerColorID = (int)AllColors.Blue;
                    break;
                case "Pink":
                    m_playerColorID = (int)AllColors.Yellow;
                    break;
                case "Green":
                    m_playerColorID = (int)AllColors.Green;
                    break;
                case "Purple":
                    m_playerColorID = (int)AllColors.Purple;
                    break;
                default:
                    break;
            }

            //switch tag with colorChanger
            string tmpTag = m_playerColorTag;
            m_playerColorTag = colorChanger.tag;
            colorChanger.tag = tmpTag;

            //switch color with colorChanger material
            Color playerMaterialColor = m_playerRenderer.material.color;
            Color colorChangerMaterialColor = colorChangerRenderer.material.color;
            m_playerRenderer.material.color = new Color(colorChangerMaterialColor.r, colorChangerMaterialColor.g, colorChangerMaterialColor.b);
            colorChangerRenderer.material.color = new Color(playerMaterialColor.r, playerMaterialColor.g, playerMaterialColor.b);

            //play a sound
            m_audiosrc.PlayOneShot(changeColorSound, 0.2f);

        }
    }

    void DisplayMessage(string text) {
        m_HUD.GetComponent<headsUpDisplay>().m_messages = text;     
    }

    void ClearMessage() {
        m_HUD.GetComponent<headsUpDisplay>().m_messages = "";
    }

    void Collect() {
        m_HUD.GetComponent<headsUpDisplay>().Collect();
        m_audiosrc.PlayOneShot(collectSound, 0.1f);
    }

    void Respawn() {
        //count death
        m_HUD.GetComponent<headsUpDisplay>().Death();

        //reset player
        m_playerColorID = 0;
        m_playerColorTag = "Grey";
        m_playerRenderer.material.color = new Color(m_playerStartMaterialColor.r, m_playerStartMaterialColor.g, m_playerStartMaterialColor.b);
        //m_controller.enabled = false;
        //transform.position = m_respawnPoint1.transform.position;
        //m_controller.enabled = true;
        TeleportPlayer(new Vector3(m_respawnPoint1.transform.position.x, m_respawnPoint1.transform.position.y, m_respawnPoint1.transform.position.z));

        //reset colorChangers
        foreach (Transform colorChanger in allColorChangers.transform) {
            colorChanger.GetChild(0).GetComponent<colorChanger>().ResetColor();
        }
    }

    public void TeleportPlayer(Vector3 newPosition) {
        m_controller.enabled = false;
        transform.position = newPosition;
        m_controller.enabled = true;
        ClearMessage();
    }

    void ToggleConversationState(bool s) {
        m_conversationState = s;
    }

    void Update()
    {
        PlayerMove();
    }
}
