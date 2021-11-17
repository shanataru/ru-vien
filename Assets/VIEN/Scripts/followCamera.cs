using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 *  Source: https://www.youtube.com/watch?v=xcn7hz7J7sI
 */
public class followCamera : MonoBehaviour
{
    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 50.0f;

    public Transform player;
    //public Transform pivot;
    public bool rotateAroundPlayer = true;
    public bool lookAtPlayer = false;
    public float rotateSpeed = 5.0f;

    [Range(0.01f, 1.0f)]
    public float smoothFactor = 0.15f;
    public float minCameraZoom = 1.0f;
    public float maxCameraZoom = 3.0f;

    private Vector3 cameraOffset;
    private float cameraZoom = 1.0f;

    //float horizontal;
    //float vertical;
    //public float rotationSmoothTime = .12f;
    //Vector3 rotationSmoothVelocity;
    //Vector3 currentRotation;

    private bool m_conversationState = false;
    void Start()
    {
        cameraOffset = (transform.position - player.position) * cameraZoom;

        //MOVEMENT B:
        //pivot.position = player.position;
        //pivot.rotation = player.rotation;
    }
    void LateUpdate()
    {
        if (m_conversationState == true) //disable camera when in conversation
            return;

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            cameraZoom -= 0.1f;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            cameraZoom += 0.1f;
        }

        cameraZoom = Mathf.Clamp(cameraZoom, minCameraZoom, maxCameraZoom);

        if (rotateAroundPlayer) {
            float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
            float vertical = Input.GetAxis("Mouse Y") * rotateSpeed;

            Quaternion camTurnAngleX = Quaternion.AngleAxis(horizontal, Vector3.up);
            cameraOffset = camTurnAngleX * cameraOffset;

            //horizontal += Input.GetAxis("Mouse X") * rotateSpeed;
            //vertical -= Input.GetAxis("Mouse Y") * rotateSpeed;
            //vertical = Mathf.Clamp(vertical, -40f, 85f);

            //currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(vertical, horizontal), ref rotationSmoothVelocity, rotationSmoothTime);
            //transform.eulerAngles = currentRotation;

            //transform.position = player.position - transform.forward * 2;
        }

        Vector3 newPosition = player.position + cameraOffset * cameraZoom;
        transform.position = Vector3.Slerp(transform.position, newPosition, smoothFactor);

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


