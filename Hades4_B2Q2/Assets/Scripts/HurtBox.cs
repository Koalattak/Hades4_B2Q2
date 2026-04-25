using UnityEngine;
using System;

namespace MaquestiauxMark.Hades
{
    public class HurtBox : MonoBehaviour
    {
        public event Action OnSuccessfulHit;
        [SerializeField] private Collider _hurtBox;
        private int _damageToDeal;


        void Awake()
        {
            DeactivateHurtBox();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.transform.root.TryGetComponent<HealthComponent>(out HealthComponent health)) return;
            health.TakeDamage(_damageToDeal);
            OnSuccessfulHit?.Invoke();
        }

        public void ActivateHurtBox(int damageToDeal)
        {
            _damageToDeal = damageToDeal;
            _hurtBox.enabled = true;
        }

        public void DeactivateHurtBox()
        {
            _hurtBox.enabled = false;
        }
    }
}
