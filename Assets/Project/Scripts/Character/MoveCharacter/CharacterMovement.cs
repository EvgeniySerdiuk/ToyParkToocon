using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Transform rotationRoot;
    private InputPlayer player;
    private CharacterController characterController;
    private Vector3 moveDirection;
    private float gravity;
    private CharacterCharacteristics characterCharacteristics;
    private Animator animator;
    private float currentVelocity;
    private Characteristic moveSpeed;

    private void Awake()
    {
        player = new InputPlayer();
        characterController = GetComponent<CharacterController>();
        characterCharacteristics = GetComponent<CharacterCharacteristics>();
        animator = GetComponentInChildren<Animator>();
        moveSpeed = characterCharacteristics.Characteristics.GetCharacteristic(CharacteristicType.MoveSpeed);
    }

    private void Update()
    {
        ApplyGravity();
        RotateCharacter();
        CharacterAnimation();
        MoveCharacter();
    }

    private void OnEnable()
    {
        player.Enable();
        player.Mobile.Touch.performed += ReadInput;
        player.Mobile.Touch.canceled += Off;
    }

    private void OnDisable()
    {
        player.Mobile.Touch.performed -= ReadInput;
    }

    private void ReadInput(InputAction.CallbackContext context)
    {
        var value = player.Mobile.Touch.ReadValue<Vector2>();
        moveDirection = new Vector3(value.x, 0, value.y);
    }

    private void Off(InputAction.CallbackContext context)
    {
        moveDirection = Vector2.zero;
    }

    private void MoveCharacter()
    {
        Vector3 movement = moveDirection.normalized * moveSpeed.Value * Time.deltaTime;
        movement.y = gravity;
        characterController.Move(movement);
    }

    private void RotateCharacter()
    {
        if (moveDirection != Vector3.zero)
        {
            Quaternion rotate = Quaternion.LookRotation(moveDirection);
            rotationRoot.rotation = Quaternion.Lerp(rotationRoot.rotation, rotate, characterCharacteristics.RotationSpeed * Time.deltaTime);
        }
    }

    private void ApplyGravity()
    {
        if (!characterController.isGrounded)
        {
            gravity += Physics.gravity.y * Time.deltaTime;

            if (transform.position.y <= 0.05) 
            {
                gravity = 0f;
            }
        }
        else
        {
            gravity = 0f;
        }

        if(transform.position.y < 0.01) {transform.position = new Vector3(transform.position.x, 0.05f, transform.position.z);} //�������
    }

    private void CharacterAnimation()
    {
        if (moveDirection != Vector3.zero)
        {
            if (currentVelocity < 1)
            {
                currentVelocity += moveSpeed.Value * Time.deltaTime;
            }
        }
        else
        {
            if (currentVelocity > 0)
            {
                currentVelocity -= moveSpeed.Value * Time.deltaTime;
            }
        }

        animator.SetFloat("SpeedMultiplier", currentVelocity);
    }
}
