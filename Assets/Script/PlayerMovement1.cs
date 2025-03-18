using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator anim;
    private CharacterController controller;

    public float speed = 5f;
    public float rotationSpeed = 10f;
    public float jumpForce = 5f;
    public float gravity = 9.81f;

    public Transform cameraTransform;

    private Vector3 movementDirection;
    private float verticalVelocity;
    private bool isGrounded;
    private bool isJumping = false; // Solo se activa cuando el salto empieza
    private bool isJumpingIdle = false; // Para manejar el salto sin moverse
    private float coyoteTimeCounter;
    private float coyoteTime = 0.2f; // Tiempo de gracia para saltos
    private float timeInAir = 0f;  // Para controlar cuanto tiempo esta en el aire

    void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Verificar si el personaje esta tocando el suelo
        isGrounded = controller.isGrounded;

        if (isGrounded)
        {
            // Si estaba en un estado de salto, desactivar las animaciones correspondientes al aterrizar
            if (isJumping || isJumpingIdle)
            {
                anim.SetBool("IsJumping", false);
                anim.SetBool("IsJumpingIdle", false);
                isJumping = false;
                isJumpingIdle = false;
            }

            verticalVelocity = -0.5f; // Mantener al personaje en el suelo
            coyoteTimeCounter = coyoteTime; // Reiniciar coyote time
        }
        else
        {
            // Si esta en el aire, aplicar la gravedad y aumentar el tiempo en el aire
            verticalVelocity -= gravity * Time.deltaTime;
            timeInAir += Time.deltaTime;
        }

        // Obtener entrada de movimiento (WASD o joystick)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Crear un vector de direccion con base en la entrada del jugador
        movementDirection = new Vector3(horizontal, 0, vertical).normalized;

        // Si el jugador esta intentando moverse
        if (movementDirection.magnitude >= 0.1f)
        {
            // Calcular el angulo de rotacion basado en la direccion de la camara
            float targetAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float smoothedAngle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);

            // Mover al personaje correctamente usando CharacterController
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * speed;
            controller.Move(moveDirection * Time.deltaTime);

            anim.SetBool("IsRunning", true);

        }
        else
        {
            anim.SetBool("IsRunning", false);
        }

        // Manejo del salto en idle
        if (!isGrounded && movementDirection.magnitude == 0f && isJumping)
        {
            anim.SetBool("IsJumpingIdle", true); // Activar animacion de salto sin moverse
            isJumpingIdle = true;
        }
        // Si el jugador estaba en un salto idle pero empieza a moverse en el aire, cambiar a la animacion de salto normal
        else if (movementDirection.magnitude > 0f && isJumpingIdle)
        {
            anim.SetBool("IsJumpingIdle", false);
            anim.SetBool("IsJumping", true);
            isJumpingIdle = false;
        }

        // Saltar con control de coyote time
        if (Input.GetButtonDown("Jump") && coyoteTimeCounter > 0 && !isJumping)
        {
            verticalVelocity = jumpForce;

            // Determinar si el salto es desde idle o en movimiento
            if (movementDirection.magnitude == 0f)
            {
                anim.SetBool("IsJumpingIdle", true);
                isJumpingIdle = true;
            }
            else
            {
                anim.SetBool("IsJumping", true);
                isJumping = true;
            }

            coyoteTimeCounter = 0; // Evitar saltos infinitos
        }

        // Si el personaje aterriza y no este moviendose, reiniciar el estado de salto
        if (isGrounded && !isJumping && !isJumpingIdle && movementDirection.magnitude == 0f)
        {
            anim.SetBool("IsJumping", false);
            anim.SetBool("IsJumpingIdle", false);
        }

        // Si el jugador estaba saltando y ahora esta corriendo, dejar de estar en el estado de salto
        if (isGrounded && movementDirection.magnitude > 0f && !isJumping && !isJumpingIdle)
        {
            anim.SetBool("IsJumping", false);
            anim.SetBool("IsJumpingIdle", false);
        }

        // Aplicar gravedad y salto usando CharacterController
        Vector3 jumpMovement = new Vector3(0, verticalVelocity, 0);
        controller.Move(jumpMovement * Time.deltaTime);
    }
}