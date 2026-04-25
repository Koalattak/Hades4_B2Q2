using System;
using UnityEngine;

namespace MaquestiauxMark.Hades
{
    public class HealthComponent : MonoBehaviour
    {
        private int _healthCurrent;
        [SerializeField] private int _healthMax;

        public event Action OnDamaged;
        public event Action OnDeath;

        private bool _isDead;
        private bool _isInvincible;


        private void Start()
        {
            _healthCurrent = _healthMax;
            _isDead = false;
            _isInvincible = false;
        }

        //Take Damage Function
        public void TakeDamage(int damage)
        {
            if (_isDead || _isInvincible) return;
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
            _isInvincible = !_isInvincible;
            Debug.Log(_isInvincible);
        }
    }
}
