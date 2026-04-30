using UnityEngine;
using System;

namespace MaquestiauxMark.Hades
{
    public class AttackHit : StateMachineBehaviour
    {
        public event Action Attack;
        public event Action StopAttack;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Attack?.Invoke();
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StopAttack?.Invoke();
        }
    }
}
