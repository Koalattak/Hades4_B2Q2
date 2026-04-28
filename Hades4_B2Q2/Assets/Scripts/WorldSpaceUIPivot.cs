using UnityEngine;

namespace MaquestiauxMark.Hades
{
    public class WorldSpaceUIPivot : MonoBehaviour
    {
        [SerializeField] private Transform _transformToChange;

        private void Update()
        {
            _transformToChange.LookAt(Camera.main.transform.position);
        }
    }
}
