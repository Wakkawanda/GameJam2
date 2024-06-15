using UnityEngine;

namespace Animation_Based_Self_Destruction
{
    public class DestroyOnExit : StateMachineBehaviour {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            Destroy(animator.gameObject, stateInfo.length);
        }
    }
}