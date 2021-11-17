using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 *  Source: https://code.tutsplus.com/tutorials/unity3d-third-person-cameras--mobile-11230
 */
public class mouseCamera : MonoBehaviour
{
    public GameObject target;
    public float rotateSpeed = 5;
    Vector3 cameraOffset;

    void Start()
    {
        cameraOffset = target.transform.position - transform.position;
    }
    void LateUpdate()
    {
        float horizontal = Input.GetAxis("Mouse X");
        float vertical = Input.GetAxis("Mouse Y") * rotateSpeed;
        target.transform.Rotate(0, horizontal * rotateSpeed, 0);

        float targetAngleY = target.transform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(Input.GetAxis("Mouse Y"), targetAngleY, 0);
        transform.position = target.transform.position - (rotation * cameraOffset);

        transform.LookAt(target.transform);
    }
}
