using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MaquestiauxMark.Hades
{
    public class PlayerController : MonoBehaviour
    {
        private Vector2 _moveDirection = Vector2.zero;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _evadeTime;
        private bool _canMovementChange = true;

        [SerializeField] private float _animatorSpeedNormal;
        [SerializeField] private float _animatorSpeedSlow;

        [SerializeField] private string _speedAnimatorName;
        [SerializeField] private string _punchAnimatorName;


        #region Inputs
        void OnMove(InputValue inputValue)
        {
            _moveDirection = inputValue.Get<Vector2>();
        }

        void OnAttack()
        {
            _animator.SetTrigger(_punchAnimatorName);
            Debug.Log("Attack");
        }

        void OnEvade()
        {
            //Start Evade
            _moveSpeed *= 2; //Funny Values
            _animator.speed = _animatorSpeedSlow; //Funny Values
            _canMovementChange = false;
            //Timer
            StartCoroutine(EvadeDelay());
            //Stop Evade
        }

        void OnRangedAttack(InputValue inputValue)
        {
            Debug.Log("Ranged Attack");

        }

        void OnMenu(InputValue inputValue)
        {
            Debug.Log("Menu");

        }
        #endregion

        IEnumerator EvadeDelay()
        {
            yield return new WaitForSeconds(_evadeTime);
            Debug.Log("Evade End");
            _animator.speed = _animatorSpeedNormal; //Funny Values
            _moveSpeed /= 2; //Funny Values
            _canMovementChange = false;

        }

        void Update()
        {
            BasicMovement();
        }

        void BasicMovement()
        {
            if(_canMovementChange)
            {
                transform.Translate(new Vector3(_moveDirection.x, 0, _moveDirection.y) * Time.deltaTime * _moveSpeed, Space.World);
                _animator.SetFloat(_speedAnimatorName, _moveDirection.magnitude);
                if (_moveDirection.magnitude > 0)
                {
                    transform.rotation = Quaternion.LookRotation(new Vector3(_moveDirection.x, 0, _moveDirection.y), Vector3.up);
                }
            }
        }
    }
}
