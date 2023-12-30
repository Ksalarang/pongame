using UnityEngine;

namespace Utils.Extensions {
public static class AnimatorExtensions {
    
    public static float getAnimationDuration(this Animator animator, string animationName) {
        foreach (var clip in animator.runtimeAnimatorController.animationClips) {
            if (clip.name == animationName) return clip.length;
        }
        return 0f;
    }
}
}