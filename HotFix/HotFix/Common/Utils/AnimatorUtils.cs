using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HotFix.Common.Utils
{
    public class AnimatorUtils
    {
        public static float GetClipLength(Animator animator,string clipName)
        {

            AnimationClip[] animationClip = animator.runtimeAnimatorController.animationClips;
            foreach (var item in animationClip)
            {
                if (item.name.Equals(clipName))
                {                   
                    return item.length;
                }
            }
            return 0;
        }
    }
}
