using UnityEngine;

public class Animation_Trigger_Switching : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void SetAnimationController(RuntimeAnimatorController newController)
    {
        if (animator != null && animator.runtimeAnimatorController != newController)
        {
            animator.runtimeAnimatorController = newController;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
