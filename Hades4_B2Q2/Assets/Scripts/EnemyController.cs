using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;

namespace MaquestiauxMark.Hades
{
    enum EEnemyState
    {
        Idle,
        Follow,
        RangedAttack,
        Attack
    }

    public class EnemyController : MonoBehaviour
    {
        private bool _stopAllActions;
        private bool _canRangeAttack;
        private bool _isAttacking;
        private bool _instantDeath = false;
        private HealthComponent _playerHealth;

        [SerializeField] private float _deathDelay;

        private EEnemyState _currentState;
        [SerializeField] private EEnemyState _initialState;

        [Header("Attack Stats")]
        [SerializeField] private int _baseAttackDamage;
        [SerializeField] private int _rangeAttackDamage;
        [SerializeField] private float _rangeAttackCooldown;
        [SerializeField] private float _baseAttackRecoil;
        [SerializeField] private float _rangeAttackRecoil;

        [Header("Animator Name")]
        [SerializeField] private string _movementAnimatorName;
        [SerializeField] private string _attackAnimatorName;
        [SerializeField] private string _rangeAttackAnimatorName;
        [SerializeField] private string _deathAnimatorName;

        [Header("Distances")]
        [SerializeField] private float _aggressiveDistance;
        [SerializeField] private float _rangedAttackDistance;
        [SerializeField] private float _inBetweenAttacksDistance;
        [SerializeField] private float _baseAttackDistance;

        [Header("References")]
        [SerializeField] private Collider _hitBox;
        [SerializeField] private BaseAttackController _baseAttackController;
        [SerializeField] private RangedAttackController _rangedAttackController;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Animator _animator;
        [SerializeField] private HealthComponent _health;
        private PlayerController _playerRef;

        public HealthComponent GetHealth() { return _health; }

        public void InitialiseEnemy(PlayerController playerRef)
        {
            _playerRef = playerRef;
            _playerHealth = _playerRef.GetHealth();
            _playerHealth.OnDeath += OnPlayerDeath;
            ResetEnemy();
        }

        #region Death and Damage

        private void ResetEnemy()
        {
            _health.OnDamaged -= EnemyDamage;
            _health.OnDeath -= EnemyDeath;

            _currentState = _initialState;
            _health.SetHealthToMax();
            _health.OnDamaged += EnemyDamage;
            _health.OnDeath += EnemyDeath;
            _instantDeath = false;
            _hitBox.enabled = true;
            _canRangeAttack = true;
            _stopAllActions = false;
        }

        private void OnPlayerDeath()
        {
            if (!gameObject) return;
            _stopAllActions = true;
            OnStateExit();
            _animator.SetBool(_movementAnimatorName, false);
            _currentState = _initialState;
            if (_navMeshAgent && gameObject)
            {
                _navMeshAgent.isStopped = true;
            }
        }

        private void EnemyDamage()
        {
            if (_currentState == _initialState)
            {
                OnStateChange(EEnemyState.Follow);
            }
        }
        public void EnemyInstantDeath()
        {
            _instantDeath = true;
            EnemyDeath();
        }

        private void EnemyDeath()
        {
            if (!gameObject) return;
            _health.OnDamaged -= EnemyDamage;
            _health.OnDeath -= EnemyDeath;
            _playerHealth.OnDeath -= OnPlayerDeath;
            _navMeshAgent.isStopped = true;
            _stopAllActions = true;
            _hitBox.enabled = false;
            _animator.SetBool(_deathAnimatorName, true);
            if (_instantDeath)
            {
                Destroy(gameObject);
            }
            else
            {
                StartCoroutine(DeathDelay()); //Destroy After a Delay
            }
        }

        private IEnumerator DeathDelay()
        {
            yield return new WaitForSeconds(_deathDelay);
            Destroy(gameObject);
        }

        #endregion
        #region Finite State Machine

