using System;
using UnityEngine;

public class ExplosionBehaviour : StateMachineBehaviour
{
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Game.Instance.ObjectPool.Unspawn(animator.gameObject);
    }
}
