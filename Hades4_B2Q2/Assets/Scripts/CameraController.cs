using UnityEngine;

namespace MaquestiauxMark.Hades
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private Vector3 _positionOffset;
        [SerializeField] private Quaternion _rotationOffset;
        [SerializeField] private float _smoothTime;
        private Vector3 _positionVelocity;

        void Start()
        {
            transform.rotation = _rotationOffset;
        }

        
        void Update()
        {
            Vector3 targetPosition = _player.transform.position + _positionOffset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _positionVelocity, _smoothTime);
        }
    }
}
