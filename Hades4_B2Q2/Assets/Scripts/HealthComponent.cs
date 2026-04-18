using System;
using UnityEngine;

namespace MaquestiauxMark.Hades
{
    public class HealthComponent : MonoBehaviour
    {
        private int _healthCurrent;
        [SerializeField] private int _healthMax;

        public event Action<int> HealthChanged;

        private void Start()
        {
            _healthCurrent = _healthMax;
            //Set up health Bar through events
            //
        }
    }
}
