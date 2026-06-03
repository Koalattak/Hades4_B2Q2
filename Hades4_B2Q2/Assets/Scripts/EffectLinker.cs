using UnityEngine;

namespace MaquestiauxMark.Hades
{
    public class EffectLinker : MonoBehaviour
    {
        //Contains all of the types of feedback apart from the animations
        
        [SerializeField] private CameraShake _cameraShake;

        [SerializeField] private AudioSource _attackSoundSource;
        [SerializeField] private ParticleSystem _attackVFX;
        private ParticleSystem _spawnedVFX;

        [SerializeField] private float _vibrationLowFrequency;
        [SerializeField] private float _vibrationHighFrequency;
        [SerializeField] private float _vibrationDuration;

        [SerializeField] private float _cameraShakeDuration;
        [SerializeField] private float _cameraShakeIntensity;


        void Start()
        {
            if (_attackVFX)
            {
                _spawnedVFX = Instantiate(_attackVFX);
            }
        }

        public void ApplyEffects(Vector3 hitPosition, Quaternion hitRotation)
        {
            if (_cameraShake)
            {
                StartCoroutine(_cameraShake.ShakeCamera(_cameraShakeDuration, _cameraShakeIntensity));
            }
            if (_vibrationDuration > 0)
            {
                HapticsController.s_hapticsInstance.HapticsPulse(_vibrationLowFrequency, _vibrationHighFrequency, _vibrationDuration);
            }

            if (_attackSoundSource !=null)
            {
                _attackSoundSource.Play();
            }
            if (_spawnedVFX)
            {
                hitRotation.x = 0f;
                hitRotation.z = 0f;
                
                _spawnedVFX.transform.SetPositionAndRotation(hitPosition, hitRotation);
                _spawnedVFX.Play();
            }
        }
    }
}
