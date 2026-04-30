using UnityEngine;
using System;

namespace MaquestiauxMark.Hades
{
    public class BaseAttackController : MonoBehaviour
    {
        [SerializeField] private HurtBox _attackHurtBox;
        [SerializeField] private Animator _animator;
        private AttackHit _attackHit;
        private int _attackDamage;
        private string _animatorParameterName;
        public event Action OnAttackStart;

        public void Initialise(int attackDamage, string animatorParameterName)
        {
            _attackDamage = attackDamage;
            _animatorParameterName = animatorParameterName;

            _attackHit = _animator.GetBehaviour<AttackHit>();
            _attackHit.Attack += OnAttackHit;
            _attackHit.StopAttack += OnAttackStop;

            _animator.SetTrigger(_animatorParameterName);
            OnAttackStart?.Invoke();
        }

        private void OnAttackHit()
        {
            _attackHurtBox.ActivateHurtBox(_attackDamage);
        }

        private void OnAttackStop()
        {
            _attackHurtBox.DeactivateHurtBox();
        }
    }
}
