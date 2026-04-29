using UnityEngine;
using System.Collections;


namespace MaquestiauxMark.Hades
{
    public class CameraShake : MonoBehaviour
    {
        //Tutorial Followed :
        //https://www.youtube.com/watch?v=9A9yj8KnM8c

        [SerializeField] float _xRange = 1f;
        [SerializeField] float _yRange = 0.5f;
        
        public IEnumerator ShakeCamera(float duration, float strength)
        {
            Vector3 originalPos = Camera.main.transform.position;

            float timeSpent = 0.0f;

            while (timeSpent < duration)
            {
                float x = Random.Range(-_xRange, _xRange) * strength;
                float y = Random.Range(-_yRange, _yRange) * strength;
                float z = Random.Range(-_yRange, _yRange) * strength;

                Camera.main.transform.position += new Vector3(x, y, z);
                timeSpent += Time.deltaTime;

                yield return null;
            }
            Camera.main.transform.position = originalPos;
        }
    }
}
