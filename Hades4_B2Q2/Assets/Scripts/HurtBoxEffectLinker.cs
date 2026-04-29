using UnityEngine;

namespace MaquestiauxMark.Hades
{
    public class HurtBoxEffectLinker : MonoBehaviour
    {
        //[SerializeField] private CameraShake _cameraShake;
        [SerializeField] private HurtBox _hurtBox;
        [SerializeField] private AudioSource _attackSoundSource;
        [SerializeField] private ParticleSystem _attackVFX;

        [SerializeField] private float _vibrationLowFrequency;
        [SerializeField] private float _vibrationHighFrequency;
        [SerializeField] private float _vibrationDuration;


        void Start()
        {
            _hurtBox.OnSuccessfulHit += ApplyEffects;
        }

        private void ApplyEffects()
        {
            HapticsController.s_hapticsInstance.HapticsPulse(_vibrationLowFrequency, _vibrationHighFrequency, _vibrationDuration);
            
            if(_attackSoundSource)
            {
                _attackSoundSource.Play();
            }
            if (_attackVFX)
            {
                _attackVFX.Play();
            }
            //_attackSoundSource.Play();
            //StartCoroutine(_cameraShake.ShakeCamera(0.1f, 0.1f));
            //_attackVFX.Play();
        }
    }
}
