using System;
using UnityEngine;

namespace Implementations.Managers
{
    public class PanelManager : MonoBehaviour
    {
        public static PanelManager Instance { private set; get; }

        public GameObject credits;
        public GameObject gameOver;
        public GameObject pauseGame;
        public GameObject settings;
        public GameObject techniques;
        
        private void Awake()
        {
            Instance = this;
        }

        public void CloseAllPanels()
        {
            credits.SetActive(false);
            gameOver.SetActive(false);
            pauseGame.SetActive(false);
            settings.SetActive(false);
            techniques.SetActive(false);
        }

        public void SwapPanel(Panel target)
        {
            CloseAllPanels();
            switch (target)
            {
                case Panel.Credits:
                    credits.SetActive(true);
                    break;
                case Panel.GameOver:
                    gameOver.SetActive(true);
                    break;
                case Panel.PauseGame:
                    pauseGame.SetActive(true);
                    break;
                case Panel.Settings:
                    settings.SetActive(true);
                    break;
                case Panel.Techniques:
                    techniques.SetActive(true);
                    break;
            }
        }
    }
}