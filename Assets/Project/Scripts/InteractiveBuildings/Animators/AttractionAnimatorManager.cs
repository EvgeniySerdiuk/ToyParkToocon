using UnityEngine;

namespace Project.Scripts.Attractions.Animators
{
    public class AttractionAnimatorManager
    {
        private Animator animator;
        
        public AttractionAnimatorManager(Animator animator)
        {
            this.animator = animator;
        }

        public void SetPerformanceAnimation()
        {
            animator.SetTrigger("Perform");
        }

        public void SetIdleAnimation()
        {
            animator.SetTrigger("Idle");
        }
    }
}