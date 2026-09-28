using UnityEngine;

public class KnightAnimatoion : MonoBehaviour
{
    private Animator animator;
    private KnightInput KnightInput;

    void Awake()
    {
        // Grabs the Animator and your PirateInput script
        animator = GetComponent<Animator>();
        KnightInput = GetComponent<KnightInput>();
    }

    void Update()
    {
        // Sends the current states from your input script directly to the Animator parameters
        animator.SetInteger("pJump", KnightInput.JumpValue());
        animator.SetInteger("pMove", KnightInput.MoveValueX());
        animator.SetBool("pGround", KnightInput.Grounded());

    }
}