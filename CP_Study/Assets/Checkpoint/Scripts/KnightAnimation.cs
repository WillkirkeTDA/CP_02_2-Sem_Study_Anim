using UnityEngine;

public class KnightAnimatoion : MonoBehaviour
{
    private Animator animator;
    private KnightInput KnightInput; // this gets the movement script of the player

    void Awake()
    {
        // Components
        animator = GetComponent<Animator>();
        KnightInput = GetComponent<KnightInput>();
    }

    void Update()
    {
        // Animations
        animator.SetInteger("pJump", KnightInput.JumpValue());
        animator.SetInteger("pMove", KnightInput.MoveValueX());
        animator.SetBool("pGround", KnightInput.Grounded());

    }
}