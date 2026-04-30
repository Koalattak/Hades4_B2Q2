using UnityEngine;

namespace MaquestiauxMark.Hades
{
    public class HealthToEffects : MonoBehaviour
    {
        [SerializeField] private HealthComponent _health;
        [SerializeField] private EffectLinker _effectLinker;
        void Start()
        {
            _health.OnDamaged += ApplyEffects;
        }

        private void ApplyEffects()
        {
            _effectLinker.ApplyEffects(transform.position, transform.rotation);
        }
    }
}
