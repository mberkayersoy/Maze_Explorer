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

    }

}
