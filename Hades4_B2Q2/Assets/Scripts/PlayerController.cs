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
        private bool _canMove = true;
        private bool _canAttack = true;
        private bool _canEvade = true;

        [SerializeField] LayerMask _evadeLayerMask;
        [SerializeField] private float _maxWallDistance;

        [SerializeField] private float _deadZone;
        [SerializeField] private float _normalMoveSpeed;
        [SerializeField] private float _evadeSpeed;
        [SerializeField] private float _evadeTime;
        [SerializeField] private float _evadeReloadTime;
        private float _moveSpeed;

        [SerializeField] private int _baseAttackDamage;
        [SerializeField] private int _rangedAttackDamage;


        [SerializeField] private HurtBox _attackHurtBox;
        [SerializeField] private HealthComponent _health;
        [SerializeField] private BaseAttackController _baseAttackController;
        [SerializeField] private RangedAttackController _rangedAttackController;


        [Header("Animator")]
        [SerializeField] private Animator _animator;
        [SerializeField] private float _animatorSpeedNormal;
        [SerializeField] private float _animatorSpeedEvade;
        [SerializeField] private string _speedAnimatorName;
        [SerializeField] private string _baseAttackAnimatorName;
        [SerializeField] private string _rangedAttackAnimatorName;
        [SerializeField] private string _deathAnimatorName;

        public HealthComponent GetHealth() { return _health; }

        private void Start()
        {
            _health.OnDamaged += Damaged;
            _health.OnDeath += Death;

            _moveSpeed = _normalMoveSpeed;
        }

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

        void OnAttack() // Mix Functions ???
        {
            if(_canAttack)
            {
                _canMove = false;
                _canAttack = false;
                _baseAttackController.Initialise(_baseAttackDamage, _baseAttackAnimatorName);
                _attackHurtBox.OnAttackEnd += EndAttack;
            }
        }
        void OnRangedAttack()
        {
            if (_canAttack)
            {
                _canAttack = false;
                _canMove = false;
                _rangedAttackController.Initialise(_rangedAttackDamage, _rangedAttackAnimatorName);
                _rangedAttackController.OnAttackEnd += EndAttack;
            }
        }

        void EndAttack()
        {
            _canMove = true;
            _canAttack = true;
            _attackHurtBox.OnAttackEnd -= EndAttack;
            _rangedAttackController.OnAttackEnd -= EndAttack;
        }

        void OnEvade()
        {
            if(_canEvade)
            {
                
                if(_moveDirection == Vector2.zero)
                {
                    _moveDirection = new(transform.forward.x, transform.forward.z);
                }
                if (_moveDirection != Vector2.zero)
                {
                    //Start Evade
                    _moveDirection = _moveDirection.normalized;
                    _moveSpeed = _evadeSpeed;
                    _animator.speed = _animatorSpeedEvade;
                    _canMovementChange = false;
                    _canEvade = false;
                    _health.ToggleInvincibility();
                    //Timer
                    StartCoroutine(EvadeDelay());
                    //Stop Evade
                }
            }
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
            StartCoroutine(EvadeReload());
        }
        IEnumerator EvadeReload()
        {
            yield return new WaitForSeconds(_evadeReloadTime);
            _canEvade = true;
        }
        #endregion


        void Update()
        {
            BasicMovement();
        }

        void BasicMovement()
        {
            if (_moveDirection.magnitude < _deadZone || !_canMove)
            {
                _animator.SetFloat(_speedAnimatorName, 0);
                return;
            }
            if (!_canMovementChange && Physics.Raycast(transform.position, transform.forward, out _, _maxWallDistance, _evadeLayerMask))
            {
                Debug.Log("Blocked");
                return;
            }
            transform.Translate(_moveSpeed * Time.deltaTime * new Vector3(_moveDirection.x, 0, _moveDirection.y), Space.World);
            _animator.SetFloat(_speedAnimatorName, _moveDirection.magnitude);
            if (_moveDirection.magnitude > 0)
            {
                transform.rotation = Quaternion.LookRotation(new Vector3(_moveDirection.x, 0, _moveDirection.y), Vector3.up);
            }
        }

        private void Death()
        {
            //Stop Player Inputs
            
            //Play Animation
            _animator.SetTrigger(_deathAnimatorName);
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
