using UnityEngine;
using UnityEngine.UI;

namespace MaquestiauxMark.Hades
{
    public class UIEvadeManager : MonoBehaviour
    {
        [SerializeField] private Slider _evadeSlider;
        [SerializeField] private PlayerController _playerRef;

        private void Start()
        {
            _playerRef.OnEvadeChange += UpdateEvadeUI;
        }

        private void UpdateEvadeUI(float percent)
        {
            _evadeSlider.value = percent;
        }
    }
}
