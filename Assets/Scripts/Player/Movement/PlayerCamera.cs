using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 5f; // Расстояние от цели
    public float smoothSpeed = 0.125f; // Скорость следования камеры

    public float minY = 1f; // Минимальная высота камеры
    public float maxY = 10f; // Максимальная высота камеры
    public float rotationSpeed = 5f; // Скорость вращения камеры

    private float pitch = 0f; // Угол вращения по оси X
    private float yaw = 0f; // Угол вращения по оси Y

    public float y_Offset;
    public float zoomSensitivity = 10f;
    public float y_OffsetTarget;

    void Start()
    {
        // Начальные углы вращения
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
    }

    void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y + y_OffsetTarget, target.position.z);
        // Получение ввода от мыши
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        yaw += mouseX;
        pitch -= mouseY;

        // Ограничение высоты вращения камеры
        pitch = Mathf.Clamp(pitch, minY, maxY);

        //Обработка ввода колесика мыши для изменения расстояния
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        distance -= scrollDelta * zoomSensitivity;

        //Проверка минимального и максимального расстояния
        distance = Mathf.Clamp(distance, 1f, 20f);

        // Вычисление новой позиции камеры
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

        // Применение сглаживания к позиции камеры
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Направление камеры на цель
        transform.LookAt(target);
    }
}
