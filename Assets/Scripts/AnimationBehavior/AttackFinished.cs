using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFinished : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.gameObject.CompareTag("NPC") || animator.gameObject.CompareTag("Enemy"))
        {
            animator.GetComponent<EnemyController>().canAttack = false;
            animator.GetComponent<EnemyController>().conAttackTime = 0;
        }
        else if (animator.gameObject.CompareTag("Player"))
        {
            animator.GetComponent<Player>().isAttackEnd = false;
        }
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.gameObject.CompareTag("NPC") || animator.gameObject.CompareTag("Enemy"))
        {
            animator.GetComponent<EnemyController>().canAttack = false;

        }
        else if (animator.gameObject.CompareTag("Player"))
        {
            animator.GetComponent<Player>().isAttackEnd = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.gameObject.CompareTag("NPC") || animator.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemy = animator.GetComponent<EnemyController>();
            enemy.canAttack = true;
            enemy.isChase = false;
            //enemy.RefreshAttackTime();
        }
        else if (animator.gameObject.CompareTag("Player"))
        {
            animator.GetComponent<Player>().isAttackEnd = true;
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
