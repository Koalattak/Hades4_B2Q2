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
        private bool _isDead;
        private bool _canMovementChange = true;
        private bool _canMove = true;
        private bool _canAttack = true;
        private bool _canEvade = true;
        private bool _isPaused = true;

        public event Action OnReset;

        [SerializeField] LayerMask _evadeLayerMask;
        [SerializeField] private float _maxWallDistance;

        [SerializeField] private float _deadZone;
        [SerializeField] private float _normalMoveSpeed;
        [SerializeField] private float _evadeSpeed;
        [SerializeField] private float _evadeTime;
        [SerializeField] private float _evadeReloadTime;
        [SerializeField] private float _deathReloadTime;
        private float _moveSpeed;

        [SerializeField] private int _baseAttackDamage;
        [SerializeField] private int _rangedAttackDamage;


        [SerializeField] private HurtBox _attackHurtBox;
        [SerializeField] private HealthComponent _health;
        [SerializeField] private BaseAttackController _baseAttackController;
        [SerializeField] private RangedAttackController _rangedAttackController;
        [SerializeField] private UIManager _interfaceManager;


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

            SpawnManager.SetCheckpoint(transform);
            ResetPlayer();
        }

        public void ResetPlayer()
        {
            _health.OnDamaged += Damaged;
            _health.OnDeath += Death;
            _interfaceManager.ActivateUICanvas();

            _moveSpeed = _normalMoveSpeed;
            _isDead = false;
            _animator.SetBool(_deathAnimatorName, _isDead);
            _health.SetHealthToMax();
            transform.position = SpawnManager.GetCheckpoint().position;
            OnReset?.Invoke();
        }

        #region Inputs
        void OnMove(InputValue inputValue)
        {
            if (_isDead) return;
            if (_canMovementChange)
            {
                _moveDirection = inputValue.Get<Vector2>();
                Vector3 cameraForwardVector = Camera.main.transform.forward;
                Vector3 cameraRightVector = Camera.main.transform.right;

                cameraForwardVector.y = 0;
                cameraRightVector.y = 0;

                Vector3 forwardRelative = _moveDirection.y * cameraForwardVector;
                Vector3 rightRelative = _moveDirection.x * cameraRightVector;

                Vector3 moveDir = forwardRelative + rightRelative;

                _moveDirection.x = moveDir.x;
                _moveDirection.y = moveDir.z;

                _moveDirection = _moveDirection.normalized;
            }
            else
            {
                _tempMoveDirection = inputValue.Get<Vector2>();
            }
        }

        void OnAttack() // Mix Functions ???
        {
            if (_isDead) return;
            if (_canAttack)
            {
                _canMove = false;
                _canAttack = false;
                _baseAttackController.Initialise(_baseAttackDamage, _baseAttackAnimatorName);
                _attackHurtBox.OnAttackEnd += EndAttack;
            }
        }
        void OnRangedAttack()
        {
            if (_isDead) return;
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
            if (_isDead) return;
            if (_canEvade)
            {
                if (_moveDirection == Vector2.zero)
                {
                    _moveDirection = new(transform.forward.x, transform.forward.z);
                }
                //Start Evade
                _moveDirection = _moveDirection.normalized;
                _moveSpeed = _evadeSpeed;
                _animator.speed = _animatorSpeedEvade;
                _canMovementChange = false;
                _canEvade = false;
                _health.ToggleInvincibility();
                //Timer
                StartCoroutine(EvadeDelay());
            }
        }

        void OnMenu()
        {
            if (!_isPaused)
            {
                _interfaceManager.ActivatePauseCanvas();
                _isPaused = true;
            }
            else
            {
                _interfaceManager.ActivateUICanvas();
                _isPaused = false;
            }
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
            if (_isDead) return;
            BasicMovement();
        }

        void BasicMovement()
        {
            if (_moveDirection.magnitude < _deadZone || !_canMove)
            {
                _animator.SetFloat(_speedAnimatorName, 0);
                return;
            }
            if (!_canMovementChange && Physics.Raycast(transform.position, transform.forward, out _, _maxWallDistance, _evadeLayerMask)) return;
            transform.Translate(_moveSpeed * Time.deltaTime * new Vector3(_moveDirection.x, 0, _moveDirection.y), Space.World);
            _animator.SetFloat(_speedAnimatorName, _moveDirection.magnitude);
            if (_moveDirection.magnitude > 0)
            {
                transform.rotation = Quaternion.LookRotation(new Vector3(_moveDirection.x, 0, _moveDirection.y), Vector3.up);
            }
        }

        private void Death()
        {
            _isDead = true; //Stop Player Inputs

            _animator.SetBool(_deathAnimatorName, _isDead); //Play Animation

            _health.OnDamaged -= Damaged;
            _health.OnDeath -= Death;

            //Stop Enemy Attacks -> In Enemy Script
            StartCoroutine(DeathDelay());
        }

        private void Damaged()
        {
            //Play Animation
            //Invincibility Frames
            //Play Sound
            //Play Haptics
        }

        private IEnumerator DeathDelay()
        {
            yield return new WaitForSeconds(_deathReloadTime); //Wait a bit
            _interfaceManager.ActivateDeathCanvas(); //Show Retry Screen
        }
    }
}
