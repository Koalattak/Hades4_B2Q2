using UnityEngine;

namespace MaquestiauxMark.Hades
{
    public class EvadeToEffects : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private EffectLinker _effectLinker;
        void Start()
        {
            _player.OnEvadeStart += ApplyEffects;
        }

        private void ApplyEffects()
        {
            _effectLinker.ApplyEffects(transform.position, transform.rotation);
        }
    }
}
