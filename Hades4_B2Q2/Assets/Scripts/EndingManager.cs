using UnityEngine;

namespace MaquestiauxMark.Hades
{
    public class EndingManager : MonoBehaviour
    {
        [SerializeField] private UIManager _manager;
        private void OnTriggerEnter(Collider other)
        {
            if (!other.transform.root.TryGetComponent<PlayerController>(out _)) return;
            _manager.ActivateWinCanvas();
        }
    }
}
