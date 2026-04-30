using UnityEngine;

namespace MaquestiauxMark.Hades
{
    public class DeathToEffects : MonoBehaviour
    {
        [SerializeField] private HealthComponent _health;
        [SerializeField] private EffectLinker _effectLinker;
        void Start()
        {
            _health.OnDeath += ApplyEffects; //When HurtBox Touches a Valid Hitbox
        }

        private void ApplyEffects()
        {
            _effectLinker.ApplyEffects(transform.position, transform.rotation);
        }
    }
}
