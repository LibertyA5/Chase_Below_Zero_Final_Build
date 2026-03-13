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

    float controllerDeadzone = 0.1f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        Debug.Log("deltaTime: " + Time.deltaTime);
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        float controllerX = Input.GetAxis("ControllerLookX") * controllerSensitivity * Time.deltaTime;
        float controllerY = Input.GetAxis("ControllerLookY") * controllerSensitivity * Time.deltaTime;

        if (Mathf.Abs(controllerX) < controllerDeadzone) controllerX = 0;
        if (Mathf.Abs(controllerY) < controllerDeadzone) controllerY = 0;

        float lookX = mouseX + controllerX;
        float lookY = mouseY + controllerY;

        yRotation += lookX;
        xRotation -= lookY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        orientation.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }

}
