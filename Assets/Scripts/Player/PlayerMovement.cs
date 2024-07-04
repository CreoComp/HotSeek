using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    [SerializeField] private float _gravity;
    [SerializeField] private float _jumpPower;

    // Добавляем переменные для управления скоростью
    public float baseSpeed = 6.0f; // Базовая скорость без учета Shift
    private float currentSpeed = 12.0f; // Текущая скорость с учетом Shift

    private Vector3 _velocity;
    private CharacterController _characterController;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Jump(Input.GetKey(KeyCode.Space) && _characterController.isGrounded);
        bool shiftPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // Изменяем скорость в зависимости от нажатия Shift
        currentSpeed = shiftPressed ? baseSpeed * 2 : baseSpeed;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * currentSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        DoGravity(_characterController.isGrounded);
    }

    private void DoGravity(bool isGrounded)
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
        if (canJump)
            _velocity.y = _jumpPower;
    }
}
