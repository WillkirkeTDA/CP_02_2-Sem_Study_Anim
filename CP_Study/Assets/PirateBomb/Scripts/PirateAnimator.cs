using UnityEngine;

public class PirateAnimator : MonoBehaviour
{
    private Animator animator;
    private PirateInput pirateInput;

    void Awake()
    {
        // Grabs the Animator and your PirateInput script
        animator = GetComponent<Animator>();
        pirateInput = GetComponent<PirateInput>();
    }

    void Update()
    {
        // Sends the current states from your input script directly to the Animator parameters
        animator.SetInteger("pJump", pirateInput.JumpValue());
        animator.SetInteger("pMove", pirateInput.MoveValueX());
        animator.SetBool("pGround", pirateInput.Grounded());
    }
}