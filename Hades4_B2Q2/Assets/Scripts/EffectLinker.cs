using UnityEngine;

namespace MaquestiauxMark.Hades
{
    public class EffectLinker : MonoBehaviour
    {
        [SerializeField] private CameraShake _cameraShake;
        [SerializeField] private AudioSource _attackSoundSource;
        [SerializeField] private ParticleSystem _attackVFX;

        [SerializeField] private float _vibrationLowFrequency;
        [SerializeField] private float _vibrationHighFrequency;
        [SerializeField] private float _vibrationDuration;

        [SerializeField] private float _cameraShakeDuration;
        [SerializeField] private float _cameraShakeIntensity;

        void Start()
        {
            //_hurtBox.OnSuccessfulHit += ApplyEffects; //When HurtBox Touched a Valid Hitbox
        }

        private void ApplyEffects()
        {
            StartCoroutine(_cameraShake.ShakeCamera(_cameraShakeDuration, _cameraShakeIntensity));
            if (_vibrationDuration > 0)
            {
                HapticsController.s_hapticsInstance.HapticsPulse(_vibrationLowFrequency, _vibrationHighFrequency, _vibrationDuration);
            }

            if (_attackSoundSource)
            {
                _attackSoundSource.Play();
            }
            if (_attackVFX)
            {
                _attackVFX.Play();
            }
        }
    }
}
