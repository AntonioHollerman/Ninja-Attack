using Implementations.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Implementations.Panels
{
    public class GameOverPanel : MonoBehaviour
    {
        public void Restart()
        {
            SceneManager.LoadScene(1);
        }

        public void MainMenu()
        {
            SceneManager.LoadScene(0);
        }

        public void Quit()
        {
            PanelManager.Instance.SwapPanel(Panel.Credits);
        }
    }
}