        private void Update()
        {
            if (!_stopAllActions)
            {
                switch (_currentState)
                {
                    case EEnemyState.Idle:
                        if (IsPlayerCloserThan(_aggressiveDistance)) //Check Distance from Player, if the distance is lower than Threshold -> Change State
                            OnStateChange(EEnemyState.Follow);
                        break;

                    case EEnemyState.Follow:
                        FollowUpdate();
                        break;
                    case EEnemyState.RangedAttack:
                        if (!_isAttacking)
                            OnStateChange(EEnemyState.Follow);
                        break;

                    case EEnemyState.Attack:
                        if (!_isAttacking)
                            OnStateChange(EEnemyState.Follow);
                        break;

                    default: break;
                }
            }
        }

        private void FollowUpdate()
        {
            if (IsPlayerCloserThan(_baseAttackDistance)) //If Player is in attack range
            {
                OnStateChange(EEnemyState.Attack);
                return;
            }
            else if (IsPlayerCloserThan(_inBetweenAttacksDistance)) { } //If Player is almost in basic attack Range -> Get Closer
            else if (IsPlayerCloserThan(_rangedAttackDistance)) //If Player is in too far for Basic Attack
            {
                OnStateChange(EEnemyState.RangedAttack);
                return;
            }
            //If Player is Too Far for Range Attack -> Get Closer
            _navMeshAgent.destination = _playerRef.transform.position;
        }

        private bool IsPlayerCloserThan(float range)
        {
            return Vector3.Distance(_playerRef.transform.position, transform.position) < range;
        }

        private void OnStateChange(EEnemyState newState)
        {
            OnStateExit();
            _currentState = newState;
            OnStateEnter();
        }

        private void OnStateEnter()
        {
            switch (_currentState)
            {
                case EEnemyState.Follow:
                    OnFollowEnter();
                    break;
                case EEnemyState.RangedAttack:
                    OnRangeAttackEnter();
                    break;
                case EEnemyState.Attack:
                    OnAttackEnter();
                    break;
                default: break;
            }
        }

        private void OnFollowEnter()
        {
            _navMeshAgent.destination = _playerRef.transform.position;
            _navMeshAgent.isStopped = false;
            _animator.SetBool(_movementAnimatorName, true);
        }

        private void OnRangeAttackEnter()
        {
            if (_canRangeAttack)
            {
                _isAttacking = true;
                _canRangeAttack = false;
                transform.LookAt(_playerRef.transform.position);//Face Player
                _rangedAttackController.Initialise(_rangeAttackDamage, _rangeAttackAnimatorName);
                StartCoroutine(RangedAttackRecoverDelay());
            }
        }
        private void OnAttackEnter()
        {
            if (!_isAttacking)
            {
                _isAttacking = true;
                transform.LookAt(_playerRef.transform.position); //Face Player
                _baseAttackController.Initialise(_baseAttackDamage, _attackAnimatorName);
                StartCoroutine(BaseAttackRecoverDelay());
            }
        }

        private void OnStateExit()
        {
            switch (_currentState)
            {
                case EEnemyState.Follow:
                    _navMeshAgent.isStopped = true;
                    _animator.SetBool(_movementAnimatorName, false);
                    break;
                default: break;
            }
        }
        #endregion
        #region Attack Delays 
        private IEnumerator BaseAttackRecoverDelay()
        {
            yield return new WaitForSeconds(_baseAttackRecoil);
            if (!_stopAllActions)
            {
                _isAttacking = false;
                OnStateChange(EEnemyState.Follow);
            }
        }
        private IEnumerator RangedAttackRecoverDelay()
        {
            yield return new WaitForSeconds(_rangeAttackRecoil);
            if (!_stopAllActions)
            {
                _isAttacking = false;
                OnStateChange(EEnemyState.Follow);

                StartCoroutine(RangedAttackReload());
            }
        }
        private IEnumerator RangedAttackReload()
        {
            yield return new WaitForSeconds(_rangeAttackCooldown);
            if (!_stopAllActions)
            {
                _canRangeAttack = true;
            }
        }
        #endregion
    }
}
