using System;
using System.Collections;
using System.Collections.Generic;
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
        public bool Active => activePanel != Panel.None;

        private List<Panel> _ignorePausePanels = new() { Panel.GameWon, Panel.GameLost, Panel.Credits };
        private void Awake()
        {
            Instance = this;
            CloseAllPanels();
        }

        private void Update()
        {
            CharacterSheet.UniversalStopCsUpdateLoop = Active;
            
            if (!Input.GetKeyDown(KeyCode.Escape) || _ignorePausePanels.Contains(activePanel))
            {
                return;
            }
            
            if (Active)
            {
                CloseAllPanels();
            }
            else
            {
                SwapPanel(Panel.PauseGame);
            }
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