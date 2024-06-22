using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMachine : StateMachineBehaviour
{
   
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        animator.SetInteger("AttackId", Random.Range(0, 2));
    }

}
