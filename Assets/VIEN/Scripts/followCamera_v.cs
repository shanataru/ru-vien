using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 *  Source: https://www.youtube.com/watch?v=xcn7hz7J7sI
 */
public class followCamera_v : MonoBehaviour
{
    float horizontal;
    float vertical;
    public float rotationSmoothTime = .12f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 60.0f;

    public Transform player;
    public bool rotateAroundPlayer = true;
    public bool lookAtPlayer = false;
    public float rotateSpeed = 5.0f;

    [Range(0.01f, 1.0f)]
    public float smoothFactor = 0.15f;
    public float minCameraZoom = 5.0f;
    public float maxCameraZoom = 15.0f;

    private Vector3 cameraOffset;
    private float cameraZoom = 5.0f;

    private bool m_conversationState = false;

    void Start()
    {
        cameraOffset = (transform.position - player.position) * cameraZoom;
    }
    void LateUpdate()
    {
        if (m_conversationState == true) //disable camera when in conversation
            return;

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            cameraZoom -= 1.0f;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            cameraZoom += 1.0f;
        }

        cameraZoom = Mathf.Clamp(cameraZoom, minCameraZoom, maxCameraZoom);

        if (rotateAroundPlayer)
        {
            horizontal += Input.GetAxis("Mouse X") * rotateSpeed;
            vertical -= Input.GetAxis("Mouse Y") * rotateSpeed;
            vertical = Mathf.Clamp(vertical, Y_ANGLE_MIN, Y_ANGLE_MAX);

            currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(vertical, horizontal), ref rotationSmoothVelocity, rotationSmoothTime);
            transform.eulerAngles = currentRotation;

            transform.position = player.position - transform.forward * cameraZoom;
        }

        if (lookAtPlayer || rotateAroundPlayer)
        {
            transform.LookAt(player);
        }
    }

    void ToggleConversationState(bool s)
    {
        m_conversationState = s;
    }
}


