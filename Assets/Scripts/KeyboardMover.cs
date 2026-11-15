using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = 9f;

    private PlayerInputActions input;
    private CharacterController cc;
    private float verticalVelocity;

    private void Awake()
    {
        input = new PlayerInputActions();
        cc = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        input.Player.Jump.performed += OnJump;
        input.Player.Enable();
    }

    private void OnDisable()
    {
        input.Player.Jump.performed -= OnJump;
        input.Player.Disable();
    }

    private void OnDestroy()
    {
        input.Dispose();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (cc.isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(2f * gravity * jumpHeight);
        }
    }

    private void Update()
    {
        if (cc.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        Vector2 moveInput = input.Player.Move.ReadValue<Vector2>();
        Vector3 horizontalMove = transform.right * moveInput.x
            + transform.forward * moveInput.y;
        horizontalMove = Vector3.ClampMagnitude(horizontalMove, 1f) * moveSpeed;

        verticalVelocity -= gravity * Time.deltaTime;

        Vector3 finalVelocity = horizontalMove + Vector3.up * verticalVelocity;
        cc.Move(finalVelocity * Time.deltaTime);
    }
}
