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
    private float coyoteTimeCounter;
    private float coyoteTime = 0.2f;
    public float fallSpeedThreshold = -2f;

    public float GetVerticalVelocity()
    {
        return verticalVelocity;
    }

    public void SetVerticalVelocity(float velocity)
    {
        verticalVelocity = velocity;
    }

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded)
        {
            verticalVelocity = -0.5f;
            coyoteTimeCounter = coyoteTime;
            anim.SetBool("IsJumping", false);
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
            coyoteTimeCounter -= Time.deltaTime; // Reducir el tiempo en el aire

        }

        // Usar Input.GetAxisRaw para el teclado
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        movementDirection = new Vector3(horizontal, 0, vertical).normalized;
        bool isMoving = movementDirection.magnitude >= 0.1f;

        anim.SetFloat("Speed", isMoving ? speed : 0f); // Responde de inmediato

        if (!isGrounded)
        {
            Debug.Log("velocidad de caida" + verticalVelocity);
            anim.SetBool("isFalling", verticalVelocity < fallSpeedThreshold);

            bool isFalling = verticalVelocity < fallSpeedThreshold;

            Debug.Log("¿Está cayendo?: " + isFalling); // Verifica si la condición se cumple

            anim.SetBool("isFalling", isFalling);
        }
        else
        {
            verticalVelocity = -2f;
            anim.SetBool("isFalling", false);
        }


        if (isMoving)
        {
            float targetAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float smoothedAngle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);

            movementDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * speed;

        }

        if (Input.GetButtonDown("Jump") && coyoteTimeCounter > 0)
        {
            verticalVelocity = jumpForce;
            anim.SetBool("IsJumping", true);
            coyoteTimeCounter = 0;
        }

        // Movimiento final del personaje
        Vector3 totalMovement = movementDirection * speed + new Vector3(0, verticalVelocity, 0);
        controller.Move(totalMovement * Time.deltaTime);

    }
}
