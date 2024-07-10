using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 5f; // ���������� �� ����
    public float smoothSpeed = 0.125f; // �������� ���������� ������

    public float minY = 1f; // ����������� ������ ������
    public float maxY = 10f; // ������������ ������ ������
    public float rotationSpeed = 5f; // �������� �������� ������

    private float pitch = 0f; // ���� �������� �� ��� X
    private float yaw = 0f; // ���� �������� �� ��� Y

    public float y_Offset;

    void Start()
    {
        // ��������� ���� ��������
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
    }

    void LateUpdate()
    {
        // ��������� ����� �� ����
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        yaw += mouseX;
        pitch -= mouseY;

        // ����������� ������ �������� ������
        pitch = Mathf.Clamp(pitch, minY, maxY);

        // ���������� ����� ������� ������
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y + y_Offset, target.position.z) - rotation * Vector3.forward * distance;

        // ���������� ����������� � ������� ������
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // ����������� ������ �� ����
        transform.LookAt(target);
    }
}
