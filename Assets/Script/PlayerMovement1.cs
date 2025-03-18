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
        }

        // Usar Input.GetAxisRaw para el teclado
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        movementDirection = new Vector3(horizontal, 0, vertical).normalized;
        bool isMoving = movementDirection.magnitude >= 0.1f;

        anim.SetFloat("Speed", isMoving ? speed : 0f); // Responde de inmediato

        if (isMoving)
        {
            float targetAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float smoothedAngle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * speed;
            controller.Move(moveDirection * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump") && coyoteTimeCounter > 0)
        {
            verticalVelocity = jumpForce;
            anim.SetBool("IsJumping", true);
            coyoteTimeCounter = 0;
        }

        Vector3 jumpMovement = new Vector3(0, verticalVelocity, 0);
        controller.Move(jumpMovement * Time.deltaTime);
    }
}
