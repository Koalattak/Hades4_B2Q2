using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MaquestiauxMark.Hades
{
    public class HapticsController : MonoBehaviour
    {
        public static HapticsController _instance;

        private Gamepad _controller;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }


        public void HapticsPulse(float lowFrequency, float highFrequency, float duration)
        {
            _controller = Gamepad.current;

            if (_controller != null)
            {
                _controller.SetMotorSpeeds(lowFrequency, highFrequency);

                StartCoroutine(StopHaptics(duration));
            }
        }


        private IEnumerator StopHaptics(float duration)
        {
            yield return new WaitForSeconds(duration);
            //Stops Rumble
            _controller.SetMotorSpeeds(0f, 0f);
        }

    }
}
