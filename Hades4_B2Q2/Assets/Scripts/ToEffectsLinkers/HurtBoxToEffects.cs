using UnityEngine;

namespace MaquestiauxMark.Hades
{
    public class HurtBoxToEffects : MonoBehaviour
    {
        [SerializeField] private HurtBox _hurtBox;
        [SerializeField] private EffectLinker _effectLinker;
        void Start()
        {
            _hurtBox.OnSuccessfulHit += ApplyEffects; //When HurtBox Touches a Valid Hitbox
        }

        private void ApplyEffects(Vector3 hitPosition, Quaternion hitRotation)
        {
            _effectLinker.ApplyEffects(hitPosition, hitRotation);
        }
    }
}
