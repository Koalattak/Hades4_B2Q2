using System;
using UnityEngine;

namespace MaquestiauxMark.Hades
{
    public class HealthComponent : MonoBehaviour
    {
        private int _healthCurrent;
        [SerializeField] private int _healthMax;

        public event Action OnDamaged;
        public event Action OnChanged;
        public event Action OnDeath;

        private bool _isDead;
        public bool IsInvincible { get; private set; }


        private void Start()
        {
            _healthCurrent = _healthMax;
            OnChanged?.Invoke();
            _isDead = false;
            IsInvincible = false;
        }

        //Take Damage Function
        public void TakeDamage(int damage)
        {
            if (_isDead || IsInvincible) return;
            _healthCurrent -= damage;
            _healthCurrent = Mathf.Clamp(_healthCurrent, 0, _healthMax);
            if(_healthCurrent > 0)
            {
                //Will Call All Damage Related Actions
                OnDamaged?.Invoke();
            }
            else
            {
                //Will Call All Death Related Actions
                OnDeath?.Invoke();
                _isDead = true;
            }
        }

        //Get Health
        public float GetHealthPercent()
        {
            return (float)_healthCurrent / (float)_healthMax;
        }

        //Invincibility Toggle
        public void ToggleInvincibility()
        {
            IsInvincible = !IsInvincible;
        }
    }
}
