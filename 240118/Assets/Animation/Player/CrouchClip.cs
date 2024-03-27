using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchClip : StateMachineBehaviour
{
    CapsuleCollider2D myCollider;//�ݶ��̴� �޾ƿ���

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(myCollider == null)//ù ����ÿ��� �ݶ��̴��� ���޾ƿԱ⿡, �ִ��� ������ üũ
            myCollider=animator.gameObject.GetComponent<CapsuleCollider2D>();//�ݶ��̴� ����

        myCollider.direction = CapsuleDirection2D.Horizontal;//�ݶ��̴� ���ع����� ����
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        myCollider.direction = CapsuleDirection2D.Vertical;//�ݶ��̴� ���ع����� ����
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
