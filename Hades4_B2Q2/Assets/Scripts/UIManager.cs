using UnityEngine;

namespace MaquestiauxMark.Hades
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _pauseCanvasRef;
        [SerializeField] private GameObject _canvasUIRef;
        [SerializeField] private GameObject _deathCanvasRef;
        [SerializeField] private GameObject _winCanvasRef;

        public void ActivatePauseCanvas()
        {
            DeactivateAllCanvas();
            _pauseCanvasRef.SetActive(true);
        }
        public void ActivateDeathCanvas()
        {
            DeactivateAllCanvas();
            _deathCanvasRef.SetActive(true);
        }
        public void ActivateWinCanvas()
        {
            DeactivateAllCanvas();
            _winCanvasRef.SetActive(true);
        }
        public void ActivateUICanvas()
        {
            DeactivateAllCanvas();
            Time.timeScale = 1f;
            _canvasUIRef.SetActive(true);
        }



        private void DeactivateAllCanvas()
        {
            Time.timeScale = 0;
            _winCanvasRef.SetActive(false);
            _pauseCanvasRef.SetActive(false);
            _canvasUIRef.SetActive(false);
            _deathCanvasRef.SetActive(false);
        }
    }
}
