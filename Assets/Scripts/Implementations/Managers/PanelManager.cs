using System;
using System.Collections;
using BaseClasses;
using UnityEngine;

namespace Implementations.Managers
{
    public class PanelManager : MonoBehaviour
    {
        public static PanelManager Instance { private set; get; }

        public GameObject credits;
        public GameObject gameWon;
        public GameObject gameLost;
        public GameObject pauseGame;
        public GameObject settings;
        public GameObject techniques;

        public Panel activePanel;

        private void Awake()
        {
            Instance = this;
        }
        

        public void CloseAllPanels()
        {
            credits.SetActive(false);
            gameWon.SetActive(false);
            gameLost.SetActive(false);
            pauseGame.SetActive(false);
            settings.SetActive(false);
            techniques.SetActive(false);
            activePanel = Panel.None;
        }

        public void SwapPanel(Panel target)
        {
            CloseAllPanels();
            switch (target)
            {
                case Panel.Credits:
                    credits.SetActive(true);
                    break;
                case Panel.GameLost:
                    gameLost.SetActive(true);
                    break;
                case Panel.GameWon:
                    gameWon.SetActive(true);
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

            activePanel = target;
        }
    }
}