using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;  // Jugador
    public float mouseSensitivity = 100f;  // Sensibilidad del ratón
    public float joystickSensitivity = 5f;  // Sensibilidad base del joystick
    public float minY = -40f;  // Límite inferior para la cámara
    public float maxY = 80f;   // Límite superior para la cámara
    private float rotationX = 0f;  // Rotación horizontal (Yaw)
    private float rotationY = 0f;  // Rotación vertical (Pitch)

    public bool invertMouseVerticalAxis = false;  // Opción para invertir el eje vertical del ratón
    public bool invertJoystickVerticalAxis = false;  // Opción para invertir el eje vertical del joystick

    public float joystickDeadZone = 0.1f;  // Rango mínimo para que el joystick mueva la cámara

    // Zoom variables
    public float minZoom = 3f;  // Distancia mínima de la cámara
    public float maxZoom = 10f; // Distancia máxima de la cámara
    public float zoomSpeed = 2f;  // Velocidad del zoom
    public float zoomSmoothTime = 0.1f;  // Tiempo de suavizado del zoom

    private float currentZoom; // Distancia inicial
    private float targetZoom;
    private float zoomVelocity;

    void Start()
    {
        targetZoom = maxZoom; // Inicia con el zoom máximo
        currentZoom = 10f; // Inicia con un valor predefinido
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
    void Update()
    {
        if (player == null) return;

        // Obtener las entradas del ratón o joystick
        float mouseX = Input.GetAxis("Mouse X");  // Entrada de ratón (X)
        float mouseY = Input.GetAxis("Mouse Y");  // Entrada de ratón (Y)
        float joystickX = Input.GetAxis("RightStickHorizontal");  // Joystick derecho (X)
        float joystickY = Input.GetAxis("RightStickVertical");  // Joystick derecho (Y)

        // Invertir el eje vertical del ratón si está activado
        if (invertMouseVerticalAxis)
        {
            mouseY = -mouseY;  // Invertir eje vertical del mouse
        }

        // Invertir el eje vertical del joystick si está activado
        if (invertJoystickVerticalAxis)
        {
            joystickY = -joystickY;  // Invertir eje vertical del joystick
        }

        // Deadzone: aplicar un umbral mínimo para que el movimiento sea reconocido
        if (Mathf.Abs(joystickX) < joystickDeadZone) joystickX = 0;
        if (Mathf.Abs(joystickY) < joystickDeadZone) joystickY = 0;

        // Escalar la sensibilidad del joystick de forma controlada
        float joystickXscaled = joystickX * (joystickSensitivity / 10f);  // Ajuste de sensibilidad
        float joystickYscaled = joystickY * (joystickSensitivity / 10f);  // Ajuste de sensibilidad

        // Combina la entrada del ratón y el joystick
        float horizontalInput = mouseX + joystickXscaled;  // Aplicar sensibilidad del joystick
        float verticalInput = mouseY + joystickYscaled;  // Aplicar sensibilidad del joystick

        // Actualizar las rotaciones acumuladas
        rotationX += horizontalInput * mouseSensitivity * Time.deltaTime;  // Sensibilidad del ratón
        rotationY -= verticalInput * mouseSensitivity * Time.deltaTime;  // Sensibilidad del ratón
        rotationY = Mathf.Clamp(rotationY, minY, maxY);  // Limitar la rotación vertical


        // Manejo del zoom con transición suave
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            targetZoom -= scrollInput * zoomSpeed;
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        }
        currentZoom = Mathf.SmoothDamp(currentZoom, targetZoom, ref zoomVelocity, zoomSmoothTime);

        // Ajustar altura cuando el zoom es mínimo
        float heightOffset = Mathf.Lerp(1.5f, 0f, (currentZoom - minZoom) / (maxZoom - minZoom));

        // Calcular posición final de la cámara
        Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0f);
        Vector3 cameraOffset = new Vector3(0, heightOffset, -currentZoom);
        transform.position = player.position + rotation * cameraOffset;

        transform.LookAt(player.position + Vector3.up * 1.5f); // Ajuste de altura para ver todo el cuerpo
    }
}
