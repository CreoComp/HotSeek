using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementDemo : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12.0f;

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);
    }
}
