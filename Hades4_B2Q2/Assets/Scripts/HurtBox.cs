using UnityEngine;
using System;
using System.Collections.Generic;

namespace MaquestiauxMark.Hades
{
    public class HurtBox : MonoBehaviour
    {
        public event Action OnSuccessfulHit;
        public event Action OnAttackEnd;
        [SerializeField] private Collider _hurtBox;
        private int _damageToDeal;
        private List<HealthComponent> _enemiesHit = new();


        void Awake()
        {
            DeactivateHurtBox();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.transform.root.TryGetComponent<HealthComponent>(out HealthComponent health)) return;
            if(health.IsInvincible || health.GodMode || _enemiesHit.Contains(health)) return;
            _enemiesHit.Add(health);
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
            _enemiesHit.Clear();
            OnAttackEnd?.Invoke();
        }
    }
}
