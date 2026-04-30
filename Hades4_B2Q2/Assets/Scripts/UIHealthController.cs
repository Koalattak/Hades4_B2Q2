using UnityEngine;
using UnityEngine.UI;

namespace MaquestiauxMark.Hades
{
    public class UIHealthController : MonoBehaviour
    {
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private HealthComponent _health;
        private Color _initialColor;
        [SerializeField] private Color _endColor = new(0f, 0f, 0f, 0f);

        void Start()
        {
            _initialColor = _backgroundImage.color;
            _health.OnDamaged += OnHealthChanged;
            _health.OnDeath += OnHealthChanged;
            _health.OnDeath += OnDeath;
            _health.OnChanged += OnHealthChanged;
        }

        private void OnHealthChanged()
        {
            _backgroundImage.color = _initialColor;
            _healthSlider.value = _health.GetHealthPercent();
        }
        private void OnDeath()
        {
            _backgroundImage.color = _endColor;
        }
    }
}
