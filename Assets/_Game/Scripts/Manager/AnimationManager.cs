using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    [SerializeField] private string animTrigger;

    public void PlayAnimation()
    {
        animator.SetBool(animTrigger, true);
    }
}
