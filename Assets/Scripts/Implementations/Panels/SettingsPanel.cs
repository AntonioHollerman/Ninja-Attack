using System;
using BaseClasses;
using Implementations.Managers;
using UnityEngine;

namespace Implementations.Panels
{
    public class SettingsPanel : MonoBehaviour
    {
        

        public void BackButton()
        {
            PanelManager.Instance.SwapPanel(Panel.PauseGame);
        }

        public void ResumeButton()
        {
            PanelManager.Instance.CloseAllPanels();    
        }
    }
}