using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle_Behaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        HealthManager h_instance = animator.gameObject.GetComponentInParent<HealthManager>();
        Player p_instance = animator.gameObject.GetComponentInParent<Player>();
        if (p_instance.should_attack && !h_instance.is_dead)
        {
            animator.SetTrigger("AttackOne");
            p_instance.can_attack = true;
            p_instance.Attack(0.4f);
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
