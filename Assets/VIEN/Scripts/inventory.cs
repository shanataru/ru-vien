using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventory : MonoBehaviour
{
    public AudioClip pickupSound;
    private int collected = 0;
    private int maxCollected;
    private GUIStyle style;

    // Start is called before the first frame update
    void Start()
    {
        maxCollected = GameObject.FindGameObjectsWithTag("Collectible").Length;
        style = new GUIStyle();
        style.normal.textColor = Color.black;
        style.fontSize = 25;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Pickup()
    {
        collected += 1;
        AudioSource.PlayClipAtPoint(pickupSound, transform.position);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Tear gems collected: " + collected.ToString() + "/" + maxCollected.ToString(), style );
    }
}
