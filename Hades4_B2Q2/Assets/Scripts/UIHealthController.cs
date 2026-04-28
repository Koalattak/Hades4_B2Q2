using UnityEngine;
using UnityEngine.UI;

namespace MaquestiauxMark.Hades
{
    public class UIHealthController : MonoBehaviour
    {
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private HealthComponent _health;
        [SerializeField] private Color _endColor = new(0f, 0f, 0f, 0f);


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _health.OnDamaged += OnHealthChanged;
            _health.OnDeath += OnHealthChanged;
            _health.OnDeath += OnDeath;
            _health.OnChanged += OnHealthChanged;
        }

        private void OnHealthChanged()
        {
            _healthSlider.value = _health.GetHealthPercent();
        }
        private void OnDeath()
        {
            _health.OnDamaged -= OnHealthChanged;
            _health.OnDeath -= OnHealthChanged;
            _health.OnDeath -= OnDeath;
            _health.OnChanged -= OnHealthChanged;

            _backgroundImage.color = _endColor;
        }
    }
}
