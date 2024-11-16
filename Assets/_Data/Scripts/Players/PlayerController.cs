using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private MobileJoystick joystick;

    private CharacterController characterController;
    private PlayerAnimator playerAnimator;

    [SerializeField] private float moveSpeed = 5;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<PlayerAnimator>();
    }

    private void Update()
    {
        ManageMovement();
    }

    private void ManageMovement()
    {
        Vector3 dirVector = joystick.GetMoveVector() * moveSpeed * Time.deltaTime / Screen.width;

        Vector3 moveVector = new Vector3(dirVector.x, 0, dirVector.y);

        characterController.Move(moveVector);

        playerAnimator.ManageAnimations(moveVector);
    }
}
