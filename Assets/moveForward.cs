using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveForward : StateMachineBehaviour
{
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Access the GameObject's transform through the animator
        animator.transform.position += animator.transform.forward * 2.0f * Time.deltaTime;
    }
}
