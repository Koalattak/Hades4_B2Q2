using UnityEngine;

namespace MaquestiauxMark.Hades
{
    public class ProjectileController : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private HurtBox _hurtBox;
        private int _damageToDeal;
        [SerializeField] private float _lifeTimeMax;
        private float _lifeTimeCurrent;
        private Vector3 _transformForward;

        public void Initialise(int damageToDeal, Vector3 forwardTransform)
        {
            _damageToDeal = damageToDeal;
            _hurtBox.ActivateHurtBox(_damageToDeal);
            _hurtBox.OnSuccessfulHit += ProjectileDestroy;
            _transformForward = forwardTransform;
        }


        void Update()
        {
            if(_lifeTimeCurrent > _lifeTimeMax)
            {
                ProjectileDestroy(Vector3.zero, Quaternion.identity);
            }
            else
            {
                _lifeTimeCurrent += Time.deltaTime;
                transform.Translate(_moveSpeed * Time.deltaTime * _transformForward);
            }
        }

        private void ProjectileDestroy(Vector3 h, Quaternion q)
        {
            _hurtBox.OnSuccessfulHit -= ProjectileDestroy;
            Destroy(gameObject);
        }
    }
}
