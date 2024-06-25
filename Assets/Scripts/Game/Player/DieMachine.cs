using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieMachine : StateMachineBehaviour
{
   
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        animator.SetInteger("DieId", Random.Range(0, 3));
    }

}
