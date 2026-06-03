using UnityEngine;

namespace MaquestiauxMark.Hades
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private Vector3 _positionOffset;
        [SerializeField] private Quaternion _rotationOffset;
        [SerializeField] private float _smoothTime;
        [SerializeField] private float _rotationSpeed;
        private Vector3 _positionVelocity;
        private Vector3 _targetPosition;
        private Quaternion _targetRotation;
        private bool _followPlayer;

        void Start()
        {
            transform.rotation = _rotationOffset;
            _followPlayer = true;
        }

        public void FollowPlayer()
        {
            _followPlayer = true;
        }

        public void MakeCameraStatic(Transform newCameraTransform)
        {
            _followPlayer = false;
            _targetPosition = newCameraTransform.position;
            _targetRotation = newCameraTransform.rotation;
        }

        void Update()
        {
            if (_followPlayer)
            {
                _targetPosition = _player.transform.position + _positionOffset;
                transform.rotation = Quaternion.Lerp(transform.rotation, _rotationOffset, Mathf.Lerp(0, 1, _rotationSpeed * Time.deltaTime));
            }
            else //In a Fixed Camera Zone
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, Mathf.Lerp(0, 1, _rotationSpeed * Time.deltaTime));
            }
            transform.position = Vector3.SmoothDamp(transform.position, _targetPosition, ref _positionVelocity, _smoothTime);
        }
    }
}
