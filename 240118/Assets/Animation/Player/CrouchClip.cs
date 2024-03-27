using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchClip : StateMachineBehaviour
{
    CapsuleCollider2D myCollider;//콜라이더 받아오기

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(myCollider == null)//첫 적용시에는 콜라이더를 못받아왔기에, 있는지 없는지 체크
            myCollider=animator.gameObject.GetComponent<CapsuleCollider2D>();//콜라이더 적용

        myCollider.direction = CapsuleDirection2D.Horizontal;//콜라이더 기준방향을 변경
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        myCollider.direction = CapsuleDirection2D.Vertical;//콜라이더 기준방향을 변경
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
