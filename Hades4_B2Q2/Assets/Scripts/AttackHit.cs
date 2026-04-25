using UnityEngine;
using System;

namespace MaquestiauxMark.Hades
{
    public class AttackHit : StateMachineBehaviour
    {
        public event Action Attack;
        public event Action StopAttack;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Attack?.Invoke();
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StopAttack?.Invoke();
        }
    }
}
