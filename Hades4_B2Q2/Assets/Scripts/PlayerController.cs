using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static Codice.Client.Common.EventTracking.TrackFeatureUseEvent.Features.DesktopGUI.Filters;

namespace MaquestiauxMark.Hades
{
    public class PlayerController : MonoBehaviour
    {
        private Vector2 _moveDirection = Vector2.zero;
        private Vector2 _tempMoveDirection = Vector2.zero;
        private bool _canMovementChange = true;

        [SerializeField] private float _deadZone;
        [SerializeField] private float _normalMoveSpeed;
        [SerializeField] private float _evadeSpeed;
        [SerializeField] private float _evadeTime;
        private float _moveSpeed;
        //private Vector2 _unNormalisedMoveDirection;

        [SerializeField] private int _attackDamage;

        private AttackHit _attackHit;
        public event Action<Collider, int> OnEnemyHit;

        //[SerializeField] private Collider _attackHurtBox;
        [SerializeField] private HurtBox _attackHurtBox;
        [SerializeField] private HealthComponent _health;

        [Header("Animator")]
        [SerializeField] private Animator _animator;
        [SerializeField] private float _animatorSpeedNormal;
        [SerializeField] private float _animatorSpeedEvade;
        [SerializeField] private string _speedAnimatorName;
        [SerializeField] private string _punchAnimatorName;



        #region Inputs
        void OnMove(InputValue inputValue)
        {
            if (_canMovementChange)
            {
                _moveDirection = inputValue.Get<Vector2>();
            }
            else
            {
                _tempMoveDirection = inputValue.Get<Vector2>();
            }
        }

        void OnAttack()
        {
            _animator.SetTrigger(_punchAnimatorName);
            Debug.Log("Attack");
        }

        void OnEvade()
        {
            if (_moveDirection != Vector2.zero)
            {
                //Start Evade
                _moveDirection = _moveDirection.normalized;
                _moveSpeed = _evadeSpeed;
                _animator.speed = _animatorSpeedEvade;
                _canMovementChange = false;
                _health.ToggleInvincibility();
                //Timer
                StartCoroutine(EvadeDelay());
                //Stop Evade
            }
        }

        void OnRangedAttack()
        {
            Debug.Log("Ranged Attack");
        }

        void OnMenu()
        {
            Debug.Log("Menu");

        }
        IEnumerator EvadeDelay()
        {
            yield return new WaitForSeconds(_evadeTime);
            _animator.speed = _animatorSpeedNormal;
            _moveSpeed = _normalMoveSpeed;
            _moveDirection = _tempMoveDirection;
            _canMovementChange = true;
            _health.ToggleInvincibility();

        }
        #endregion

        private void Start()
        {
            _health.OnDamaged += Damaged;
            _health.OnDeath += Death;

            _moveSpeed = _normalMoveSpeed;

            _attackHit = _animator.GetBehaviour<AttackHit>();
            _attackHit.Attack += OnAttackHit;
            _attackHit.StopAttack += OnAttackStop;
        }

        void Update()
        {
            BasicMovement();
        }

        void BasicMovement()
        {
            if (_moveDirection.magnitude < _deadZone)
            {
                _animator.SetFloat(_speedAnimatorName, 0);
                return;
            }
            transform.Translate(_moveSpeed * Time.deltaTime * new Vector3(_moveDirection.x, 0, _moveDirection.y), Space.World);
            _animator.SetFloat(_speedAnimatorName, _moveDirection.magnitude);
            if (_moveDirection.magnitude > 0)
            {
                transform.rotation = Quaternion.LookRotation(new Vector3(_moveDirection.x, 0, _moveDirection.y), Vector3.up);
            }
        }

        private void OnAttackHit()
        {
            _attackHurtBox.ActivateHurtBox(_attackDamage);
        }

        private void OnAttackStop()
        {
            _attackHurtBox.DeactivateHurtBox();
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("test");
            OnEnemyHit?.Invoke(other, 10);
        }

        private void Death()
        {
            //Play Animation
            //Stop Enemy Attacks
            //Wait a bit
            //Show Retry Screen
        }

        private void Damaged()
        {
            //Play Animation
            //Invincibility Frames
            //Play Sound
            //Play Haptics
        }


    }
}
