using UnityEngine;

namespace MaquestiauxMark.Hades
{
    public class AttacksToEffects : MonoBehaviour
    {
        [SerializeField] private BaseAttackController _baseAttackController;
        [SerializeField] private RangedAttackController _rangedAttackController;
        [SerializeField] private EffectLinker _baseEffectLinker;
        [SerializeField] private EffectLinker _rangedEffectLinker;
        void Start()
        {
            _baseAttackController.OnAttackStart += BaseApplyEffects;
            _rangedAttackController.OnRangedAttackStart += RangedApplyEffects;
        }

        private void BaseApplyEffects()
        {
            _baseEffectLinker.ApplyEffects(transform.position, transform.rotation);
        }
        private void RangedApplyEffects()
        {
            _rangedEffectLinker.ApplyEffects(transform.position, transform.rotation);
        }
    }
}
