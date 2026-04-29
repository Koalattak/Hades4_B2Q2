using UnityEngine;
using UnityEngine.SceneManagement;

namespace MaquestiauxMark.Hades
{
    public class GameSceneManager : MonoBehaviour
    {
        [SerializeField] private string _gameSceneString;
        [SerializeField] private string _menuSceneString;
        
        public void QuitGame()
        {
            Application.Quit();
        }

        public void StartGame()
        {
            SceneManager.LoadScene(_gameSceneString);
        }
        public void StartMainMenu()
        {
            SceneManager.LoadScene(_menuSceneString);
        }
    }
}
