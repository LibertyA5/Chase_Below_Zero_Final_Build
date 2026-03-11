using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float mouseSensitivity = 250f;
    public float controllerSensitivity = 120f;

    public Transform orientation;

    float xRotation;
    float yRotation;

    float controllerDeadzone = 0.2f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float controllerX = Input.GetAxis("ControllerLookX");
        float controllerY = Input.GetAxis("ControllerLookY");

        if (Mathf.Abs(controllerX) < controllerDeadzone) controllerX = 0;
        if (Mathf.Abs(controllerY) < controllerDeadzone) controllerY = 0;

        float lookX =
            mouseX * mouseSensitivity * Time.deltaTime +
            controllerX * controllerSensitivity * Time.deltaTime;

        float lookY =
            mouseY * mouseSensitivity * Time.deltaTime +
            controllerY * controllerSensitivity * Time.deltaTime;

        yRotation += lookX;
        xRotation -= lookY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}