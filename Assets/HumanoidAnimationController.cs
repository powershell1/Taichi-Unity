using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HumanoidAnimationController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        // Get the Animator component attached to this GameObject
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // ==========================================
        // METHOD 1: Play directly by State Name
        // ==========================================
        // Best for instant overrides without caring about transitions.
        if (Input.GetKeyDown(KeyCode.P))
        {
            // "DanceState" must be the exact name of the node in the Animator window
            animator.Play("DanceState"); 
            
            // Note: You can also add a transition duration:
            // animator.CrossFade("DanceState", 0.2f); 
        }

        // ==========================================
        // METHOD 2: Using Animator Parameters
        // ==========================================
        // Best practice for state machines and smooth blending.

        // A. Trigger (Good for one-off actions like jumping or attacking)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("JumpTrigger"); 
        }

        // B. Boolean (Good for continuous toggle states like crouching or running)
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        animator.SetBool("IsRunning", isRunning);

        // C. Float (Good for Blend Trees, like moving from idle to walk to run)
        float verticalInput = Input.GetAxis("Vertical"); // Returns -1 to 1 based on W/S or Up/Down arrows
        animator.SetFloat("MoveSpeed", Mathf.Abs(verticalInput)); 
    }
}