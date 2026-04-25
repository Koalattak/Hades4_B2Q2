using UnityEngine;

namespace MaquestiauxMark.Hades
{
    public class EnemyController : MonoBehaviour
    {
        private bool _canAttackPlayer;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private Animator _animator;
        [SerializeField] private HealthComponent _health;
        [SerializeField] private PlayerController _playerRef;

        private void Start()
        {
            _playerRef.OnEnemyHit += EnemyHit;
            _health.OnDamaged += EnemyDamage;
            _health.OnDeath += EnemyDeath;
        }

        //private void OnTriggerEnter(Collider other)
        //{
        //    _health.TakeDamage(2);
        //}

        private void EnemyHit(Collider other, int damage)
        {
            Debug.Log("Hit");
            _health.TakeDamage(damage);
        }
        private void EnemyDamage()
        {
            //_health.TakeDamage(1);

            //Play Hit Anim
            //Play Hurt Sound
            Debug.Log("ouch");
        }

        private void EnemyDeath()
        {

        }
    }
}
