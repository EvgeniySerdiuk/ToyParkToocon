using UnityEngine;

namespace Project.Scripts.AI.Workers
{
    public class WorkersAnimator
    {
        private Animator animator;
        
        public WorkersAnimator(Animator animator)
        {
            this.animator = animator;
        }

        public void SetMoveAnimation()
        {
            animator.SetFloat("SpeedMultiplier", 1);
        }

        public void SetIdleAnimation()
        {
            animator.SetFloat("SpeedMultiplier", 0);
        }
    }
}