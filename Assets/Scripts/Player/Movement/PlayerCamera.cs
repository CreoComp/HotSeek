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
    //public float zoomSensitivity = 0.1f; 

    void Start()
    {
        // Начальные углы вращения
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
    }

    void LateUpdate()
    {
        // Получение ввода от мыши
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        yaw += mouseX;
        pitch -= mouseY;

        // Ограничение высоты вращения камеры
        pitch = Mathf.Clamp(pitch, minY, maxY);

        /* Обработка ввода колесика мыши для изменения расстояния
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        distance -= scrollDelta * zoomSensitivity; */

        /* Проверка минимального и максимального расстояния
        distance = Mathf.Clamp(distance, 1f, 20f); */

        // Вычисление новой позиции камеры
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y + y_Offset, target.position.z) - rotation * Vector3.forward * distance;

        // Применение сглаживания к позиции камеры
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Направление камеры на цель
        transform.LookAt(target);
    }
}
