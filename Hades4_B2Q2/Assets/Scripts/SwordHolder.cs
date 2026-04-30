using UnityEngine;

namespace MaquestiauxMark.Hades
{
    public class SwordHolder : MonoBehaviour
    {
        private Quaternion _fixedRotation;
        void Start()
        {
            _fixedRotation = transform.localRotation;
        }


        void Update()
        {
            transform.localRotation = _fixedRotation;
        }
    }
}
