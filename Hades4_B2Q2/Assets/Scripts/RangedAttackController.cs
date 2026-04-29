using System;
using UnityEngine;

namespace MaquestiauxMark.Hades
{
    public class RangedAttackController : MonoBehaviour
    {
        [SerializeField] private ProjectileController _projectile;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _spawnTransform;
        private RangedAttackHit _rangedAttackHit;
        private int _attackDamage;
        private string _animatorParameterName;
        public event Action OnAttackEnd;

        private void Start()
        {
            _rangedAttackHit = _animator.GetBehaviour<RangedAttackHit>();
            _rangedAttackHit.Attack += OnAttackHit;
            _rangedAttackHit.StopAttack += OnStopAttack;
        }

        public void Initialise(int attackDamage, string animatorParameterName)
        {
            _attackDamage = attackDamage;
            _animatorParameterName = animatorParameterName;

            _animator.SetTrigger(_animatorParameterName);
        }
        private void OnStopAttack()
        {
            OnAttackEnd?.Invoke();
        }

        private void OnAttackHit()
        {
            //Spawn Projectile
            ProjectileController spawnedProjectile = Instantiate(_projectile , _spawnTransform.position, Quaternion.identity);
            spawnedProjectile.Initialise(_attackDamage, transform.forward);
        }
    }
}
