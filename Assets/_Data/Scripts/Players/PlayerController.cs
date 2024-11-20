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
        Vector3 correctDirVector = joystick.GetMoveVector() * moveSpeed * Time.deltaTime / Screen.width;
        correctDirVector.z = correctDirVector.y;
        correctDirVector.y = 0;
        playerAnimator.ManageAnimations(correctDirVector);

        Vector3 dirVector = joystick.GetMoveVector() * moveSpeed * Time.deltaTime / Screen.width;
        dirVector.z = dirVector.y;
        dirVector.y = -1;

        characterController.Move(dirVector);


    }
}
