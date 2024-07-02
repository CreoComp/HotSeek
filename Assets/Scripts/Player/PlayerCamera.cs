using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensivity = 2.0f;
    public float maxYAngle = 80f;

    private float rotationX = 0f;

    private void Update()
    {
        float mouseX = Input.GetAxis("MouseX");
        float mouseY = Input.GetAxis("MouseY");

        transform.parent.Rotate(Vector3.up * mouseX * sensivity);

        rotationX -= mouseY * sensivity;
        rotationX = Mathf.Clamp(rotationX, -maxYAngle, maxYAngle);

        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }
}
