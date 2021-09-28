using UnityEngine;
namespace Fossil
{
    public class DestroyOnAnimationFinish : StateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Destroy(animator.gameObject);
        }
    }
}