using UnityEngine;

namespace MaquestiauxMark.Hades
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _pauseCanvasRef;
        [SerializeField] private GameObject _canvasUIRef;
        [SerializeField] private GameObject _deathCanvasRef;

        public void ActivatePauseCanvas()
        {
            Time.timeScale = 0;
            _pauseCanvasRef.SetActive(true);
            _canvasUIRef.SetActive(false);
            _deathCanvasRef.SetActive(false);
        }
        public void ActivateDeathCanvas()
        {
            Time.timeScale = 0;
            _pauseCanvasRef.SetActive(false);
            _canvasUIRef.SetActive(false);
            _deathCanvasRef.SetActive(true);
        }
        public void ActivateUICanvas()
        {
            Time.timeScale = 1f;
            _pauseCanvasRef.SetActive(false);
            _canvasUIRef.SetActive(true);
            _deathCanvasRef.SetActive(false);
        }
    }
}
