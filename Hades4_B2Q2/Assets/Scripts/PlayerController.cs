using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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
        private bool _isPaused = false;

        public event Action OnReset;
        public event Action OnEvadeStart;
        public event Action<float> OnEvadeChange;

        [Header("Evade")]
        [SerializeField] private float _maxWallDistance;
        [SerializeField] private float _evadeSpeed;
        [SerializeField] private float _evadeTime;
        [SerializeField] private float _evadeReloadTime;
        private float _evadeCooldown;
        [SerializeField] LayerMask _evadeLayerMask;

        [Header("Movement")]
        [SerializeField] private float _deadZone;
        [SerializeField] private float _normalMoveSpeed;
        [SerializeField] private float _deathReloadTime;
        private float _moveSpeed;

        [Header("Attacks")]
        [SerializeField] private int _baseAttackDamage;
        [SerializeField] private int _rangedAttackDamage;

        [Header("References")]
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

        private float _dashReloadPercent => (_evadeTime - _evadeCooldown) / _evadeTime;
        public HealthComponent GetHealth() { return _health; }

        private void Start()
        {

            SpawnManager.SetCheckpoint(transform);
            ResetPlayer();
        }

        public void ResetPlayer()
        {
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
            if (_isDead || _isPaused) return;
            if (_canMovementChange)
            {
                //Logic followed from this video : https://www.youtube.com/watch?v=reWtxGTyN78
                //Movement facing the camera 
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

        void OnAttack() { Attack(false); }
        void OnRangedAttack() { Attack(true); }

        void Attack(bool isRangedAttack) //Generalised Method for Both Attacks
        {
            if (_isDead || !_canAttack || _isPaused) return;
            _canMove = false;
            _canAttack = false;
            if (isRangedAttack)
            {
                _rangedAttackController.Initialise(_rangedAttackDamage, _rangedAttackAnimatorName);
                _rangedAttackController.OnAttackEnd += EndAttack;
            }
            else
            {
                _baseAttackController.Initialise(_baseAttackDamage, _baseAttackAnimatorName);
                _attackHurtBox.OnAttackEnd += EndAttack;
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
            if (_isDead || _isPaused) return;
            if (_canEvade)
            {
                if (_moveDirection == Vector2.zero) //Allows Dash when Immobile
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
                OnEvadeStart?.Invoke();
                //Timer
                _evadeCooldown = _evadeTime;
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
        }
        private void EvadeReload()
        {
            if (_canEvade) return;

            if (_evadeCooldown > 0)
                _evadeCooldown -= Time.deltaTime;
            else
                _canEvade = true;

            OnEvadeChange?.Invoke(_dashReloadPercent); //For UI
        }
        #endregion


        void Update()
        {
            if (_isDead) return;
            BasicMovement();
            EvadeReload();
        }

        void BasicMovement()
        {
            if (_moveDirection.magnitude < _deadZone || !_canMove)
            {
                _animator.SetFloat(_speedAnimatorName, 0);
                return;
            }
            //Checks for Wall Collisions (Due to not using Rigidbody Movements)
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

            _health.OnDeath -= Death;

            //Stop Enemy Attacks -> In Enemy Script
            StartCoroutine(DeathDelay());
        }

        private IEnumerator DeathDelay()
        {
            yield return new WaitForSeconds(_deathReloadTime); //Wait a bit
            _interfaceManager.ActivateDeathCanvas(); //Show Retry Screen
        }
    }
}
