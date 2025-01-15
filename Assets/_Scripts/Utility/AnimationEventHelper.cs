using UnityEngine;

namespace BossRush.Utility
{
    /// <summary>
    /// Helper class for the animation even system.
    /// </summary>
    public class AnimationEventHelper : MonoBehaviour
    {
        private static readonly int IS_SHOCKWAVE_COMPLETE = Animator.StringToHash("IsShockwaveComplete");

        /// <summary>
        /// Should be called after the animation of the Shockwave animation has finished.
        /// </summary>
        public void OnShockwaveAnimationComplete()
        {
            var animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool(IS_SHOCKWAVE_COMPLETE, true);
            }
        }
    }
}
