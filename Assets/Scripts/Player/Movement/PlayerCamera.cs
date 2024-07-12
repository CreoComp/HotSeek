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
    public float zoomSensitivity = 10f;
    public float y_OffsetTarget;

    void Start()
    {
        // ��������� ���� ��������
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
    }

    void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y + y_OffsetTarget, target.position.z);
        // ��������� ����� �� ����
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        yaw += mouseX;
        pitch -= mouseY;

        // ����������� ������ �������� ������
        pitch = Mathf.Clamp(pitch, minY, maxY);

        //��������� ����� �������� ���� ��� ��������� ����������
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        distance -= scrollDelta * zoomSensitivity;

        //�������� ������������ � ������������� ����������
        distance = Mathf.Clamp(distance, 1f, 20f);

        // ���������� ����� ������� ������
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = new Vector3(targetPosition.x, targetPosition.y + y_Offset, targetPosition.z) - rotation * Vector3.forward * distance;

        // Prevent camera from going underground
        desiredPosition.y = Mathf.Max(desiredPosition.y, targetPosition.y + 0.1f);

        // Raycast to check for obstacles
        RaycastHit hit;
        if (Physics.Raycast(targetPosition, desiredPosition - targetPosition, out hit, distance))
        {
            // If there's an obstacle, adjust the camera position to avoid it
            desiredPosition = targetPosition + (desiredPosition - targetPosition).normalized * (hit.distance - 0.1f);
        }

        // ���������� ����������� � ������� ������
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // ����������� ������ �� ����
        transform.LookAt(target);
    }
}
