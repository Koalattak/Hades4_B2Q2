using UnityEngine;
using UnityEngine.UI;

namespace MaquestiauxMark.Hades
{
    public class UIHealthController : MonoBehaviour
    {
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private HealthComponent _health;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _health.OnDamaged += OnHealthChanged;
            _health.OnDeath += OnHealthChanged;
            OnHealthChanged();
        }

        private void OnHealthChanged()
        {
            _healthSlider.value = _health.GetHealthPercent();
        }
    }
}
