using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationManager : MonoBehaviour
{
    private Animator animator;
    private PlayerController playerMovement;


    // Animation parameteres 
    private int animSpeedID;
    private int animIDJump;
    private int animIDGrounded;
    private int animIDFreeFall;
    private int animIDIsThirdPerson;
    private int animIDVertical;
    private int animdIDHorizontal;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerController>();
    }

    private void Start()
    {
        AssignAnimationIDs();
        playerMovement.OnSpeedChangeAction += PlayerMovement_OnSpeedChangeAction;
        playerMovement.OnGroundStateChangeAction += PlayerMovement_OnGroundStateChangeAction;
        playerMovement.OnJumpAction += PlayerMovement_OnJumpAction;
        playerMovement.OnFreeFallAction += PlayerMovement_OnFreeFallAction;
        playerMovement.OnCameraChangeAction += PlayerMovement_OnCameraChangeAction;
        playerMovement.OnMoveDirectionAction += PlayerMovement_OnMoveDirectionAction;
    }

    private void PlayerMovement_OnMoveDirectionAction(Vector2 moveDirection, float currentSpeed)
    {
        animator.SetFloat(animIDVertical, moveDirection.y * currentSpeed * Time.deltaTime);
        animator.SetFloat(animdIDHorizontal, moveDirection.x * currentSpeed * Time.deltaTime);
    }

    private void PlayerMovement_OnCameraChangeAction(bool isThirdPerson)
    {
        animator.SetBool(animIDIsThirdPerson, isThirdPerson);
    }

    private void PlayerMovement_OnFreeFallAction(bool isFreeFall)
    {
        animator.SetBool(animIDFreeFall, isFreeFall);
    }

    private void PlayerMovement_OnJumpAction(bool isJumping)
    {
        animator.SetBool(animIDJump, isJumping);
    }

    private void PlayerMovement_OnGroundStateChangeAction(bool isGrounded)
    {
        animator.SetBool(animIDGrounded, isGrounded);
    }

    private void PlayerMovement_OnSpeedChangeAction(float speed)
    {
        animator.SetFloat(animSpeedID, speed);
    }
    private void AssignAnimationIDs()
    {
        animSpeedID = Animator.StringToHash("Speed");
        animIDJump = Animator.StringToHash("Jump");
        animIDGrounded = Animator.StringToHash("Grounded");
        animIDFreeFall = Animator.StringToHash("FreeFall");
        animIDIsThirdPerson = Animator.StringToHash("IsThirdPerson");
        animIDVertical = Animator.StringToHash("Vertical");
        animdIDHorizontal = Animator.StringToHash("Horizontal");

    }

}
