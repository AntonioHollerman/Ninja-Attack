using System;
using System.Collections.Generic;
using BaseClasses;
using Implementations.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Implementations.Panels
{
    public class PauseGamePanel : MonoBehaviour
    {
        

        public void QuitButton()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        public void ContinueButton()
        {
            PanelManager.Instance.CloseAllPanels();
        }

        public void SettingsButton()
        {
            PanelManager.Instance.SwapPanel(Panel.Settings);
        }

        public void TechniqueButton()
        {
            PanelManager.Instance.SwapPanel(Panel.Techniques);
        }

        public void MainMenuButton()
        {
            SceneManager.LoadScene(0);
        }
    }
}