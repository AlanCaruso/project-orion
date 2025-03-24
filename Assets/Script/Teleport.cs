using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Teleport : MonoBehaviour
{
    public Vector3 teleportPosition = new Vector3(0, 10, 0); // Coordenada de teletransporte
    public float yOffset = 2f; // Ajuste de altura
    public float fallThreshold = -10f; // Altura mínima antes de teletransportarse
    public float fallSpeedThreshold = -2f; // Velocidad mínima para considerar que está cayendo


    private Animator animator;
    private float lastVerticalVelocity; // Guardará la velocidad de caída antes del TP

    private PlayerMovement playerMovement; // Referencia a PlayerMovement
    private CharacterController controller;

    private void Start()
    {
 
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {

        if (transform.position.y < fallThreshold) // Si el jugador cae por debajo del umbral
        {
            TeleportPlayer();
        }
       
    }

    private void TeleportPlayer()
    {

        // Guardar la velocidad de caída actual
        float fallVelocity = playerMovement.GetVerticalVelocity();

        // Teletransportar al jugador
        Vector3 newPosition = teleportPosition + new Vector3(0, yOffset, 0);
        controller.enabled = false;  // Desactivar el CharacterController antes de cambiar la posición
        transform.position = newPosition;
        controller.enabled = true;   // Reactivar el CharacterController después

        // Restaurar la velocidad de caída
        playerMovement.SetVerticalVelocity(fallVelocity);

        // Determinar si sigue cayendo después del teletransporte
        bool isFalling = fallVelocity < playerMovement.fallSpeedThreshold;
        animator.SetBool("isFalling", isFalling);
    }
}
