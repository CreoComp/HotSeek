using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    [SerializeField] private float _gravity;
    [SerializeField] private float _jumpPower;


    public float speed = 12.0f;
    private Vector3 _velocity;
    private CharacterController _characterController;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Jump(Input.GetKey(KeyCode.Space) && _characterController.isGrounded);
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        DoGraviry(_characterController.isGrounded);
    }
    private void DoGraviry(bool isGrounded)
    {
        if (isGrounded && _velocity.y < 0)
        {
            _velocity.y = -1f;
        }
        _velocity.y -= _gravity * Time.fixedDeltaTime;
        _characterController.Move(_velocity * Time.fixedDeltaTime); 
    }

    private void Jump(bool canJump)
    {
        if(canJump)
        _velocity.y = _jumpPower;
    }
}
