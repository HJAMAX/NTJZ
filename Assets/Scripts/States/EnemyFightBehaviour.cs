using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFightBehaviour : StateMachineBehaviour
{
    private float currentTime;

    private Bullet bullet;

    private bool isShooted;
	 
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject bulletObj = Game.Instance.ObjectPool.Spawn("Dart");//Instantiate(ObjectPool.Instance.bullet, animator.transform.position, Quaternion.identity);
        bulletObj.transform.position = animator.transform.position;

        bullet = bulletObj.GetComponent<Bullet>();
        bullet.Fire();

        currentTime = animator.recorderStartTime;
        isShooted = false;
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentTime += Time.deltaTime;
        if(currentTime - animator.recorderStartTime > 0.2f && !isShooted)
        {
            bullet.GetComponent<SpriteRenderer>().enabled = true;
            //爆炸效果
            Game.Instance.EventBus.SendEvents("explosion", 0, bullet.transform.position);
            isShooted = true;
        }
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
